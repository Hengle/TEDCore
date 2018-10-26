
namespace TEDCore.AssetBundle
{
    public class SingleAssetBundleDownloadInfo
    {
        public float Progress;
        public float AssetSize;

        public SingleAssetBundleDownloadInfo(float progress, float assetSize)
        {
            Progress = progress;
            AssetSize = assetSize;
        }
    }
}
