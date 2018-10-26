using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using System.Linq;
using TEDCore.Resource;

namespace TEDCore.AssetBundle
{
    public class AssetBundleSystem : MonoSingleton<AssetBundleSystem>
    {
        public class LoadedAssetBundle
        {
            public UnityEngine.AssetBundle Bundle;

            public LoadedAssetBundle(UnityEngine.AssetBundle assetBundle)
            {
                Bundle = assetBundle;
            }

#if UNITY_EDITOR
            public UnityEngine.Object SimulateObject;
            public LoadedAssetBundle(UnityEngine.Object asset)
            {
                SimulateObject = asset;
            }

            public UnityEngine.Object[] SimulateObjects;
            public LoadedAssetBundle(UnityEngine.Object[] assets)
            {
                SimulateObjects = assets;
            }
#endif
        }

        private int m_maxDownloadRequest = 1;
        private AssetBundleLoadType m_assetBundleLoadType;
        private string m_downloadingURL;

        private AssetBundleCatalogs m_assetBundleCatalogs;

        public bool Initialized
        {
            get { return m_assetBundleManifest != null; }
        }

        private AssetBundleManifest m_assetBundleManifest;
        private Action<bool> m_onAssetBundleManifestLoaded;

        private Action<SingleAssetBundleDownloadInfo> m_onSingleAssetBundleProgressChanged;
        private Action<TotalAssetBundleDownloadInfo> m_onTotalAssetBundleProgressChanged;
        private TotalAssetBundleDownloadInfo m_totalAssetBundleDownloadInfo;

        private Dictionary<string, LoadedAssetBundle> m_loadedAssetBundles = new Dictionary<string, LoadedAssetBundle>();
        private Dictionary<string, UnityWebRequest> m_downloadingRequests = new Dictionary<string, UnityWebRequest>();
        private Dictionary<string, string> m_downloadingErrors = new Dictionary<string, string>();
        private List<AssetBundleLoadRequest> m_inProgressRequests = new List<AssetBundleLoadRequest>();
        private Dictionary<string, string[]> m_dependencies = new Dictionary<string, string[]>();
        private List<string> m_completeDownloadAssetBundles = new List<string>();
        private UnityWebRequest m_cacheRequest;

        private List<string> m_waitingDownloadAssetBundleNames = new List<string>();

        private class WaitingDownloadRequest
        {
            public string AssetBundleName;
            public bool IsManifest;
        }
        private Queue<WaitingDownloadRequest> m_waitingDownloadRequests = new Queue<WaitingDownloadRequest>();

        protected override void OnDestroy()
        {
            base.OnDestroy();

            foreach (var downloadingRequest in m_downloadingRequests)
            {
                downloadingRequest.Value.Dispose();
            }
        }

        private void Update()
        {
            UpdateWaitingDownloadRequests();
            UpdateDownloadingRequests();
            UpdateCompleteDownloadRequests();
        }


        private void UpdateWaitingDownloadRequests()
        {
            if (m_waitingDownloadRequests.Count == 0 ||
                m_downloadingRequests.Count() >= m_maxDownloadRequest)
            {
                return;
            }

            for (int i = m_downloadingRequests.Count(); i < m_maxDownloadRequest; i++)
            {
                if (m_waitingDownloadRequests.Count == 0)
                {
                    break;
                }

                var waitingDownloadRequest = m_waitingDownloadRequests.Dequeue();

                UnityWebRequest request = null;
                var url = m_downloadingURL + waitingDownloadRequest.AssetBundleName;

                if (waitingDownloadRequest.IsManifest)
                {
                    request = UnityWebRequestAssetBundle.GetAssetBundle(url);
                }
                else
                {
                    TEDDebug.LogFormat("[AssetBundleSystem] - Start downloading asset bundle '{0}' at frame {1}", waitingDownloadRequest.AssetBundleName, Time.frameCount);
                    request = UnityWebRequestAssetBundle.GetAssetBundle(url, m_assetBundleManifest.GetAssetBundleHash(waitingDownloadRequest.AssetBundleName), 0);
                }

                request.SendWebRequest();

                m_downloadingRequests.Add(waitingDownloadRequest.AssetBundleName, request);
                m_waitingDownloadAssetBundleNames.Remove(waitingDownloadRequest.AssetBundleName);
            }
        }


        private void UpdateDownloadingRequests()
        {
            m_completeDownloadAssetBundles.Clear();

            foreach (var keyValue in m_downloadingRequests)
            {
                CachedAssetBundle(keyValue.Key, keyValue.Value);
            }
        }


        private void CachedAssetBundle(string assetBundleName, UnityWebRequest unityWebRequest)
        {
            if (unityWebRequest.isDone)
            {
                if (HasRequestError(assetBundleName, unityWebRequest))
                {
                    return;
                }

                var assetBundle = DownloadHandlerAssetBundle.GetContent(unityWebRequest);
                if (assetBundle != null)
                {
                    m_loadedAssetBundles.Add(assetBundleName, new LoadedAssetBundle(assetBundle));
                }
                else
                {
                    AddDownloadError(assetBundleName, unityWebRequest.error);
                }

                m_completeDownloadAssetBundles.Add(assetBundleName);
            }
            else
            {
                if (m_onSingleAssetBundleProgressChanged != null)
                {
                    m_onSingleAssetBundleProgressChanged(new SingleAssetBundleDownloadInfo(unityWebRequest.downloadProgress, m_assetBundleCatalogs.GetFileSize(assetBundleName)));
                }
            }
        }


        private bool HasRequestError(string assetBundleName, UnityWebRequest unityWebRequest)
        {
            if (unityWebRequest.isHttpError || unityWebRequest.isNetworkError || !string.IsNullOrEmpty(unityWebRequest.error))
            {
                AddDownloadError(assetBundleName, unityWebRequest.error);
                m_completeDownloadAssetBundles.Add(assetBundleName);

                return true;
            }

            return false;
        }


        private void AddDownloadError(string assetBundleName, string error)
        {
            if (m_downloadingErrors.ContainsKey(assetBundleName))
            {
                m_downloadingErrors[assetBundleName] = error;
            }
            else
            {
                m_downloadingErrors.Add(assetBundleName, error);
            }
        }


        private void UpdateCompleteDownloadRequests()
        {
            foreach (string key in m_completeDownloadAssetBundles)
            {
                TEDDebug.LogFormat("[AssetBundleSystem] - Download asset bundle '{0}' successfully at frame {1}", key, Time.frameCount);

                m_cacheRequest = m_downloadingRequests[key];
                m_cacheRequest.Dispose();
                m_downloadingRequests.Remove(key);
            }

            if (m_totalAssetBundleDownloadInfo != null)
            {
                m_totalAssetBundleDownloadInfo.SetDownloadCount(m_downloadingRequests.Count + m_waitingDownloadRequests.Count);

                if (m_totalAssetBundleDownloadInfo.Progress == 1)
                {
                    TEDDebug.LogFormat("[AssetBundleSystem] - Download AssetBundle complete at frame {0}", Time.frameCount);
                    ResourceSystem.Instance.Clear();
                }

                if (m_onTotalAssetBundleProgressChanged != null)
                {
                    m_onTotalAssetBundleProgressChanged(m_totalAssetBundleDownloadInfo);
                }
            }

            if (m_downloadingRequests.Count == 0 && m_waitingDownloadRequests.Count == 0)
            {
                m_onSingleAssetBundleProgressChanged = null;
                m_onTotalAssetBundleProgressChanged = null;
                m_totalAssetBundleDownloadInfo = null;
            }

            m_inProgressRequests.RemoveAll(request => !request.Update());
        }


        private void ReassignShader(UnityEngine.AssetBundle assetBundle)
        {
            if (assetBundle.isStreamedSceneAssetBundle)
            {
                return;
            }

            var assets = assetBundle.LoadAllAssets<Material>();
            Shader cacheShader = null;
            for (int i = 0; i < assets.Length; i++)
            {
                cacheShader = assets[i].shader;
                if (cacheShader == null)
                {
                    continue;
                }

                assets[i].shader = Shader.Find(cacheShader.name);
            }
        }


        public void Initialize(AssetBundleInitializeData initializeData)
        {
            m_maxDownloadRequest = initializeData.MaxDownloadRequest;
            m_onAssetBundleManifestLoaded = initializeData.OnManifestLoaded;
            m_assetBundleLoadType = initializeData.LoadType;

            TEDDebug.LogFormat("[AssetBundleSystem] - Initialize with load type '{0}'", m_assetBundleLoadType);

            if (m_assetBundleLoadType == AssetBundleLoadType.Streaming)
            {
                initializeData.DownloadURL = AssetBundleDef.GetDownloadStreamingAssetsPath();
            }

            if (m_assetBundleLoadType != AssetBundleLoadType.Simulate)
            {
                StartCoroutine(PreInitialize(initializeData.DownloadURL));
            }
            else
            {
                if (m_onAssetBundleManifestLoaded != null)
                {
                    m_onAssetBundleManifestLoaded(true);
                    m_onAssetBundleManifestLoaded = null;
                }
            }
        }


        private IEnumerator PreInitialize(string relativePath)
        {
            var platformName = AssetBundleDef.GetPlatformName();

            m_downloadingURL = string.Format("{0}/{1}/", relativePath, platformName);
            TEDDebug.LogFormat("[AssetBundleSystem] - The AssetBundle download url is {0}", m_downloadingURL);

            yield return StartCoroutine(InitializeCatalog());

            if (m_assetBundleCatalogs == null)
            {
                if (m_onAssetBundleManifestLoaded != null)
                {
                    m_onAssetBundleManifestLoaded(false);
                    m_onAssetBundleManifestLoaded = null;
                    yield break;
                }
            }

            var request = InitializeManifest(platformName);
            if (request != null)
            {
                yield return StartCoroutine(request);
            }
        }


        private IEnumerator<float> InitializeCatalog()
        {
            TEDDebug.LogFormat("[AssetBundleSystem] - Start download AssetBundleCatalog at frame {0}", Time.frameCount);

            UnityWebRequest request = UnityWebRequest.Get(m_downloadingURL + AssetBundleDef.CATALOG_FILE_NAME);
            request.SendWebRequest();

            while (!request.isDone)
            {
                yield return 0;
            }

            if (!string.IsNullOrEmpty(request.error))
            {
                m_assetBundleCatalogs = null;
                TEDDebug.LogErrorFormat("[AssetBundleSystem] - Download AssetBundleCatalog failed. Error log \"{0}\"", request.error);
            }
            else
            {
                m_assetBundleCatalogs = new AssetBundleCatalogs(request.downloadHandler.text);
                TEDDebug.LogFormat("[AssetBundleSystem] - Download AssetBundleCatalog complete at frame {0}", Time.frameCount);
            }

            request.Dispose();
        }


        private AssetBundleLoadManifestRequest InitializeManifest(string path)
        {
            UnloadAssetBundles(new List<string> { AssetBundleDef.GetPlatformName() });

            TEDDebug.LogFormat("[AssetBundleSystem] - Start download AssetBundleManifest at frame {0}", Time.frameCount);

            DownloadAssetBundle(path, true);

            var request = new AssetBundleLoadManifestRequest(path, "AssetBundleManifest");
            m_inProgressRequests.Add(request);

            return request;
        }


        public void SetupManifest(AssetBundleManifest manifest)
        {
            m_assetBundleManifest = manifest;
            if (m_onAssetBundleManifestLoaded != null)
            {
                m_onAssetBundleManifestLoaded(m_assetBundleManifest != null);
                m_onAssetBundleManifestLoaded = null;
            }
        }


        public void Download(Action<SingleAssetBundleDownloadInfo> onSingleAssetBundleDownloadProgressChanged, Action<TotalAssetBundleDownloadInfo> onTotalAssetBundleProgressChanged)
        {
            m_onSingleAssetBundleProgressChanged = onSingleAssetBundleDownloadProgressChanged;
            m_onTotalAssetBundleProgressChanged = onTotalAssetBundleProgressChanged;

            if (m_assetBundleLoadType == AssetBundleLoadType.Simulate)
            {
                TEDDebug.LogFormat("[AssetBundleSystem] - The AssetBundle load type is on Simulate, don't need to download.");
                return;
            }

            if (null == m_assetBundleManifest)
            {
                TEDDebug.LogError("[AssetBundleSystem] - Please download AssetBundleManifest by calling ResourceSystem.InstancenitAssetBundle() first");
                return;
            }

            TEDDebug.LogFormat("[AssetBundleSystem] - Start download AssetBundle at frame {0}", Time.frameCount);

            var allAssetBundles = m_assetBundleManifest.GetAllAssetBundles();
            var downloadAssetBundleNames = new List<string>();

            for (int i = 0; i < allAssetBundles.Length; i++)
            {
                if (Caching.IsVersionCached(m_downloadingURL + allAssetBundles[i], m_assetBundleManifest.GetAssetBundleHash(allAssetBundles[i])))
                {
                    continue;
                }

                downloadAssetBundleNames.Add(allAssetBundles[i]);
            }

            UnloadAssetBundles(allAssetBundles.ToList());

            for (int i = 0; i < allAssetBundles.Length; i++)
            {
                DownloadAssetBundle(allAssetBundles[i], false);
            }

            m_totalAssetBundleDownloadInfo = new TotalAssetBundleDownloadInfo(allAssetBundles.Length, m_assetBundleCatalogs.GetAllFileSize(downloadAssetBundleNames));
        }


        public AssetBundleLoadAssetRequest<T> LoadAssetAsync<T>(string assetBundleName, string assetName) where T : UnityEngine.Object
        {
            AssetBundleLoadAssetRequest<T> request = null;

#if UNITY_EDITOR
            if (m_assetBundleLoadType == AssetBundleLoadType.Simulate)
            {
                request = new AssetBundleLoadAssetRequestSimulate<T>(assetBundleName, assetName);
            }
            else
#endif
            {
                LoadFromNetwork(assetBundleName);
                request = new AssetBundleLoadAssetRequestFull<T>(assetBundleName, assetName);
            }

            m_inProgressRequests.Add(request);

            return request;
        }


        public AssetBundleLoadAllAssetRequest<T> LoadAllAssetAsync<T>(string assetBundleName, string assetName) where T : UnityEngine.Object
        {
            AssetBundleLoadAllAssetRequest<T> request = null;

#if UNITY_EDITOR
            if (m_assetBundleLoadType == AssetBundleLoadType.Simulate)
            {
                request = new AssetBundleLoadAllAssetRequestSimulate<T>(assetBundleName, assetName);
            }
            else
#endif
            {
                LoadFromNetwork(assetBundleName);
                request = new AssetBundleLoadAllAssetRequestFull<T>(assetBundleName);
            }

            m_inProgressRequests.Add(request);

            return request;
        }


        public AssetBundleLoadSceneRequest LoadSceneAsync(string assetBundleName, string sceneName, bool isAdditive)
        {
            AssetBundleLoadSceneRequest request = null;

            LoadFromNetwork(assetBundleName);
            request = new AssetBundleLoadSceneRequest(assetBundleName, sceneName, isAdditive);

            m_inProgressRequests.Add(request);

            return request;
        }


        private void LoadFromNetwork(string assetBundleName)
        {
            LoadDependenciesFromNetwork(assetBundleName);
            LoadAssetBundleFromNetwork(assetBundleName);
        }


        private bool LoadAssetBundleFromNetwork(string assetBundleName)
        {
            LoadedAssetBundle bundle = null;
            m_loadedAssetBundles.TryGetValue(assetBundleName, out bundle);
            if (bundle != null)
            {
                return true;
            }

            return DownloadAssetBundle(assetBundleName, false);
        }


        private bool DownloadAssetBundle(string assetBundleName, bool isLoadingAssetBundleManifest)
        {
            if (m_loadedAssetBundles.ContainsKey(assetBundleName) && m_loadedAssetBundles[assetBundleName].Bundle != null)
            {
                return true;
            }

            if (m_waitingDownloadAssetBundleNames.Contains(assetBundleName))
            {
                return true;
            }

            // @TODO: Do we need to consider the referenced count of WWWs?
            // In the demo, we never have duplicate WWWs as we wait LoadAssetAsync()/LoadLevelAsync() to be finished before calling another LoadAssetAsync()/LoadLevelAsync().
            // But in the real case, users can call LoadAssetAsync()/LoadLevelAsync() several times then wait them to be finished which might have duplicate WWWs.
            if (m_downloadingRequests.ContainsKey(assetBundleName))
            {
                return true;
            }

            m_waitingDownloadAssetBundleNames.Add(assetBundleName);
            m_waitingDownloadRequests.Enqueue(new WaitingDownloadRequest()
            {
                AssetBundleName = assetBundleName,
                IsManifest = isLoadingAssetBundleManifest
            });

            return false;
        }


        private void LoadDependenciesFromNetwork(string assetBundleName)
        {
            if (m_dependencies.ContainsKey(assetBundleName))
            {
                return;
            }

            var dependencies = m_assetBundleManifest.GetAllDependencies(assetBundleName);
            if (dependencies.Length == 0)
            {
                return;
            }

            m_dependencies.Add(assetBundleName, dependencies);
            for (int i = 0; i < dependencies.Length; i++)
            {
                LoadAssetBundleFromNetwork(dependencies[i]);
            }
        }


        private void UnloadAssetBundles(List<string> bundleNames)
        {
            foreach (var downloadingRequest in m_downloadingRequests)
            {
                downloadingRequest.Value.Dispose();
            }
            m_downloadingRequests.Clear();

            foreach (var bundleName in bundleNames)
            {
                UnloadAssetBundle(bundleName);
            }

            m_inProgressRequests.Clear();
            m_waitingDownloadRequests.Clear();
            m_waitingDownloadAssetBundleNames.Clear();

            foreach (var bundle in Resources.FindObjectsOfTypeAll<UnityEngine.AssetBundle>())
            {
                if (bundleNames.Contains(bundle.name))
                {
                    bundle.Unload(false);
                }
            }

            Resources.UnloadUnusedAssets();
        }


        private void UnloadAssetBundle(string bundleName)
        {
            UnloadAssetBundleInternal(bundleName);
            UnloadDependencies(bundleName);
        }


        private void UnloadAssetBundleInternal(string bundleName)
        {
            var error = string.Empty;
            var bundle = GetLoadedAssetBundle(bundleName, out error);

            if (bundle != null && bundle.Bundle != null)
            {
                bundle.Bundle.Unload(false);
                bundle.Bundle = null;
            }

            m_loadedAssetBundles.Remove(bundleName);
            m_downloadingErrors.Remove(bundleName);
            m_completeDownloadAssetBundles.Remove(bundleName);
        }


        private void UnloadDependencies(string bundleName)
        {
            string[] dependencies = null;
            if (!m_dependencies.TryGetValue(bundleName, out dependencies))
            {
                return;
            }

            foreach (var dependency in dependencies)
            {
                UnloadAssetBundleInternal(dependency);
            }

            m_dependencies.Remove(bundleName);
        }


        public LoadedAssetBundle GetLoadedAssetBundle(string assetBundleName, out string error)
        {
            if (m_downloadingErrors.TryGetValue(assetBundleName, out error))
            {
                return null;
            }

            LoadedAssetBundle bundle = null;
            m_loadedAssetBundles.TryGetValue(assetBundleName, out bundle);
            if (null == bundle)
            {
                return null;
            }

            string[] dependencies = null;
            if (!m_dependencies.TryGetValue(assetBundleName, out dependencies))
            {
                return bundle;
            }

            foreach (var dependency in dependencies)
            {
                if (m_downloadingErrors.TryGetValue(assetBundleName, out error))
                {
                    return bundle;
                }

                LoadedAssetBundle dependentBundle;
                m_loadedAssetBundles.TryGetValue(dependency, out dependentBundle);
                if (null == dependentBundle)
                {
                    return null;
                }
            }

            return bundle;
        }
    }
}