using UnityEngine;
using UnityEditor;
using System.IO;

public class StateTool : EditorWindow
{
	private static string TEMPLATE_PATH = "Assets/TEDCore/Tools/StateTool/Template_State.txt";
	private static string GENERATE_SCRIPT_PATH = Application.dataPath + "/Scripts/States/";
	private static string STATE = "State";

	private string _stateName;

	[MenuItem("TEDTools/State/Generate")]
	public static void ShowWindow()
	{
		EditorWindow.GetWindow(typeof(StateTool));
	}

	private void OnGUI()
	{
		GUILayout.BeginVertical ();
		_stateName = EditorGUILayout.TextField ("State Name", _stateName);

		if (!string.IsNullOrEmpty (_stateName))
		{
			if (File.Exists (GENERATE_SCRIPT_PATH + _stateName + STATE + ".cs"))
			{
				GUILayout.TextField (_stateName + STATE + ".cs have already exist.");
			}
			else if (GUILayout.Button ("Generate"))
			{
				string template = GetTemplate (TEMPLATE_PATH);
				template = template.Replace ("$StateName", _stateName + STATE);

				GenerateScript (_stateName + STATE, template);

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