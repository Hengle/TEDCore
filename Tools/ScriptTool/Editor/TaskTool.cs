using UnityEngine;
using UnityEditor;
using System.IO;

public class TaskTool : EditorWindow
{
	private static string TEMPLATE_PATH = "Assets/TEDCore/Tools/ScriptTool/Templates/Task.txt";
	private static string GENERATE_SCRIPT_PATH = Application.dataPath + "/Scripts/Tasks/";
	private static string FILE_CLASS = "Task";

	private string _fileName;

	[MenuItem("TEDTools/Scripts/Generate Empty Task")]
	public static void ShowWindow()
	{
		EditorWindow.GetWindow(typeof(TaskTool));
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
				GUILayout.TextField (fileName + FileHandler.CS_EXTENSION + " have already exist.");
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