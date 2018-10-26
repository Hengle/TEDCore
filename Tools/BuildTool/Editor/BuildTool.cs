using System;
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace TEDCore.Build
{
    public class BuildTool
    {
        [MenuItem("TEDCore/Build/Build Android Develop")]
        private static void BuildAndroidDevelop()
        {
            string[] scenes = GetSceneNames();
            string buildPath = GetAndroidBuildPath();

            if (null == scenes || scenes.Length == 0 || null == buildPath)
            {
                return;
            }

            AssetDatabase.Refresh();
            BuildPipeline.BuildPlayer(scenes, buildPath, BuildTarget.Android, BuildOptions.Development | BuildOptions.AllowDebugging);
        }


        [MenuItem("TEDCore/Build/Build Android Release")]
        private static void BuildAndroidRelease()
        {
            string[] scenes = GetSceneNames();
            string buildPath = GetAndroidBuildPath();

            if (scenes == null || scenes.Length == 0 || buildPath == null)
            {
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


        private static string GetAndroidBuildPath()
        {
            string version = "";

            string[] split = new string[] { "." };
            string[] bundleIdentifier = PlayerSettings.bundleVersion.Split(split, StringSplitOptions.RemoveEmptyEntries);

            for (int cnt = 0; cnt < bundleIdentifier.Length; cnt++)
            {
                version += bundleIdentifier[cnt] + ".";
            }

            version += PlayerSettings.Android.bundleVersionCode;

            return string.Format(Application.dataPath.Replace("Assets", "") + "{0}_v{1}_{2}.apk", PlayerSettings.productName, version, DateTime.Now.ToString("yyyyMMddHHmm"));
        }
    }
}