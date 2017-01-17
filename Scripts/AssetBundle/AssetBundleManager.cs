using UnityEngine;
#if UNITY_EDITOR	
using UnityEditor;
#endif
using System.Collections;
using System.Collections.Generic;

/*
 	In this demo, we demonstrate:
	1.	Automatic asset bundle dependency resolving & loading.
		It shows how to use the manifest assetbundle like how to get the dependencies etc.
	2.	Automatic unloading of asset bundles (When an asset bundle or a dependency thereof is no longer needed, the asset bundle is unloaded)
	3.	Editor simulation. A bool defines if we load asset bundles from the project or are actually using asset bundles(doesn't work with assetbundle variants for now.)
		With this, you can player in editor mode without actually building the assetBundles.
	4.	Optional setup where to download all asset bundles
	5.	Build pipeline build postprocessor, integration so that building a player builds the asset bundles and puts them into the player data (Default implmenetation for loading assetbundles from disk on any platform)
	6.	Use WWW.LoadFromCacheOrDownload and feed 128 bit hash to it when downloading via web
		You can get the hash from the manifest assetbundle.
	7.	AssetBundle variants. A prioritized list of variants that should be used if the asset bundle with that variant exists, first variant in the list is the most preferred etc.
*/

// Loaded assetBundle contains the references count which can be used to unload dependent assetBundles automatically.
public class LoadedAssetBundle
{
	public AssetBundle m_AssetBundle;
	public int m_ReferencedCount;
	
	public LoadedAssetBundle(AssetBundle assetBundle)
	{
		m_AssetBundle = assetBundle;
		m_ReferencedCount = 1;
	}
}

// Class takes care of loading assetBundle and its dependencies automatically, loading variants automatically.
public class AssetBundleManager : MonoBehaviour
{
	const string kAssetBundlesPath = "/AssetBundles/";

	private string m_BaseDownloadingURL = "";
	private string[] m_Variants =  {  };
	private AssetBundleManifest m_AssetBundleManifest = null;

	private Dictionary<string, LoadedAssetBundle> m_LoadedAssetBundles = new Dictionary<string, LoadedAssetBundle> ();
	private Dictionary<string, WWW> m_DownloadingWWWs = new Dictionary<string, WWW> ();
	private Dictionary<string, string> m_DownloadingErrors = new Dictionary<string, string> ();
	private List<AssetBundleLoadOperation> m_InProgressOperations = new List<AssetBundleLoadOperation> ();
	private Dictionary<string, string[]> m_Dependencies = new Dictionary<string, string[]> ();
	private string[] _assetBundleData;
	private bool _haveLoadedAssetBundleData = false;

	// The base downloading url which is used to generate the full downloading url with the assetBundle names.
	public string BaseDownloadingURL
	{
		get { return m_BaseDownloadingURL; }
		set { m_BaseDownloadingURL = value; }
	}

	// Variants which is used to define the active variants.
	public string[] Variants
	{
		get { return m_Variants; }
		set { m_Variants = value; }
	}

	// AssetBundleManifest object which can be used to load the dependecies and check suitable assetBundle variants.
	public AssetBundleManifest AssetBundleManifestObject
	{
		set {m_AssetBundleManifest = value; }
	}


	private void Awake()
	{
		StartCoroutine (PreInitialize());
	}


	private IEnumerator PreInitialize()
	{		
		string platformFolderForAssetBundles = 
			#if UNITY_EDITOR
			GetPlatformFolderForAssetBundles(EditorUserBuildSettings.activeBuildTarget);
		#else
		GetPlatformFolderForAssetBundles(Application.platform);
		#endif
		
		// Set base downloading url.
		string relativePath = GetRelativePath();
		BaseDownloadingURL = relativePath + kAssetBundlesPath + platformFolderForAssetBundles + "/";
		
		// Initialize AssetBundleManifest which loads the AssetBundleManifest object.
		var request = Initialize(platformFolderForAssetBundles);
		if (request != null)
			yield return StartCoroutine(request);
	}


	#if UNITY_EDITOR
	public static string GetPlatformFolderForAssetBundles(BuildTarget target)
	{
		switch(target)
		{
		case BuildTarget.Android:
			return "Android";
		case BuildTarget.iOS:
			return "iOS";
		case BuildTarget.WebPlayer:
			return "WebPlayer";
		case BuildTarget.StandaloneWindows:
		case BuildTarget.StandaloneWindows64:
			return "Windows";
		case BuildTarget.StandaloneOSXIntel:
		case BuildTarget.StandaloneOSXIntel64:
		case BuildTarget.StandaloneOSXUniversal:
			return "OSX";
			// Add more build targets for your own.
			// If you add more targets, don't forget to add the same platforms to GetPlatformFolderForAssetBundles(RuntimePlatform) function.
		default:
			return null;
		}
	}
	#endif


	public string GetRelativePath()
	{
		if (Application.isEditor)
			return "file://" +  Application.streamingAssetsPath;
		//			return "file://" +  System.Environment.CurrentDirectory.Replace("\\", "/"); // Use the build output folder directly.
		else if (Application.isWebPlayer)
			return System.IO.Path.GetDirectoryName(Application.absoluteURL).Replace("\\", "/")+ "/StreamingAssets";
		else if (Application.isMobilePlatform || Application.isConsolePlatform)
			return Application.streamingAssetsPath;
		else // For standalone player.
			return "file://" +  Application.streamingAssetsPath;
	}

	
	static string GetPlatformFolderForAssetBundles(RuntimePlatform platform)
	{
		switch(platform)
		{
		case RuntimePlatform.Android:
			return "Android";
		case RuntimePlatform.IPhonePlayer:
			return "iOS";
		case RuntimePlatform.WindowsWebPlayer:
		case RuntimePlatform.OSXWebPlayer:
			return "WebPlayer";
		case RuntimePlatform.WindowsPlayer:
			return "Windows";
		case RuntimePlatform.OSXPlayer:
			return "OSX";
			// Add more build platform for your own.
			// If you add more platforms, don't forget to add the same targets to GetPlatformFolderForAssetBundles(BuildTarget) function.
		default:
			return null;
		}
	}


	// Get loaded AssetBundle, only return vaild object when all the dependencies are downloaded successfully.
	public LoadedAssetBundle GetLoadedAssetBundle (string assetBundleName, out string error)
	{
		if (m_DownloadingErrors.TryGetValue(assetBundleName, out error))
			return null;
	
		LoadedAssetBundle bundle = null;
		m_LoadedAssetBundles.TryGetValue(assetBundleName, out bundle);
		if (bundle == null)
			return null;
		
		// No dependencies are recorded, only the bundle itself is required.
		string[] dependencies = null;
		if (!m_Dependencies.TryGetValue(assetBundleName, out dependencies) )
			return bundle;
		
		// Make sure all dependencies are loaded
		foreach(var dependency in dependencies)
		{
			if (m_DownloadingErrors.TryGetValue(assetBundleName, out error) )
				return bundle;

			// Wait all the dependent assetBundles being loaded.
			LoadedAssetBundle dependentBundle;
			m_LoadedAssetBundles.TryGetValue(dependency, out dependentBundle);
			if (dependentBundle == null)
				return null;
		}

		return bundle;
	}

	// Load AssetBundleManifest.
	public AssetBundleLoadManifestOperation Initialize (string manifestAssetBundleName)
	{
		LoadAssetBundle(manifestAssetBundleName, true);
		var operation = new AssetBundleLoadManifestOperation (manifestAssetBundleName, "AssetBundleManifest", typeof(AssetBundleManifest));
		m_InProgressOperations.Add (operation);
		return operation;
	}
	
	// Load AssetBundle and its dependencies.
	public void LoadAssetBundle(string assetBundleName, bool isLoadingAssetBundleManifest = false)
	{
		if (!isLoadingAssetBundleManifest)
			assetBundleName = RemapVariantName (assetBundleName);

		// Check if the assetBundle has already been processed.
		bool isAlreadyProcessed = LoadAssetBundleInternal(assetBundleName, isLoadingAssetBundleManifest);

		// Load dependencies.
		if (!isAlreadyProcessed && !isLoadingAssetBundleManifest)
			LoadDependencies(assetBundleName);
	}
	
	// Remaps the asset bundle name to the best fitting asset bundle variant.
	public string RemapVariantName(string assetBundleName)
	{
		string[] bundlesWithVariant = m_AssetBundleManifest.GetAllAssetBundlesWithVariant();

		// If the asset bundle doesn't have variant, simply return.
		if (System.Array.IndexOf(bundlesWithVariant, assetBundleName) < 0 )
			return assetBundleName;

		string[] split = assetBundleName.Split('.');

		int bestFit = int.MaxValue;
		int bestFitIndex = -1;
		// Loop all the assetBundles with variant to find the best fit variant assetBundle.
		for (int i = 0; i < bundlesWithVariant.Length; i++)
		{
			string[] curSplit = bundlesWithVariant[i].Split('.');
			if (curSplit[0] != split[0])
				continue;
			
			int found = System.Array.IndexOf(m_Variants, curSplit[1]);
			if (found != -1 && found < bestFit)
			{
				bestFit = found;
				bestFitIndex = i;
			}
		}

		if (bestFitIndex != -1)
			return bundlesWithVariant[bestFitIndex];
		else
			return assetBundleName;
	}

	// Where we actuall call WWW to download the assetBundle.
	public bool LoadAssetBundleInternal (string assetBundleName, bool isLoadingAssetBundleManifest)
	{
		// Already loaded.
		LoadedAssetBundle bundle = null;
		m_LoadedAssetBundles.TryGetValue(assetBundleName, out bundle);
		if (bundle != null)
		{
			bundle.m_ReferencedCount++;
			return true;
		}

		// @TODO: Do we need to consider the referenced count of WWWs?
		// In the demo, we never have duplicate WWWs as we wait LoadAssetAsync()/LoadLevelAsync() to be finished before calling another LoadAssetAsync()/LoadLevelAsync().
		// But in the real case, users can call LoadAssetAsync()/LoadLevelAsync() several times then wait them to be finished which might have duplicate WWWs.
		if (m_DownloadingWWWs.ContainsKey(assetBundleName) )
			return true;

		WWW download = null;
		string url = m_BaseDownloadingURL + assetBundleName;

		// For manifest assetbundle, always download it as we don't have hash for it.
		if (isLoadingAssetBundleManifest)
			download = new WWW(url);
		else
			download = WWW.LoadFromCacheOrDownload(url, m_AssetBundleManifest.GetAssetBundleHash(assetBundleName), 0); 

		m_DownloadingWWWs.Add(assetBundleName, download);

		return false;
	}

	// Where we get all the dependencies and load them all.
	public void LoadDependencies(string assetBundleName)
	{
		if (m_AssetBundleManifest == null)
		{
			Debug.LogError("Please initialize AssetBundleManifest by calling AssetBundleManager.Initialize()");
			return;
		}

		// Get dependecies from the AssetBundleManifest object..
		string[] dependencies = m_AssetBundleManifest.GetAllDependencies(assetBundleName);
		if (dependencies.Length == 0)
			return;
			
		for (int i=0;i<dependencies.Length;i++)
			dependencies[i] = RemapVariantName (dependencies[i]);
			
		// Record and load all dependencies.
		m_Dependencies.Add(assetBundleName, dependencies);
		for (int i=0;i<dependencies.Length;i++)
			LoadAssetBundleInternal(dependencies[i], false);
	}

	// Unload assetbundle and its dependencies.
	public void UnloadAssetBundle(string assetBundleName)
	{
//		Debug.Log(m_LoadedAssetBundles.Count + " assetbundle(s) in memory before unloading " + assetBundleName);

		UnloadAssetBundleInternal(assetBundleName);
		UnloadDependencies(assetBundleName);

//		Debug.Log(m_LoadedAssetBundles.Count + " assetbundle(s) in memory after unloading " + assetBundleName);
	}

	public void UnloadDependencies(string assetBundleName)
	{
		string[] dependencies = null;
		if (!m_Dependencies.TryGetValue(assetBundleName, out dependencies) )
			return;

		// Loop dependencies.
		foreach(var dependency in dependencies)
		{
			UnloadAssetBundleInternal(dependency);
		}

		m_Dependencies.Remove(assetBundleName);
	}

	public void UnloadAssetBundleInternal(string assetBundleName)
	{
		string error;
		LoadedAssetBundle bundle = GetLoadedAssetBundle(assetBundleName, out error);
		if (bundle == null)
			return;

//		Debug.Log (assetBundleName + " referenced count = " + bundle.m_ReferencedCount);
		if (--bundle.m_ReferencedCount == 0)
		{
			bundle.m_AssetBundle.Unload(false);
			m_LoadedAssetBundles.Remove(assetBundleName);
			//Debug.Log("AssetBundle " + assetBundleName + " has been unloaded successfully");
		}

//		Debug.Log (assetBundleName + " referenced count = " + bundle.m_ReferencedCount);
	}

	private void Update()
	{
		// Collect all the finished WWWs.
		var keysToRemove = new List<string>();
		foreach (var keyValue in m_DownloadingWWWs)
		{
			WWW download = keyValue.Value;

			// If downloading fails.
			if (download.error != null)
			{
				m_DownloadingErrors.Add(keyValue.Key, download.error);
				keysToRemove.Add(keyValue.Key);
				continue;
			}

			// If downloading succeeds.
			if(download.isDone)
			{
				Debug.Log("Downloading " + keyValue.Key + " is done at frame " + Time.frameCount);
				m_LoadedAssetBundles.Add(keyValue.Key, new LoadedAssetBundle(download.assetBundle) );
				keysToRemove.Add(keyValue.Key);
			}
		}

		// Remove the finished WWWs.
		foreach( var key in keysToRemove)
		{
			WWW download = m_DownloadingWWWs[key];
			m_DownloadingWWWs.Remove(key);
			download.Dispose();
		}

		// Update all in progress operations
		for (int i=0;i<m_InProgressOperations.Count;)
		{
			if (!m_InProgressOperations[i].Update())
			{
				m_InProgressOperations.RemoveAt(i);
			}
			else
				i++;
		}
	}

	// Load asset from the given assetBundle.
	public AssetBundleLoadAssetOperation LoadAssetAsync (string assetBundleName, string assetName, System.Type type)
	{
		AssetBundleLoadAssetOperation operation = null;

		LoadAssetBundle (assetBundleName);
		operation = new AssetBundleLoadAssetOperationFull (assetBundleName, assetName, type);

		m_InProgressOperations.Add (operation);

		return operation;
	}

	// Load level from the given assetBundle.
	public AssetBundleLoadOperation LoadLevelAsync (string assetBundleName, string levelName, bool isAdditive)
	{
		AssetBundleLoadOperation operation = null;

		LoadAssetBundle (assetBundleName);
		operation = new AssetBundleLoadLevelOperation (assetBundleName, levelName, isAdditive);

		m_InProgressOperations.Add (operation);

		return operation;
	}

	public bool IsAssetBundle(string path)
	{
		#if ASSET_BUNDLE
		if (null == _assetBundleData)
		{
			if(_haveLoadedAssetBundleData)
			{
				return false;
			}

			_haveLoadedAssetBundleData = true;

			TextAsset textAsset = Resources.Load<TextAsset> ("AssetBundleData");

			if (null == textAsset)
			{
				return false;
			}

			_assetBundleData = textAsset.text.Split (new char[]{ '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
			Resources.UnloadAsset (textAsset);
		}

		for (int cnt = 0; cnt < _assetBundleData.Length; cnt++)
		{
			if (path == _assetBundleData [cnt])
			{
				return true;
			}
		}
		#endif

		return false;
	}
} // End of AssetBundleManager.