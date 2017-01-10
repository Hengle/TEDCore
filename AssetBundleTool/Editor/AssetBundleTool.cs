using UnityEngine;
using UnityEditor;
using System.Collections;
using TEDCore;
using System.IO;
using TEDCore.Resource;
using System.Collections.Generic;

public class AssetBundleTool
{
    private static string ASSET_BUNDLE_RESOURCE_FOLDER = "AssetBundleResources/";
    private const string ASSET_BUNDLE_OUTPUT_FOLDER = "AssetBundles";

    [MenuItem("TEDTools/AssetBundles/Clear Cache")]
    private static void ClearCache()
    {
        if(Caching.CleanCache())
        {
            Debug.Log("Successfully cleaned all caches.");
        }
        else
        {
            Debug.Log("Cache was in use.");
        }
    }


    [MenuItem("TEDTools/AssetBundles/Build for PC")]
    private static void OnAssetBundleBuildForPC ()
    {
        EditorPrefs.SetBool("AssetBundleBuildForPC", !EditorPrefs.GetBool("AssetBundleBuildForPC", false));
    }
    [MenuItem("TEDTools/AssetBundles/Build for PC", true)]
    private static bool OnAssetBundleBuildForPCValidate ()
    {
        Menu.SetChecked("TEDTools/AssetBundles/Build for PC", EditorPrefs.GetBool("AssetBundleBuildForPC", false));
        return true;
    }


    [MenuItem("TEDTools/AssetBundles/Build for OSX")]
    private static void OnAssetBundleBuildForOSX ()
    {
        EditorPrefs.SetBool("AssetBundleBuildForOSX", !EditorPrefs.GetBool("AssetBundleBuildForOSX", false));
    }
    [MenuItem("TEDTools/AssetBundles/Build for OSX", true)]
    private static bool OnAssetBundleBuildForOSXValidate ()
    {
        Menu.SetChecked("TEDTools/AssetBundles/Build for OSX", EditorPrefs.GetBool("AssetBundleBuildForOSX", false));
        return true;
    }


    [MenuItem("TEDTools/AssetBundles/Build for Linux")]
    private static void OnAssetBundleBuildForLinux ()
    {
        EditorPrefs.SetBool("AssetBundleBuildForLinux", !EditorPrefs.GetBool("AssetBundleBuildForLinux", false));
    }
    [MenuItem("TEDTools/AssetBundles/Build for Linux", true)]
    private static bool OnAssetBundleBuildForLinuxValidate ()
    {
        Menu.SetChecked("TEDTools/AssetBundles/Build for Linux", EditorPrefs.GetBool("AssetBundleBuildForLinux", false));
        return true;
    }


    [MenuItem("TEDTools/AssetBundles/Build for iOS")]
    private static void OnAssetBundleBuildForIOS ()
    {
        EditorPrefs.SetBool("AssetBundleBuildForIOS", !EditorPrefs.GetBool("AssetBundleBuildForIOS", false));
    }
    [MenuItem("TEDTools/AssetBundles/Build for iOS", true)]
    private static bool OnAssetBundleBuildForIOSValidate ()
    {
        Menu.SetChecked("TEDTools/AssetBundles/Build for iOS", EditorPrefs.GetBool("AssetBundleBuildForIOS", false));
        return true;
    }


    [MenuItem("TEDTools/AssetBundles/Build for Android")]
    private static void OnAssetBundleBuildForAndroid ()
    {
        EditorPrefs.SetBool("AssetBundleBuildForAndroid", !EditorPrefs.GetBool("AssetBundleBuildForAndroid", false));
    }
    [MenuItem("TEDTools/AssetBundles/Build for Android", true)]
    private static bool OnAssetBundleBuildForAndroidValidate ()
    {
        Menu.SetChecked("TEDTools/AssetBundles/Build for Android", EditorPrefs.GetBool("AssetBundleBuildForAndroid", false));
        return true;
    }


    [MenuItem ("TEDTools/AssetBundles/Build AssetBundles")]
    public static void BuildAssetBundleNormal()
    {
        BuildAssetBundles ();
    }


    [MenuItem ("TEDTools/AssetBundles/Build AssetBundles (Force Rebuild)")]
    private static void BuildAssetBundlesForceRebuild()
    {
        BuildAssetBundles (true);
    }


    private static void BuildAssetBundles (bool forceRebuild = false)
    {
        if (!SetupAssetBundleNames())
        {
            Debug.Log("Don't have AssetBundleResources folder.");
            return;
        }

        // Choose the output path according to the build target.
        string outputPath = GetOutputPath ();
        if (!Directory.Exists(outputPath))
        {
            Directory.CreateDirectory(outputPath);
        }

        BuildTarget currentTarget = EditorUserBuildSettings.activeBuildTarget;

        Dictionary<BuildTarget, string> targetName = new Dictionary<BuildTarget,string>();
        targetName.Add(BuildTarget.iOS, "AssetBundleBuildForIOS");
        targetName.Add(BuildTarget.Android, "AssetBundleBuildForAndroid");
        targetName.Add(BuildTarget.StandaloneLinux64, "AssetBundleBuildForLinux");
        targetName.Add(BuildTarget.StandaloneWindows, "AssetBundleBuildForPC");
        targetName.Add(BuildTarget.StandaloneOSXUniversal, "AssetBundleBuildForOSX");

        BuildAssetBundleOptions options = BuildAssetBundleOptions.None;
        if (forceRebuild)
        {
            options |= BuildAssetBundleOptions.ForceRebuildAssetBundle;
        }

        foreach (KeyValuePair<BuildTarget, string> kvp in targetName)
        {
            if (!EditorPrefs.GetBool (kvp.Value, false))
            {
                continue;
            }

            BuildPipeline.BuildAssetBundles (outputPath, options, kvp.Key);
        }

        AssetDatabase.Refresh ();

        if (EditorUserBuildSettings.activeBuildTarget != currentTarget)
        {
            EditorUserBuildSettings.SwitchActiveBuildTarget (currentTarget);
        }

        Debug.Log("Build Asset Bundle Sucess.");
    }


    private static bool SetupAssetBundleNames()
    {
        ClearAssetBundleNames ();

        string assetPath = Path.Combine(Application.dataPath, ASSET_BUNDLE_RESOURCE_FOLDER);
        DirectoryInfo directoryInfo = new DirectoryInfo (assetPath);

        if (!directoryInfo.Exists)
        {
            return false;
        }

        SetupAssetBundleNames(directoryInfo);

        AssetDatabase.Refresh();

        return true;
    }


    private static void ClearAssetBundleNames()
    {
        string assetPath = Application.dataPath;
        DirectoryInfo directoryInfo = new DirectoryInfo (assetPath);

        SetupAssetBundleNames(directoryInfo, true);

        AssetDatabase.Refresh();
    }


    private static void SetupAssetBundleNames(DirectoryInfo directoryInfo, bool clear = false)
    {
        FileInfo[] fileInfos = directoryInfo.GetFiles();
        for(int cnt = 0; cnt < fileInfos.Length; cnt++)
        {
            if(AssetBundleData.IsLegalExtension(fileInfos[cnt].Extension))
            {
                string assetbundleName = directoryInfo.FullName;

                if (clear)
                {
                    assetbundleName = "";
                }
                else
                {
                    assetbundleName = assetbundleName.Replace(Path.Combine(Application.dataPath, ASSET_BUNDLE_RESOURCE_FOLDER), "") + "/" + fileInfos[cnt].Name.Substring(0, fileInfos[cnt].Name.Length - 4) + ".assetbundle";
                    assetbundleName = assetbundleName.ToLower();
                }

                string filePath = fileInfos[cnt].FullName;

                if (clear)
                {
                    filePath = filePath.Replace(Application.dataPath, "");
                    filePath = "Assets" + filePath;
                }
                else
                {
                    filePath = filePath.Replace(Path.Combine(Application.dataPath, ASSET_BUNDLE_RESOURCE_FOLDER), "");
                    filePath = Path.Combine (ASSET_BUNDLE_RESOURCE_FOLDER, filePath);
                    filePath = Path.Combine ("Assets", filePath);
                }

                AssetImporter.GetAtPath(filePath).assetBundleName = assetbundleName;

                //              if (clear)
                //              {
                //                  Debug.Log (string.Format ("Clear the asset \"{0}\" assetBundleName", filePath));
                //              }
                //              else
                //              {
                //                  Debug.Log (string.Format("Set the asset \"{0}\" assetBundleName to {1}", fileInfos[cnt].Name, assetbundleName));
                //              }
            }
        }

        DirectoryInfo[] directoryInfos = directoryInfo.GetDirectories();
        if(directoryInfos.Length != 0)
        {
            for(int cnt = 0; cnt < directoryInfos.Length; cnt++)
            {
                SetupAssetBundleNames(directoryInfos[cnt], clear);
            }
        }
    }


    private static string GetOutputPath()
    {
        string outputPath = Path.Combine (Application.streamingAssetsPath, ASSET_BUNDLE_OUTPUT_FOLDER);
        outputPath = Path.Combine (outputPath, AssetBundleManager.GetPlatformFolderForAssetBundles(EditorUserBuildSettings.activeBuildTarget));

        return outputPath;
    }
}
