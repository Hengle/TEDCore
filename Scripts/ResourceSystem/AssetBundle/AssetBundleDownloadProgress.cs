
namespace TEDCore.AssetBundle
{
    public class AssetBundleDownloadProgress
    {
        public float Progress;
        public int DownloadAssetAmount;
        public int TotalAssetAmount;
        public float TotalAssetSize;

        public AssetBundleDownloadProgress(int totalAssetAmount, float totalAssetSize)
        {
            Progress = 0;
            DownloadAssetAmount = 0;
            TotalAssetAmount = totalAssetAmount;
            TotalAssetSize = totalAssetSize;
        }


        public void SetDownloadCount(int downloadCount)
        {
            DownloadAssetAmount = TotalAssetAmount - downloadCount;

            if (TotalAssetAmount == 0)
            {
                Progress = 1;
            }
            else
            {
                Progress = (float)DownloadAssetAmount / TotalAssetAmount;
            }
        }
    }
}
