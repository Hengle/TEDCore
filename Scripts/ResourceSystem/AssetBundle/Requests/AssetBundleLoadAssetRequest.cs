using UnityEngine;

namespace TEDCore.AssetBundle
{
    public abstract class AssetBundleLoadAssetRequest<T> : AssetBundleLoadRequest where T : Object
    {
        public abstract T GetAsset();
    }
}