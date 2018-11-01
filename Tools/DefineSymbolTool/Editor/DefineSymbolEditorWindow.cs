#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

namespace TEDCore.DefineSymbol
{
    public class DefineSymbolEditorWindow : EditorWindow
    {
        private Vector2 m_scrollPosition = Vector2.zero;
        private static DefineSymbolEditorWindow m_window;

        [MenuItem("TEDCore/Define Symbol Window")]
        private static void OpenWindow()
        {
            m_window = (DefineSymbolEditorWindow)EditorWindow.GetWindow(typeof(DefineSymbolEditorWindow), false, "DefineSymbol Window");
            m_window.Show();
        }

        private void OnFocus()
        {
            DefineSymbolEditor.Initialize();
        }

        private void OnGUI()
        {
            if (EditorApplication.isPlaying)
            {
                EditorGUILayout.HelpBox("DefineSymbolSettings can't be modified in runtime.", MessageType.Warning);
                return;
            }

            if (EditorApplication.isCompiling)
            {
                EditorGUILayout.HelpBox("DefineSymbolSettings can't be modified in compiling.", MessageType.Warning);
                return;
            }

            m_scrollPosition = EditorGUILayout.BeginScrollView(m_scrollPosition);

            CreateMenu();
            CreateScriptingDefineSymbols();
            CreateButtons();

            EditorGUILayout.EndScrollView();
        }

        private void CreateMenu()
        {
            EditorGUILayout.BeginVertical(GUI.skin.box);

            EditorGUILayout.PrefixLabel("Scripting Define Symbols");

            var defineSymbols = DefineSymbolEditor.DefineSymbols;
            for (int i = 0; i < defineSymbols.Count; i++)
            {
                CreateSymbolMenuParts(defineSymbols[i], i);
                GUILayout.Space(5);
            }

            var newSymbol = new DefineSymbol(string.Empty, string.Empty, false);
            CreateSymbolMenuParts(newSymbol, defineSymbols.Count);

            EditorGUILayout.EndVertical();
        }

        private void CreateSymbolMenuParts(DefineSymbol symbol, int symbolIndex)
        {
            EditorGUILayout.BeginVertical(symbol.Enable ? GUI.skin.button : GUI.skin.textField);
            {
                EditorGUI.BeginChangeCheck();

                var isEnabled = symbol.Enable;

                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUI.BeginChangeCheck();
                    EditorGUILayout.LabelField("Enabled", GUILayout.Width(80));
                    isEnabled = EditorGUILayout.Toggle(isEnabled, GUILayout.Width(15));

                    if (symbolIndex < DefineSymbolEditor.DefineSymbols.Count)
                    {
                        GUILayout.FlexibleSpace();
                        if (GUILayout.Button("X", GUILayout.Width(20), GUILayout.Height(14)))
                        {
                            DefineSymbolEditor.Delete(symbolIndex);
                            return;
                        }
                    }
                }
                EditorGUILayout.EndHorizontal();

                var key = symbol.Key;
                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField("Key", GUILayout.Width(80));
                    key = GUILayout.TextField(key);
                }
                EditorGUILayout.EndHorizontal();

                var description = symbol.Description;
                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField("Description", GUILayout.Width(80));
                    description = GUILayout.TextField(description);
                }
                EditorGUILayout.EndHorizontal();

                if (EditorGUI.EndChangeCheck())
                {
                    DefineSymbolEditor.UpdateDefineSymbol(symbolIndex, key, description, isEnabled);
                }

            }
            EditorGUILayout.EndVertical();
        }

        private void CreateScriptingDefineSymbols()
        {
            GUILayout.Space(20);

            EditorGUILayout.LabelField("Current Scripting Define Symbols");
            EditorGUILayout.HelpBox(PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup), MessageType.None);

            GUILayout.Space(20);

            EditorGUILayout.LabelField("New Scripting Define Symbols");
            EditorGUILayout.HelpBox(DefineSymbolEditor.GetResult(), MessageType.None);
        }

        private void CreateButtons()
        {
            GUILayout.Space(20);

            if (DefineSymbolEditor.IsEdited)
            {
                EditorGUILayout.HelpBox("DefineSymbolSettings has been modified, keep in mind to save it or not.", MessageType.Warning);

                GUILayout.Space(20);

                EditorGUILayout.BeginVertical(GUI.skin.box);
                CreateResetButton();
                CreateSaveButton();
                EditorGUILayout.EndVertical();
            }
        }

        private void CreateResetButton()
        {
            if (GUILayout.Button("Reset"))
            {
                DefineSymbolEditor.Initialize();
            }
        }

        private void CreateSaveButton()
        {
            if (GUILayout.Button("Save"))
            {
                DefineSymbolEditor.Save();
            }
        }
    }
}

#endif