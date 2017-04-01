using UnityEngine;
using UnityEditor;
using System.IO;

public class StateTool : EditorWindow
{
	private static string TEMPLATE_PATH = "Assets/TEDCore/Tools/ScriptTool/Templates/State.txt";
	private static string GENERATE_SCRIPT_PATH = Application.dataPath + "/Scripts/States/";
	private static string FILE_CLASS = "State";

	private string _fileName;

	[MenuItem("TEDTools/Scripts/Generate Empty State")]
	public static void ShowWindow()
	{
		EditorWindow.GetWindow(typeof(StateTool));
	}

	private void OnGUI()
	{
		GUILayout.BeginVertical ();
		_fileName = EditorGUILayout.TextField ("File Name", _fileName);

		if (!string.IsNullOrEmpty (_fileName))
		{
			string fileName = _fileName + FILE_CLASS;

			GUILayout.TextField ("Generate script name will be " + _fileName + FILE_CLASS + FileHandler.CS_EXTENSION);

			if (File.Exists (GENERATE_SCRIPT_PATH + fileName + FileHandler.CS_EXTENSION))
			{
				GUILayout.TextField (_fileName + FILE_CLASS + FileHandler.CS_EXTENSION + " have already exist.");
			}
			else if (GUILayout.Button ("Generate"))
			{
				string template = GetTemplate (TEMPLATE_PATH);
				template = template.Replace ("$SCRIPTNAME", fileName);

				FileHandler.GenerateScript (GENERATE_SCRIPT_PATH, fileName, template);
			}
		}

		GUILayout.EndVertical ();
	}

	private static string GetTemplate(string path)
	{
		TextAsset txt = (TextAsset)AssetDatabase.LoadAssetAtPath(path, typeof(TextAsset));

		return txt.text;
	}
}