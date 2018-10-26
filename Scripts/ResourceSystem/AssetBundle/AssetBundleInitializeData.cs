using System;

namespace TEDCore.AssetBundle
{
    public class AssetBundleInitializeData
    {
        public int MaxDownloadRequest;
        public string DownloadURL;
        public AssetBundleLoadType LoadType;
        public Action<bool> OnManifestLoaded;

        public AssetBundleInitializeData(int maxDownloadRequest,
                                         string downloadURL,
                                         AssetBundleLoadType loadType,
                                         Action<bool> onManifestLoaded)
        {
            MaxDownloadRequest = maxDownloadRequest;
            DownloadURL = downloadURL;
            LoadType = loadType;
            OnManifestLoaded = onManifestLoaded;
        }
    }
}