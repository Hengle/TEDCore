using UnityEngine;
using UnityEditor;
using System.IO;

namespace TEDCore.AssetBundle
{
    public class AssetBundleBuilder
    {
        private const string STREAMING_ASSETS_FOLDER_PATH = "Assets/StreamingAssets/";

        public static void Build(AssetBundleBuildInfo buildInfo)
        {
            AssetBundleNameBuilder.Build();

            AssetDatabase.Refresh();
            AssetDatabase.RemoveUnusedAssetBundleNames();

            if (buildInfo.CleanFolders)
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
                var streamingAssetsPath = Path.Combine(STREAMING_ASSETS_FOLDER_PATH, AssetBundleDef.ASSET_BUNDLE_OUTPUT_FOLDER);
                streamingAssetsPath = Path.Combine(streamingAssetsPath, AssetBundleDef.GetPlatformName());

                if (Directory.Exists(streamingAssetsPath))
                {
                    Directory.Delete(streamingAssetsPath, true);
                }

                DirectoryCopy(buildInfo.OutputPath, streamingAssetsPath);
            }

            AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);

            TEDDebug.Log("Build AssetBundles successfully.");
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
    }
}