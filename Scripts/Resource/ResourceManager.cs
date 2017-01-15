using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TEDCore.Utils;

namespace TEDCore.Resource
{
	public class ResourceManager : MonoBehaviour
	{
		private class Resource
		{
			public object Res;
			public int CheckoutCount;
		}
		
		
		private Dictionary<string, Resource> m_resources;
		
		private void Awake()
		{
			m_resources = new Dictionary<string, Resource>();
		}


		//Load the resource into the resource cache
		public void LoadResource(string name)
		{
			//Don't load resource twice
			if(m_resources.ContainsKey(name))
			{
				return;
			}
			
			Resource res = new Resource();
			res.Res = Resources.Load(name);			
			res.CheckoutCount = 0;
			
			if(res.Res == null)
			{
				Debug.LogError("[ResourceManager] - Failed to find resource '" + name + "'");
			}
			
			m_resources[name] = res;
		}


		//Get a previously loaded resource with an option to load it if it's not loaded
		//Call checkIn when you are done with the resource
		public T CheckOut<T>(string name, bool forceLoad = false) where T : class
		{
			if(forceLoad)
			{
				LoadResource(name);
			}

			if(m_resources.ContainsKey(name))
			{
				Resource res = m_resources[name];
				res.CheckoutCount++;
				
				return (T)res.Res;
			}
			
			return null;
		}
		
		
		//Checkout and create an instance
		public GameObject CheckOutAndInstantiate(string name, bool forceLoad = false)
		{
			GameObject go = CheckOut<GameObject>(name, forceLoad);
			
			if(go == null)
			{
				return null;
			}
			
			go = (GameObject)GameObject.Instantiate(go);
			go.name = name;
			return go;
		}
		
		
		//When you are done with a resource you should check it in
		public void CheckIn(string name)
		{
			if(m_resources.ContainsKey(name))
			{
				Resource res = m_resources[name];
				res.CheckoutCount--;

				Clean();
			}
		}
		
		//Checks in a game object and destroys it. If you changed the name of the gameobject the original needs to be given
		public void CheckInAndDestroy(GameObject go, string originalName = null)
		{
			if(go == null)
			{
				return;
			}
			
			MeshFilter mesh = go.GetComponent<MeshFilter>();
			
			if(mesh != null)
			{
				Mesh.Destroy(mesh.mesh);
			}			
			
			CheckIn(originalName == null ? go.name : originalName);
			GameObject.Destroy(go);
		}
		
		
		//Removes all resources that aren't checkout out
		public void Clean()
		{
			List<string> itemsToRemove = new List<string>();
			
			foreach(KeyValuePair<string, Resource> kvp in m_resources)
			{
				if(kvp.Value.CheckoutCount <= 0)
				{
					itemsToRemove.Add(kvp.Key);
				}
			}
			
			
			foreach(string key in itemsToRemove)
			{
				m_resources.Remove(key);
				GameSystemManager.Get<AssetBundleManager>().UnloadAssetBundle(key);
			}
			
			Resources.UnloadUnusedAssets();
			System.GC.Collect();
		}


		//Removes all resources
		public void Clear()
		{
			m_resources.Clear();
			Resources.UnloadUnusedAssets();
			System.GC.Collect();
		}


		#region Async
		public IEnumerator LoadResourceAsync(string name)
		{
			if(AssetBundleData.IsAssetBundle(name))
			{
				yield return StartCoroutine(LoadAssetBundleAsync(name));
			}

			if(m_resources.ContainsKey(name))
			{
				yield break;
			}

			ResourceRequest resourceRequest = Resources.LoadAsync(name);

			while(!resourceRequest.isDone)
			{
				yield return 0;
			}
			
			Resource res = new Resource();
			res.Res = (object)resourceRequest.asset;
			res.CheckoutCount = 0;
			
			if(res.Res == null)
			{
				Debug.LogError("[ResourceManager] - Failed to find resource '" + name + "'");
			}

			m_resources[name] = res;
		}


		public IEnumerator LoadResourceAsync(string name, System.Action<string> callback)
		{
			if(AssetBundleData.IsAssetBundle(name))
			{
				yield return StartCoroutine(LoadAssetBundleAsync(name, callback));
			}

			yield return StartCoroutine(LoadResourceAsync(name));

			Resource res = null;
			
			if(m_resources.ContainsKey(name))
			{
				res = m_resources[name];
				res.CheckoutCount++;
			}

			if (callback != null)
			{
				callback(name);
			}
		}


		public IEnumerator CheckOutAsync<T>(string name, System.Action<T> callback, bool forceLoad = false) where T : class
		{
			if(AssetBundleData.IsAssetBundle(name))
			{
				yield return StartCoroutine(CheckOutAssetBundleAsync<T>(name, callback, forceLoad));
			}

			if(forceLoad)
			{
				yield return StartCoroutine(LoadResourceAsync(name));
			}

			Resource res = null;

			if(m_resources.ContainsKey(name))
			{
				res = m_resources[name];
				res.CheckoutCount++;
			}
			
			if (callback != null)
			{
				callback((T)res.Res);
			}
		}
		#endregion


		#region AssetBundle Async
		public IEnumerator LoadAssetBundleAsync(string assetPath)
		{
			if(m_resources.ContainsKey(assetPath))
			{
				yield break;
			}

			string assetName = assetPath.Split ('/') [assetPath.Split ('/').Length - 1];
			Debug.LogWarning(string.Format("Start to load asset \"{0}\" at frame {1}", assetName, Time.frameCount));
			
			// Load asset from assetBundle.
			AssetBundleLoadAssetOperation request = GameSystemManager.Get<AssetBundleManager>().LoadAssetAsync(assetPath.ToLower() + ".assetbundle", assetName, typeof(Object) );
			if (request == null)
				yield break;
			yield return StartCoroutine(request);
			
			// Get the asset.
			Object asset = request.GetAsset<Object> ();
			Debug.LogWarning(string.Format("{0} to load asset \"{1}\" at frame {2}", asset == null ? "Failed" : "Succeeded", assetName, Time.frameCount));
			
			Resource res = new Resource();
			res.Res = (object)asset;
			res.CheckoutCount = 0;
			
			if(res.Res == null)
			{
				Debug.LogError("[ResourceManager] - Failed to find resource '" + assetName + "'");
			}
			
			m_resources[assetPath] = res;
		}
		
		
		public IEnumerator LoadAssetBundleAsync(string assetPath, System.Action<string> callback)
		{
			yield return StartCoroutine(LoadAssetBundleAsync(assetPath));
			
			Resource res = null;
			
			if(m_resources.ContainsKey(assetPath))
			{
				res = m_resources[assetPath];
				res.CheckoutCount++;
			}
			
			if (callback != null)
			{
				callback(assetPath);
			}
		}
		
		
		public IEnumerator CheckOutAssetBundleAsync<T>(string assetPath, System.Action<T> callback, bool forceLoad = false) where T : class
		{
			if(forceLoad)
			{
				yield return StartCoroutine(LoadAssetBundleAsync(assetPath));
			}
			
			Resource res = null;
			
			if(m_resources.ContainsKey(assetPath))
			{
				res = m_resources[assetPath];
				res.CheckoutCount++;
			}
			
			if (callback != null)
			{
				callback((T)res.Res);
			}
		}
		#endregion
	}
}

