using System;
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using TEDCore;

public class BuildTool : ScriptableObject 
{
    private static string _buildPath = ".";

    [MenuItem("TEDTools/Build/Build Android Develop")]
    private static void BuildAndroidDevelop ()
    {
        string[] scenes = GetBuildScenes();
        string path = GetBuildPathAndroid();

        if(scenes == null || scenes.Length == 0 || path == null)
        {
            return;
        }

        SetupAndroid();

        AssetBundleTool.BuildAssetBundleNormal ();
        AssetDatabase.Refresh();

        DefineTool.SetDefineSymbolDevelop();

        BuildPipeline.BuildPlayer(scenes, path, BuildTarget.Android, BuildOptions.Development | BuildOptions.AllowDebugging);
    }


    [MenuItem("TEDTools/Build/Build Android Release")]
    private static void BuildAndroidRelease ()
    {
        string[] scenes = GetBuildScenes();
        string path = GetBuildPathAndroid();

        if(scenes == null || scenes.Length == 0 || path == null)
        {
            return;
        }

        SetupAndroid();

        AssetBundleTool.BuildAssetBundleNormal ();
        AssetDatabase.Refresh();

        DefineTool.SetDefineSymbolDevelop();

        BuildPipeline.BuildPlayer(scenes, path, BuildTarget.Android, BuildOptions.None);
    }

	
    private static string[] GetBuildScenes()
	{
		List<string> names = new List<string>();
		
		foreach(EditorBuildSettingsScene e in EditorBuildSettings.scenes)
		{
			if(e==null)
				continue;
			
			if(e.enabled)
				names.Add(e.path);
		}
		return names.ToArray();
	}

	
    private static string GetBuildPathAndroid()
	{
        return string.Format(_buildPath + "/{0}_{1}.apk", PlayerSettings.productName, DateTime.Now.ToString("yyyyMMddhhmm"));
	}


    private static void SetupAndroid()
	{
		Dictionary<string, Action<string>> argHandlers = new Dictionary<string, Action<string>>
		{
			{"-keystoreName", delegate(string value)
				{
					PlayerSettings.Android.keystoreName = value;
				}
			},
			{"-keystorePass", delegate(string value)
				{
					PlayerSettings.Android.keystorePass = value;
				}
			},
			{"-keyaliasName", delegate(string value)
				{
					PlayerSettings.Android.keyaliasName = value;
				}
			},
			{"-keyaliasPass", delegate(string value)
				{
					PlayerSettings.Android.keyaliasPass = value;
				}
			},
			{"-dest", delegate(string value)
				{
                    _buildPath = value;
				}
			}
		};

		string[] cmdArgs = Environment.GetCommandLineArgs();
		Action<string> handler;
		for(int i = 0;i < cmdArgs.Length; i++)
		{
			if(argHandlers.ContainsKey(cmdArgs[i]))
			{
				handler = argHandlers[cmdArgs[i]];
				handler(cmdArgs[i + 1]);
			}
		}
	}
}