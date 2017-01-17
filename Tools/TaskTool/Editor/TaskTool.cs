using UnityEngine;
using UnityEditor;
using System.IO;

public class TaskTool : EditorWindow
{
	private static string TEMPLATE_PATH = "Assets/TEDCore/Tools/TaskTool/Templates/Task.txt";
	private static string GENERATE_SCRIPT_PATH = Application.dataPath + "/Scripts/Tasks/";
	private static string TASK = "Task";

	private string _taskName;

	[MenuItem("TEDTools/Task/Generate")]
	public static void ShowWindow()
	{
		EditorWindow.GetWindow(typeof(TaskTool));
	}

	private void OnGUI()
	{
		GUILayout.BeginVertical ();
		_taskName = EditorGUILayout.TextField ("Task Name", _taskName);

		if (!string.IsNullOrEmpty (_taskName))
		{
			string fileName = _taskName + TASK;

			if (File.Exists (GENERATE_SCRIPT_PATH + fileName + FileHandler.CS_EXTENSION))
			{
				GUILayout.TextField (fileName + FileHandler.CS_EXTENSION + " have already exist.");
			}
			else if (GUILayout.Button ("Generate"))
			{
				string template = GetTemplate (TEMPLATE_PATH);
				template = template.Replace ("$TaskName", fileName);

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