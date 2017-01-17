using UnityEngine;
using UnityEditor;
using System.IO;

public class TaskTool : EditorWindow
{
	private static string TEMPLATE_PATH = "Assets/TEDCore/Tools/TaskTool/Template_Task.txt";
	private static string GENERATE_SCRIPT_PATH = Application.dataPath + "/Scripts/Tasks/";
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
			if (File.Exists (GENERATE_SCRIPT_PATH + _taskName + "Task.cs"))
			{
				GUILayout.TextField (_taskName + "Task.cs have already exist.");
			}
			else if (GUILayout.Button ("Generate"))
			{
				string template = GetTemplate (TEMPLATE_PATH);
				template = template.Replace ("$TaskName", _taskName + "Task");

				GenerateScript (_taskName + "Task", template);

				AssetDatabase.Refresh ();
			}
		}

		GUILayout.EndVertical ();
	}

	private static string GetTemplate(string path)
	{
		TextAsset txt = (TextAsset)AssetDatabase.LoadAssetAtPath(path, typeof(TextAsset));

		return txt.text;
	}

	private static void GenerateScript(string fileName, string content)
	{
		fileName = GENERATE_SCRIPT_PATH + fileName + ".cs";

		if (!Directory.Exists (GENERATE_SCRIPT_PATH))
		{
			Directory.CreateDirectory (GENERATE_SCRIPT_PATH);
		}

		if (File.Exists(fileName))
		{
			File.Delete (fileName);
		}

		StreamWriter sr = File.CreateText(fileName);
		sr.WriteLine (content);
		sr.Close();
	}
}