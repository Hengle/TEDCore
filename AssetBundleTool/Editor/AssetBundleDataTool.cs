using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

public class AssetBundleDataTool
{
	private static string GENERATE_SCRIPT_PATH = Application.dataPath + "/AssetBundleTool/Resources/";
	private static string ASSET_BUNDLE_RESOURCE_FOLDER = Application.dataPath + "/AssetBundleResources/";
	private static string ASSET_BUNDLE_DATA = "AssetBundleData";

	public static void GenerateAssetBundleData()
	{
		DirectoryInfo directoryInfo = new DirectoryInfo (ASSET_BUNDLE_RESOURCE_FOLDER);
		string assetBundleFolders = "";

		if (directoryInfo.Exists)
		{
			List<string> fileList = new List<string> ();
			GetAllAssetBundlePath (fileList, directoryInfo);

			if (fileList.Count > 0)
			{
				assetBundleFolders = fileList [0];

				for (int cnt = 1; cnt < fileList.Count; cnt++)
				{
					assetBundleFolders += string.Format ("\n{0}", fileList [cnt]);
				}
			}
		}

		GenerateResources (ASSET_BUNDLE_DATA, assetBundleFolders);

		AssetDatabase.Refresh ();
	}


	private static void GetAllAssetBundlePath(List<string> allFiles, DirectoryInfo directoryInfo)
	{
		FileInfo[] fileInfos = directoryInfo.GetFiles();
		for(int cnt = 0; cnt < fileInfos.Length; cnt++)
		{
			if(AssetBundleExtension.IsLegal(fileInfos[cnt].Extension))
			{
				allFiles.Add (fileInfos[cnt].FullName.Replace(ASSET_BUNDLE_RESOURCE_FOLDER, "").Replace(fileInfos[cnt].Extension, ""));
			}
		}

		DirectoryInfo[] directoryInfos = directoryInfo.GetDirectories();
		if(directoryInfos.Length != 0)
		{
			for(int cnt = 0; cnt < directoryInfos.Length; cnt++)
			{
				GetAllAssetBundlePath (allFiles, directoryInfos[cnt]);
			}
		}
	}


	private static void GenerateResources(string fileName, string content)
	{
		fileName = GENERATE_SCRIPT_PATH + fileName + ".txt";

		if (!Directory.Exists (GENERATE_SCRIPT_PATH))
		{
			Directory.CreateDirectory (GENERATE_SCRIPT_PATH);
		}

		if (File.Exists(fileName))
		{
			File.Delete(fileName);
		}

		StreamWriter sr = File.CreateText(fileName);
		sr.WriteLine (content);
		sr.Close();
	}
}