using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

public class AssetBundleDataTool
{
	private static string GENERATE_SCRIPT_PATH = Application.dataPath + "/AssetBundleTool/GenerateScripts/";
	private static string ASSET_BUNDLE_RESOURCE_FOLDER = Application.dataPath + "/AssetBundleResources/";
	private static string TEMPLATE_PATH = "Assets/AssetBundleTool/Editor/Template_AssetBundleData.txt";

	public static void GenerateAssetBundleData()
	{
		DirectoryInfo directoryInfo = new DirectoryInfo (ASSET_BUNDLE_RESOURCE_FOLDER);
		if (!directoryInfo.Exists)
		{
			return;
		}

		List<string> folderList = new List<string> ();
		GetAllFolders (folderList, directoryInfo);

		string assetBundleFolders = "";

		if (folderList.Count > 0)
		{
			assetBundleFolders = string.Format ("\"{0}/\"", folderList [0]);

			for (int cnt = 1; cnt < folderList.Count; cnt++)
			{
				assetBundleFolders += string.Format (",\n\t\t\"{0}/\"", folderList [cnt]);
			}
		}
		else
		{
			assetBundleFolders = "\"\"";
		}

		string template = GetTemplate (TEMPLATE_PATH);
		template = template.Replace ("$AssetBundleFolders", assetBundleFolders);
		GenerateScript ("AssetBundleData", template);

		AssetDatabase.Refresh ();
	}


	private static void GetAllFolders(List<string> allFolders, DirectoryInfo directoryInfo)
	{
		DirectoryInfo[] cacheFolders = directoryInfo.GetDirectories();
		for (int cnt = 0; cnt < cacheFolders.Length; cnt++)
		{
			allFolders.Add (cacheFolders[cnt].FullName.Replace(ASSET_BUNDLE_RESOURCE_FOLDER, ""));
			GetAllFolders (allFolders, cacheFolders[cnt]);
		}
	}


	private static string GetTemplate(string path)
	{
		TextAsset txt = (TextAsset)AssetDatabase.LoadAssetAtPath(path, typeof(TextAsset));

		return txt.text;
	}


	private static void GenerateScript(string scriptName, string content)
	{
		scriptName = GENERATE_SCRIPT_PATH + scriptName + ".cs";

		if (!Directory.Exists (GENERATE_SCRIPT_PATH))
		{
			Directory.CreateDirectory (GENERATE_SCRIPT_PATH);
		}

		if (File.Exists(scriptName))
		{
			File.Delete(scriptName);
		}

		StreamWriter sr = File.CreateText(scriptName);
		sr.WriteLine (content);
		sr.Close();
	}
}