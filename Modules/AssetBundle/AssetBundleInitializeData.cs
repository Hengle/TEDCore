using System;

namespace TEDCore.AssetBundle
{
    public class AssetBundleInitializeData
    {
        public int MaxDownloadRequest;
        public string DownloadURL;
        public AssetBundleLoadType LoadType;
        public Action<bool> OnInitializeFinish;

        public AssetBundleInitializeData(int maxDownloadRequest,
                                         string downloadURL,
                                         AssetBundleLoadType loadType,
                                         Action<bool> onInitializeFinish)
        {
            MaxDownloadRequest = maxDownloadRequest;
            DownloadURL = downloadURL;
            LoadType = loadType;
            OnInitializeFinish = onInitializeFinish;
        }
    }
}