using System.IO;
using UnityEditor;

public class FileHandler
{
	public static string TXT_EXTENSION = ".txt";
	public static string CS_EXTENSION = ".cs";

	public static void GenerateScript(string folderPath, string fileName, string content)
	{
		if (!Directory.Exists (folderPath))
		{
			Directory.CreateDirectory (folderPath);
		}

		if (File.Exists(folderPath + fileName + CS_EXTENSION))
		{
			File.Delete(folderPath + fileName + CS_EXTENSION);
		}

		StreamWriter sr = File.CreateText(folderPath + fileName + CS_EXTENSION);
		sr.WriteLine (content);
		sr.Close();

		AssetDatabase.Refresh ();
	}

	public static void GenerateTxt(string folderPath, string fileName, string content)
	{
		if (!Directory.Exists (folderPath))
		{
			Directory.CreateDirectory (folderPath);
		}

		if (File.Exists(folderPath + fileName + TXT_EXTENSION))
		{
			File.Delete(folderPath + fileName + TXT_EXTENSION);
		}

		StreamWriter sr = File.CreateText(folderPath + fileName + TXT_EXTENSION);
		sr.WriteLine (content);
		sr.Close();

		AssetDatabase.Refresh ();
	}
}
