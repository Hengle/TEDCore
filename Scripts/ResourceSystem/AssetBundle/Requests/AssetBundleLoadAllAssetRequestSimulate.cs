#if UNITY_EDITOR
using UnityEngine;
using System.Linq;

namespace TEDCore.AssetBundle
{
    public class AssetBundleLoadAllAssetRequestSimulate<T> : AssetBundleLoadAllAssetRequest<T> where T : Object
    {
        private string m_assetBundleName;
        private string m_assetName;

        public AssetBundleLoadAllAssetRequestSimulate(string assetBundleName, string assetName)
        {
            m_assetBundleName = assetBundleName;
            m_assetName = assetName;
        }


        public override T[] GetAsset()
        {
            var assetPaths = UnityEditor.AssetDatabase.GetAssetPathsFromAssetBundleAndAssetName(m_assetBundleName, m_assetName);

            if (assetPaths == null || assetPaths.Length == 0)
            {
                return null;
            }

            Object[] assets = null;
            foreach (var path in assetPaths)
            {
                assets = UnityEditor.AssetDatabase.LoadAllAssetsAtPath(path);
                if (assets != null && assets.OfType<T>().Any())
                {
                    break;
                }
            }

            if (assets == null || assets.Length == 0)
            {
                return null;
            }

            return assets.OfType<T>().ToArray();
        }


        public override bool Update()
        {
            return false;
        }


        public override bool IsDone()
        {
            return true;
        }
    }
}
#endif