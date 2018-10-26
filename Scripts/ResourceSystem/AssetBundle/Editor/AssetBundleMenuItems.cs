using UnityEngine;
using UnityEditor;

namespace TEDCore.AssetBundle
{
    public class AssetBundleMenuItems
    {
        private const string ASSETBUNDLE_CLEAR_CACHE_NAME = "TEDCore/AssetBundles/Clear Cache";
        private const string ASSETBUNDLE_EDITOR_LOAD_TYPE_SIMULATE = "TEDCore/AssetBundles/Editor Load Type/Simulate";
        private const string ASSETBUNDLE_EDITOR_LOAD_TYPE_STREAMING = "TEDCore/AssetBundles/Editor Load Type/Streaming";
        private const string ASSETBUNDLE_EDITOR_LOAD_TYPE_NETWORK = "TEDCore/AssetBundles/Editor Load Type/Network";

        [MenuItem(ASSETBUNDLE_CLEAR_CACHE_NAME, priority = 5)]
        private static void ClearCache()
        {
            if (Caching.ClearCache())
            {
                Debug.Log("Cleaned all caches successfully.");
            }
            else
            {
                Debug.LogWarning("Failed to clean caches.");
            }
        }


        [MenuItem(ASSETBUNDLE_EDITOR_LOAD_TYPE_SIMULATE, false)]
        private static void SwitchLoadModeToSimulate()
        {
            AssetBundleDef.SetAssetBundleLoadType(AssetBundleLoadType.Simulate);
        }


        [MenuItem(ASSETBUNDLE_EDITOR_LOAD_TYPE_SIMULATE, true)]
        private static bool SwitchLoadModeToSimulateValidate()
        {
            var loadType = AssetBundleDef.GetAssetBundleLoadType();
            Menu.SetChecked(ASSETBUNDLE_EDITOR_LOAD_TYPE_SIMULATE, loadType == AssetBundleLoadType.Simulate);
            return loadType != AssetBundleLoadType.Simulate;
        }


        [MenuItem(ASSETBUNDLE_EDITOR_LOAD_TYPE_STREAMING, false)]
        private static void SwitchLoadModeToStreaming()
        {
            AssetBundleDef.SetAssetBundleLoadType(AssetBundleLoadType.Streaming);
        }


        [MenuItem(ASSETBUNDLE_EDITOR_LOAD_TYPE_STREAMING, true)]
        private static bool SwitchLoadModeToStreamingValidate()
        {
            var loadType = AssetBundleDef.GetAssetBundleLoadType();
            Menu.SetChecked(ASSETBUNDLE_EDITOR_LOAD_TYPE_STREAMING, loadType == AssetBundleLoadType.Streaming);
            return loadType != AssetBundleLoadType.Streaming;
        }


        [MenuItem(ASSETBUNDLE_EDITOR_LOAD_TYPE_NETWORK, false)]
        private static void SwitchLoadModeToNetwork()
        {
            AssetBundleDef.SetAssetBundleLoadType(AssetBundleLoadType.Network);
        }


        [MenuItem(ASSETBUNDLE_EDITOR_LOAD_TYPE_NETWORK, true)]
        private static bool SwitchLoadModeToNetworkValidate()
        {
            var loadType = AssetBundleDef.GetAssetBundleLoadType();
            Menu.SetChecked(ASSETBUNDLE_EDITOR_LOAD_TYPE_NETWORK, loadType == AssetBundleLoadType.Network);
            return loadType != AssetBundleLoadType.Network;
        }
    }
}
