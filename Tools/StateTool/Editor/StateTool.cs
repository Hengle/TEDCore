using UnityEngine;
using UnityEditor;
using System.IO;

public class StateTool : EditorWindow
{
	private static string TEMPLATE_PATH = "Assets/TEDCore/Tools/StateTool/Templates/State.txt";
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
			string fileName = _stateName + STATE;

			if (File.Exists (GENERATE_SCRIPT_PATH + fileName + FileHandler.CS_EXTENSION))
			{
				GUILayout.TextField (_stateName + STATE + FileHandler.CS_EXTENSION + " have already exist.");
			}
			else if (GUILayout.Button ("Generate"))
			{
				string template = GetTemplate (TEMPLATE_PATH);
				template = template.Replace ("$StateName", fileName);

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