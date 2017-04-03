using UnityEditor;
using UnityEngine;
using System.IO;

public class TemplateScriptTool
{
	[MenuItem ("Assets/Create/C# Template Scripts/Empty State", false, 80)]
	private static void CreateEmptyState ()
	{
		CreateTemplateScript ("TEDCore/Tools/TemplateScriptTool/Templates/TemplateState.txt", "NewState.cs");
	}

	[MenuItem ("Assets/Create/C# Template Scripts/Empty Task", false, 80)]
	private static void CreateEmptyTask ()
	{
		CreateTemplateScript ("TEDCore/Tools/TemplateScriptTool/Templates/TemplateTask.txt", "NewTask.cs");
	}

	private static void CreateTemplateScript(string resourcePath, string pathName)
	{
		string resourceFile = Path.Combine (Application.dataPath, resourcePath);

		Texture2D csIcon = EditorGUIUtility.IconContent ("cs Script Icon").image as Texture2D;

		ReplaceTemplateScriptNameAction endNameEditAction = ScriptableObject.CreateInstance<ReplaceTemplateScriptNameAction> ();

		ProjectWindowUtil.StartNameEditingIfProjectWindowExists (0, endNameEditAction, pathName, csIcon, resourceFile);
	}
}