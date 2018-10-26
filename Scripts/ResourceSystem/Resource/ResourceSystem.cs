using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using TEDCore.AssetBundle;

namespace TEDCore.Resource
{
    public class ResourceSystem : Singleton<ResourceSystem>
    {
        public class LoadedResource
        {
            public UnityEngine.Object m_resource;
            public UnityEngine.Object[] m_resources;
            public int m_referencedCount;

            public LoadedResource(UnityEngine.Object resource)
            {
                m_resource = resource;
                m_referencedCount = 0;
            }


            public LoadedResource(UnityEngine.Object[] resources)
            {
                m_resources = resources;
                m_referencedCount = 0;
            }
        }

        [SerializeField]
        private Dictionary<string, LoadedResource> m_loadedResources = new Dictionary<string, LoadedResource>();
        private Dictionary<string, int> m_asyncLoadingReferencedCounts = new Dictionary<string, int>();

        private bool IsPrefab<T>() where T : UnityEngine.Object
        {
            return typeof(T) == typeof(GameObject) || typeof(T).IsSubclassOf(typeof(Component));
        }

        private string GetCacheKey<T>(string assetBundleName, string assetName) where T : UnityEngine.Object
        {
            if (IsPrefab<T>())
            {
                return string.Format("{0}/{1}.prefab", assetBundleName, assetName);
            }

            return string.Format("{0}/{1}.{2}", assetBundleName, assetName, typeof(T).Name);
        }


        private bool InCache<T>(string assetBundleName, string assetName) where T : UnityEngine.Object
        {
            return m_loadedResources.ContainsKey(GetCacheKey<T>(assetBundleName, assetName));
        }


        public void Unload<T>(string assetName) where T : UnityEngine.Object
        {
            Unload<T>(string.Empty, assetName);
        }


        public void Unload<T>(string assetBundleName, string assetName) where T : UnityEngine.Object
        {
            if (InCache<T>(assetBundleName, assetName))
            {
                var res = m_loadedResources[GetCacheKey<T>(assetBundleName, assetName)];
                res.m_referencedCount--;

                Release();
            }
        }


        public void Release()
        {
            var removeResources = new List<string>();

            foreach (KeyValuePair<string, LoadedResource> kvp in m_loadedResources)
            {
                if (m_asyncLoadingReferencedCounts.ContainsKey(kvp.Key))
                {
                    continue;
                }

                if (kvp.Value.m_referencedCount <= 0)
                {
                    removeResources.Add(kvp.Key);
                }
            }


            for (int i = 0; i < removeResources.Count; i++)
            {
                m_loadedResources.Remove(removeResources[i]);
            }

            Resources.UnloadUnusedAssets();
            System.GC.Collect();
        }


        public void Clear()
        {
            m_loadedResources.Clear();
            Resources.UnloadUnusedAssets();
            System.GC.Collect();
        }


        #region LoadAsync
        public void LoadAsync<T>(string assetName, Action<T> callback, bool unloadAutomatically = true) where T : UnityEngine.Object
        {
            LoadAsync<T>(string.Empty, assetName, callback, unloadAutomatically);
        }


        public void LoadAsync<T>(string assetBundleName, string assetName, Action<T> callback, bool unloadAutomatically = true) where T : UnityEngine.Object
        {
            StartCoroutine(StartLoadAsync<T>(assetBundleName, assetName, callback, unloadAutomatically));
        }


        private IEnumerator StartLoadAsync<T>(string assetBundleName, string assetName, Action<T> callback, bool unloadAutomatically) where T : UnityEngine.Object
        {
            if (IsPrefab<T>())
            {
                yield return StartCoroutine(PreloadAsync<GameObject>(assetBundleName, assetName));
            }
            else
            {
                yield return StartCoroutine(PreloadAsync<T>(assetBundleName, assetName));
            }

            if (!InCache<T>(assetBundleName, assetName))
            {
                Debug.LogErrorFormat("[ResourceSystem] - Failed to load asset '{0}' from bundle '{1}' async, the asset didn't exist in the project.", assetName, assetBundleName);

                if (null != callback)
                {
                    callback(null);
                }
                yield break;
            }

            var res = m_loadedResources[GetCacheKey<T>(assetBundleName, assetName)];
            res.m_referencedCount++;

            T asset = res.m_resource as T;
            if (asset == null)
            {
                asset = (res.m_resource as GameObject).GetComponent<T>();
            }

            if (null != callback)
            {
                callback(asset);
            }

            if(unloadAutomatically)
            {
                Unload<T>(assetBundleName, assetName);
            }
        }


        private IEnumerator PreloadAsync<T>(string assetBundleName, string assetName) where T : UnityEngine.Object
        {
            AddAsyncLoadingReferencedCounts(GetCacheKey<T>(assetBundleName, assetName));

            //Preload the asset from AssetBundle async
            if (!string.IsNullOrEmpty(assetBundleName))
            {
                yield return StartCoroutine(PreloadAssetBundleAsync<T>(assetBundleName, assetName));
            }

            //Preload the asset from Resources async
            if (!InCache<T>(assetBundleName, assetName))
            {
                yield return StartCoroutine(PreloadResourceAsync<T>(assetName));
            }

            RemoveAsyncLoadingReferencedCounts(GetCacheKey<T>(assetBundleName, assetName));
        }


        private void AddAsyncLoadingReferencedCounts(string path)
        {
            if (m_asyncLoadingReferencedCounts.ContainsKey(path))
            {
                m_asyncLoadingReferencedCounts[path] = m_asyncLoadingReferencedCounts[path] + 1;
            }
            else
            {
                m_asyncLoadingReferencedCounts.Add(path, 1);
            }
        }


        private void RemoveAsyncLoadingReferencedCounts(string path)
        {
            if (m_asyncLoadingReferencedCounts.ContainsKey(path))
            {
                m_asyncLoadingReferencedCounts[path]--;

                if (m_asyncLoadingReferencedCounts[path] == 0)
                {
                    m_asyncLoadingReferencedCounts.Remove(path);
                }
            }
        }


        private IEnumerator PreloadResourceAsync<T>(string assetName) where T : UnityEngine.Object
        {
            if (InCache<T>(string.Empty, assetName))
            {
                yield break;
            }

            var startFrameCount = Time.frameCount;

            var resourceRequest = Resources.LoadAsync<T>(assetName);
            while (!resourceRequest.isDone)
            {
                yield return 0;
            }

            var res = new LoadedResource(resourceRequest.asset);

            if (null == res.m_resource)
            {
                yield break;
            }

            m_loadedResources[GetCacheKey<T>(string.Empty, assetName)] = res;

            Debug.LogFormat("[ResourceSystem] - Loading Asset '{0}' has done from frame {1} to frame {2}.", assetName, startFrameCount, Time.frameCount);
        }


        private IEnumerator PreloadAssetBundleAsync<T>(string assetBundleName, string assetName) where T : UnityEngine.Object
        {
            if (InCache<T>(assetBundleName, assetName))
            {
                yield break;
            }

            var startFrameCount = Time.frameCount;

            var request = AssetBundleSystem.Instance.LoadAssetAsync<T>(assetBundleName, assetName);
            if (null == request)
            {
                yield break;
            }

            yield return StartCoroutine(request);

            var asset = request.GetAsset();

            var res = new LoadedResource(asset);

            if (null == res.m_resource)
            {
                yield break;
            }

            m_loadedResources[GetCacheKey<T>(assetBundleName, assetName)] = res;

            Debug.LogFormat("[ResourceSystem] - Loading AssetBundle '{0}' has done from frame {1} to frame {2}.", assetName, startFrameCount, Time.frameCount);
        }
        #endregion


        #region LoadAllAsync
        public void LoadAllAsync<T>(string assetName, Action<List<T>> callback, bool unloadAutomatically = true) where T : UnityEngine.Object
        {
            LoadAllAsync<T>(string.Empty, assetName, callback, unloadAutomatically);
        }


        public void LoadAllAsync<T>(string assetBundleName, string assetName, Action<List<T>> callback, bool unloadAutomatically = true) where T : UnityEngine.Object
        {
            StartCoroutine(StartLoadAllAsync<T>(assetBundleName, assetName, callback, unloadAutomatically));
        }


        private IEnumerator StartLoadAllAsync<T>(string assetBundleName, string assetName, Action<List<T>> callback, bool unloadAutomatically) where T : UnityEngine.Object
        {
            yield return StartCoroutine(PreloadAllAsync<T>(assetBundleName, assetName));

            if (!InCache<T>(assetBundleName, assetName))
            {
                Debug.LogErrorFormat("[ResourceSystem] - Failed to load asset '{0}' from bundle '{1}' async, the asset didn't exist in the project.", assetName, assetBundleName);

                if (null != callback)
                {
                    callback(null);
                }
                yield break;
            }

            var res = m_loadedResources[GetCacheKey<T>(assetBundleName, assetName)];
            res.m_referencedCount++;

            var assets = new List<T>();
            T cache = null;

            for (int i = 0; i < res.m_resources.Length; i++)
            {
                cache = (T)res.m_resources[i];
                if (cache)
                {
                    assets.Add(cache);
                }
            }

            if (null != callback)
            {
                callback(assets);
            }

            if(unloadAutomatically)
            {
                Unload<T>(assetBundleName, assetName);
            }
        }


        private IEnumerator PreloadAllAsync<T>(string assetBundleName, string assetName) where T : UnityEngine.Object
        {
            AddAsyncLoadingReferencedCounts(GetCacheKey<T>(assetBundleName, assetName));

            //Preload the asset from AssetBundle async
            if (!string.IsNullOrEmpty(assetBundleName))
            {
                yield return StartCoroutine(PreloadAllAssetBundleAsync<T>(assetBundleName, assetName));
            }

            //Preload the asset from Resources async
            if (!InCache<T>(assetBundleName, assetName))
            {
                T[] assets = Resources.LoadAll<T>(assetName);
                var res = new LoadedResource(assets);
                m_loadedResources[GetCacheKey<T>(assetBundleName, assetName)] = res;
            }

            RemoveAsyncLoadingReferencedCounts(GetCacheKey<T>(assetBundleName, assetName));
        }


        private IEnumerator PreloadAllAssetBundleAsync<T>(string assetBundleName, string assetName) where T : UnityEngine.Object
        {
            if (InCache<T>(assetBundleName, assetName))
            {
                yield break;
            }

            var startFrameCount = Time.frameCount;

            AssetBundleLoadAllAssetRequest<T> request = AssetBundleSystem.Instance.LoadAllAssetAsync<T>(assetBundleName, assetName);
            if (null == request)
            {
                yield break;
            }

            yield return StartCoroutine(request);

            var assets = request.GetAsset();

            var res = new LoadedResource(assets);

            if (null == res.m_resources || res.m_resources.Length == 0)
            {
                yield break;
            }

            m_loadedResources[GetCacheKey<T>(assetBundleName, assetName)] = res;

            Debug.LogFormat("[ResourceSystem] - Loading AssetBundle '{0}' has done from frame {1} to frame {2}.", assetName, startFrameCount, Time.frameCount);
        }
        #endregion
    }
}

