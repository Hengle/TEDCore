using System;
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace TEDCore.Build
{
    public class BuildTool
    {
        [MenuItem("TEDCore/Build/Build StandaloneOSX Develop")]
        private static void BuildStandaloneDevelop()
        {
            string[] scenes = GetSceneNames();
            string buildPath = GetStandaloneOSXPath();

            if (null == scenes || scenes.Length == 0 || null == buildPath)
            {
                if(null == scenes || scenes.Length == 0)
                {
                    TEDDebug.LogError("The scenes are null or empty.");
                }
                else
                {
                    TEDDebug.LogError("The build path is null.");
                }

                return;
            }

            AssetDatabase.Refresh();
            BuildPipeline.BuildPlayer(scenes, buildPath, BuildTarget.StandaloneOSX, BuildOptions.Development | BuildOptions.AllowDebugging);
        }

        [MenuItem("TEDCore/Build/Build StandaloneOSX Release")]
        private static void BuildStandaloneRelease()
        {
            string[] scenes = GetSceneNames();
            string buildPath = GetStandaloneOSXPath();

            if (null == scenes || scenes.Length == 0 || null == buildPath)
            {
                if (null == scenes || scenes.Length == 0)
                {
                    TEDDebug.LogError("The scenes are null or empty.");
                }
                else
                {
                    TEDDebug.LogError("The build path is null.");
                }

                return;
            }

            AssetDatabase.Refresh();
            BuildPipeline.BuildPlayer(scenes, buildPath, BuildTarget.StandaloneOSX, BuildOptions.Development | BuildOptions.AllowDebugging);
        }

        [MenuItem("TEDCore/Build/Build Android Develop")]
        private static void BuildAndroidDevelop()
        {
            string[] scenes = GetSceneNames();
            string buildPath = GetAndroidBuildPath();

            if (null == scenes || scenes.Length == 0 || null == buildPath)
            {
                if (null == scenes || scenes.Length == 0)
                {
                    TEDDebug.LogError("The scenes are null or empty.");
                }
                else
                {
                    TEDDebug.LogError("The build path is null.");
                }

                return;
            }

            AssetDatabase.Refresh();
            BuildPipeline.BuildPlayer(scenes, buildPath, BuildTarget.Android, BuildOptions.None);
        }

        [MenuItem("TEDCore/Build/Build Android Release")]
        private static void BuildAndroidRelease()
        {
            string[] scenes = GetSceneNames();
            string buildPath = GetAndroidBuildPath();

            if (scenes == null || scenes.Length == 0 || buildPath == null)
            {
                if (null == scenes || scenes.Length == 0)
                {
                    TEDDebug.LogError("The scenes are null or empty.");
                }
                else
                {
                    TEDDebug.LogError("The build path is null.");
                }

                return;
            }

            AssetDatabase.Refresh();
            BuildPipeline.BuildPlayer(scenes, buildPath, BuildTarget.Android, BuildOptions.None);
        }

        private static string[] GetSceneNames()
        {
            List<string> sceneNames = new List<string>();
            EditorBuildSettingsScene[] scenes = EditorBuildSettings.scenes;
            for (int i = 0; i < scenes.Length; i++)
            {
                if (null == scenes[i])
                {
                    continue;
                }

                if (scenes[i].enabled)
                {
                    sceneNames.Add(scenes[i].path);
                }
            }

            return sceneNames.ToArray();
        }

        private static string GetStandaloneOSXPath()
        {
            string version = PlayerSettings.bundleVersion + "." + PlayerSettings.macOS.buildNumber;
            return GetLocationPathName(version);
        }

        private static string GetAndroidBuildPath()
        {
            string version = PlayerSettings.bundleVersion + "." + PlayerSettings.Android.bundleVersionCode;
            return GetLocationPathName(version);
        }

        private static string GetLocationPathName(string version)
        {
            return Application.dataPath.Replace("Assets", "") + string.Format("{0}_v{1}_{2}.apk", PlayerSettings.productName, version, DateTime.Now.ToString("yyyyMMddHHmm"));
        }
    }
}