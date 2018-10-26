using UnityEditor;
using UnityEngine;
using System.IO;

namespace TEDCore.TemplateScript
{
    public class TemplateScriptTool
    {
        private const string TEMPLATE_PATH = "Tools/TemplateScriptTool/Templates/";

        [MenuItem("Assets/Create/C# Template Scripts/Empty State", false, 80)]
        private static void CreateEmptyState()
        {
            CreateTemplateScript(TEMPLATE_PATH + "TemplateState.txt", "NewState.cs");
        }


        [MenuItem("Assets/Create/C# Template Scripts/Empty Task", false, 80)]
        private static void CreateEmptyTask()
        {
            CreateTemplateScript(TEMPLATE_PATH + "TemplateTask.txt", "NewTask.cs");
        }


        [MenuItem("Assets/Create/C# Template Scripts/Empty View", false, 80)]
        private static void CreateEmptyElement()
        {
            CreateTemplateScript(TEMPLATE_PATH + "TemplateView.txt", "NewView.cs");
        }


        private static void CreateTemplateScript(string resourcePath, string pathName)
        {
            string resourceFile = Path.Combine(Application.dataPath, resourcePath);

            Texture2D csIcon = EditorGUIUtility.IconContent("cs Script Icon").image as Texture2D;

            ReplaceTemplateScriptNameAction endNameEditAction = ScriptableObject.CreateInstance<ReplaceTemplateScriptNameAction>();
            ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0, endNameEditAction, pathName, csIcon, resourceFile);
        }
    }
}