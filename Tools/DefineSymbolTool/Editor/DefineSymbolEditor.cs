#if UNITY_EDITOR

using UnityEditor;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TEDCore.DefineSymbol
{
    public static class DefineSymbolEditor
    {
        private const string DEFINE_SYMBOL_FOLDER = "Assets/DefineSymbol/";
        private const string DEFINE_SYMBOL_SETTING_FILE_NAME = "DefineSymbolSettings.asset";
        private const string DEFINE_SYMBOL_SEPARATOR = ";";

        public static bool IsEdited { get { return m_isEdited || PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup) != GetResult(); } }
        private static bool m_isEdited;

        public static List<DefineSymbol> DefineSymbols { get { return m_defineSymbols; } }
        private static List<DefineSymbol> m_defineSymbols;

        public static void Initialize()
        {
            m_isEdited = false;
            m_defineSymbols = new List<DefineSymbol>();

            var defineSymbolSettings = (DefineSymbolSettings)AssetDatabase.LoadAssetAtPath(DEFINE_SYMBOL_FOLDER + DEFINE_SYMBOL_SETTING_FILE_NAME, typeof(DefineSymbolSettings));
            if (null == defineSymbolSettings)
            {
                var projectScriptingDefineSymbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup).Split(new char[] { ';' });
                for (int i = 0; i < projectScriptingDefineSymbols.Length; i++)
                {
                    m_defineSymbols.Add(new DefineSymbol(projectScriptingDefineSymbols[i], "", true));
                }

                Save();

                defineSymbolSettings = (DefineSymbolSettings)AssetDatabase.LoadAssetAtPath(DEFINE_SYMBOL_FOLDER + DEFINE_SYMBOL_SETTING_FILE_NAME, typeof(DefineSymbolSettings));
            }

            m_defineSymbols = defineSymbolSettings.defineSymbols.ToList();
        }

        public static void UpdateDefineSymbol(int index, string key, string description, bool isEnabled)
        {
            m_isEdited = true;
            var symbolCount = m_defineSymbols.Count;

            if (index >= symbolCount)
            {
                for (int i = symbolCount - 1; i < index; i++)
                {
                    var newSymbol = new DefineSymbol(key, description, isEnabled);
                    m_defineSymbols.Add(newSymbol);
                }
            }

            var symbol = m_defineSymbols[index];
            symbol.Update(key, description, isEnabled);
        }

        public static void Delete(int index)
        {
            if (index >= m_defineSymbols.Count)
            {
                return;
            }

            m_isEdited = true;

            m_defineSymbols.RemoveAt(index);
        }

        public static string GetResult()
        {
            var scriptingDefineSymbols = string.Empty;
            for (int i = 0; i < m_defineSymbols.Count; i++)
            {
                if (m_defineSymbols[i].Enable)
                {
                    if (!string.IsNullOrEmpty(scriptingDefineSymbols))
                    {
                        scriptingDefineSymbols += DEFINE_SYMBOL_SEPARATOR;
                    }
                    scriptingDefineSymbols += m_defineSymbols[i].Key;
                }
            }

            return scriptingDefineSymbols;
        }

        public static void Save()
        {
            m_isEdited = false;

            PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup, GetResult());

            SaveScriptableObject();

            AssetDatabase.SaveAssets();
        }

        private static void SaveScriptableObject()
        {
            if (!Directory.Exists(DEFINE_SYMBOL_FOLDER))
            {
                Directory.CreateDirectory(DEFINE_SYMBOL_FOLDER);
            }

            if (File.Exists(DEFINE_SYMBOL_FOLDER + DEFINE_SYMBOL_SETTING_FILE_NAME))
            {
                var DefineSymbolSettings = (DefineSymbolSettings)AssetDatabase.LoadAssetAtPath(DEFINE_SYMBOL_FOLDER + DEFINE_SYMBOL_SETTING_FILE_NAME, typeof(DefineSymbolSettings));
                DefineSymbolSettings.defineSymbols = m_defineSymbols.ToArray();
                EditorUtility.SetDirty(DefineSymbolSettings);
            }
            else
            {
                var DefineSymbolSettings = UnityEngine.ScriptableObject.CreateInstance<DefineSymbolSettings>();
                DefineSymbolSettings.defineSymbols = m_defineSymbols.ToArray();
                AssetDatabase.CreateAsset(DefineSymbolSettings, DEFINE_SYMBOL_FOLDER + DEFINE_SYMBOL_SETTING_FILE_NAME);
            }
        }
    }
}

#endif