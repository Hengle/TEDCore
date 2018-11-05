using UnityEditor;

namespace TEDCore.AssetBundle
{
    public class AssetBundleBuildInfo
    {
        public string OutputPath;
        public BuildTarget Target;
        public bool CleanFolders;
        public bool CopyToStreamingAssets;
        public BuildAssetBundleOptions BuildOptions;
        public AssetBundleBuild[] SpecificAssetBundles;
    }
}
