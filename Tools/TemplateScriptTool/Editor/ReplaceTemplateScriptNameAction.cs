using UnityEngine;
using UnityEditor;
using UnityEditor.ProjectWindowCallback;
using System.IO;

namespace TEDCore.TemplateScript
{
    public class ReplaceTemplateScriptNameAction : EndNameEditAction
    {
        public override void Action(int instanceId, string pathName, string resourceFile)
        {
            string text = File.ReadAllText(resourceFile);
            string scriptName = Path.GetFileNameWithoutExtension(pathName);

            text = text.Replace("$SCRIPTNAME", scriptName);

            StreamWriter sr = File.CreateText(pathName);
            sr.WriteLine(text);
            sr.Close();

            AssetDatabase.ImportAsset(pathName);
            ProjectWindowUtil.ShowCreatedAsset(AssetDatabase.LoadAssetAtPath<TextAsset>(pathName));
            AssetDatabase.Refresh();
        }
    }
}
