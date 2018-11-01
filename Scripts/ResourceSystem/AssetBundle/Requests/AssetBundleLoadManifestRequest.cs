using UnityEngine;

namespace TEDCore.AssetBundle
{
    public class AssetBundleLoadManifestRequest : AssetBundleLoadAssetRequestFull<AssetBundleManifest>
    {
        public AssetBundleLoadManifestRequest(string assetBundleName, string assetName)
            : base(assetBundleName, assetName)
        {
            
        }

        public override bool Update ()
        {
            base.Update();

            if (null != m_request && m_request.isDone)
            {
                TEDDebug.LogFormat("[AssetBundleLoadManifestRequest] - Setup AssetBundleManifest successfully at frame {0}", Time.frameCount);
                AssetBundleManager.Instance.SetupManifest(GetAsset());
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}