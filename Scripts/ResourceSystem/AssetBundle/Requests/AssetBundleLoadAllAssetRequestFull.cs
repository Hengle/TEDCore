using UnityEngine;
using System.Linq;

namespace TEDCore.AssetBundle
{
    public class AssetBundleLoadAllAssetRequestFull<T> : AssetBundleLoadAllAssetRequest<T> where T : Object
    {
        private string m_assetBundleName;
        private string m_downloadingError;
        protected AssetBundleRequest m_request = null;

        public AssetBundleLoadAllAssetRequestFull(string assetBundleName)
        {
            m_assetBundleName = assetBundleName;
        }


        public override T[] GetAsset()
        {
            if (null != m_request && m_request.isDone)
            {
                return m_request.allAssets.OfType<T>().ToArray();
            }
            else
            {
                return null;
            }
        }


        public override bool Update ()
        {
            if (null != m_request)
            {
                return false;
            }

            var bundle = AssetBundleSystem.Instance.GetLoadedAssetBundle (m_assetBundleName, out m_downloadingError);
            if (null != bundle)
            {
                m_request = bundle.Bundle.LoadAllAssetsAsync();
                return false;
            }
            else
            {
                return true;
            }
        }


        public override bool IsDone ()
        {
            if (null == m_request && null != m_downloadingError)
            {
                Debug.LogErrorFormat("[AssetBundleLoadAssetRequestFull] - Load AssetBundle '{0}' failed with reason {1}", m_assetBundleName, m_downloadingError);
                return true;
            }

            return null != m_request && m_request.isDone;
        }
    }
}