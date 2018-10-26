using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

namespace TEDCore.AssetBundle
{
    public class AssetBundleBuildTool
    {
        public const string ASSET_BUNDLE_OUTPUT_FOLDER = "AssetBundles";
        private const string STREAMING_ASSETS_FOLDER_PATH = "Assets/StreamingAssets/";

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


        public static void BuildAssetBundles(AssetBundleBuildInfo buildInfo)
        {
            AssetBundleNameBuilder.Build();

            AssetDatabase.Refresh();
            AssetDatabase.RemoveUnusedAssetBundleNames();

            if (buildInfo.ForceRebuild)
            {
                if (Directory.Exists(buildInfo.OutputPath))
                {
                    Directory.Delete(buildInfo.OutputPath, true);
                }
            }

            if (!Directory.Exists(buildInfo.OutputPath))
            {
                Directory.CreateDirectory(buildInfo.OutputPath);
            }

            if (buildInfo.SpecificAssetBundles == null || buildInfo.SpecificAssetBundles.Length == 0)
            {
                BuildPipeline.BuildAssetBundles(buildInfo.OutputPath, buildInfo.BuildOptions, buildInfo.Target);
            }
            else
            {
                BuildPipeline.BuildAssetBundles(buildInfo.OutputPath, buildInfo.SpecificAssetBundles, buildInfo.BuildOptions, buildInfo.Target);
            }

            AssetBundleCatalogBuilder.Build(buildInfo.OutputPath);

            if (buildInfo.CopyToStreamingAssets)
            {
                var streamingAssetsPath = Path.Combine(STREAMING_ASSETS_FOLDER_PATH, ASSET_BUNDLE_OUTPUT_FOLDER);
                streamingAssetsPath = Path.Combine(streamingAssetsPath, AssetBundleDef.GetPlatformName());

                if (Directory.Exists(streamingAssetsPath))
                {
                    Directory.Delete(streamingAssetsPath, true);
                }

                DirectoryCopy(buildInfo.OutputPath, streamingAssetsPath);

                if (buildInfo.OptimizedInitialPackageSize)
                {
                    OptimizedInitialPackage(buildInfo.InitialAssetBundlePackages);
                }
            }

            AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);

            Debug.Log("Build AssetBundles successfully.");
        }


        public static string GetDefaultOutputPath()
        {
            return GetOutputPath(AssetBundleDef.GetPlatformName());
        }


        public static string GetOutputPath(string buildTarget)
        {
            var outputPath = Path.Combine(Application.dataPath.Replace("Assets", ""), ASSET_BUNDLE_OUTPUT_FOLDER);
            outputPath = Path.Combine(outputPath, buildTarget);

            return outputPath;
        }


        private static void DirectoryCopy(string sourceDirName, string destDirName)
        {
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }

            foreach (var folderPath in Directory.GetDirectories(sourceDirName, "*", SearchOption.AllDirectories))
            {
                if (!Directory.Exists(folderPath.Replace(sourceDirName, destDirName)))
                {
                    Directory.CreateDirectory(folderPath.Replace(sourceDirName, destDirName));
                }
            }

            foreach (var filePath in Directory.GetFiles(sourceDirName, "*.*", SearchOption.AllDirectories))
            {
                var newFilePath = Path.Combine(Path.GetDirectoryName(filePath).Replace(sourceDirName, destDirName), Path.GetFileName(filePath));
                File.Copy(filePath, newFilePath, true);
            }
        }

        private static void OptimizedInitialPackage(List<string> recordedInitialPackages)
        {
            var streamingAssetsPath = Path.Combine(STREAMING_ASSETS_FOLDER_PATH, ASSET_BUNDLE_OUTPUT_FOLDER);
            streamingAssetsPath = Path.Combine(streamingAssetsPath, AssetBundleDef.GetPlatformName());
            streamingAssetsPath += "/";

            var initialPackages = new List<string>();
            initialPackages.Add(AssetBundleDef.GetPlatformName());
            initialPackages.Add(AssetBundleDef.CATALOG_FILE_NAME);
            initialPackages.AddRange(recordedInitialPackages);

            var remainingFileNames = new List<string>();
            foreach (var name in initialPackages)
            {
                remainingFileNames.Add(streamingAssetsPath + name);
                remainingFileNames.Add(streamingAssetsPath + name + ".manifest");
                remainingFileNames.Add(streamingAssetsPath + name + ".meta");
                remainingFileNames.Add(streamingAssetsPath + name + ".manifest.meta");
            }

            foreach (var filePath in Directory.GetFiles(streamingAssetsPath, "*.*", SearchOption.AllDirectories))
            {
                if (remainingFileNames.Contains(filePath))
                {
                    continue;
                }

                File.Delete(filePath);
            }

            foreach (var directoryPath in Directory.GetDirectories(streamingAssetsPath, "*.*", SearchOption.AllDirectories))
            {
                if (!Directory.Exists(directoryPath))
                {
                    continue;
                }

                var files = Directory.GetFiles(directoryPath);
                if (files.Length == 0)
                {
                    Directory.Delete(directoryPath, true);
                }
            }

            AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
        }
    }

    public class AssetBundleBuildInfo
    {
        public string OutputPath;
        public BuildTarget Target;
        public bool ForceRebuild;
        public bool CopyToStreamingAssets;
        public BuildAssetBundleOptions BuildOptions;
        public AssetBundleBuild[] SpecificAssetBundles;
        public bool OptimizedInitialPackageSize;
        public List<string> InitialAssetBundlePackages;
    }
}
