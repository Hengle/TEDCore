using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace TEDCore.Toolbar
{
    public class UGUIToolbar : EditorWindow, IHasCustomMenu
    {
        private class ButtonData
        {
            public Type Type;
            public string MenuItemPath;

            public ButtonData(Type type, string menuItemPath)
            {
                Type = type;
                MenuItemPath = menuItemPath;
            }
        }

        private static readonly ButtonData[] BUTTON_DATAS =
        {
            new ButtonData(typeof(GameObject), "GameObject/Create Empty"),
            new ButtonData(typeof(Text), "GameObject/UI/Text"),
            new ButtonData(typeof(Image), "GameObject/UI/Image"),
            new ButtonData(typeof(RawImage), "GameObject/UI/Raw Image"),
            new ButtonData(typeof(Button), "GameObject/UI/Button"),
            new ButtonData(typeof(Toggle), "GameObject/UI/Toggle"),
            new ButtonData(typeof(Slider), "GameObject/UI/Slider"),
            new ButtonData(typeof(Scrollbar), "GameObject/UI/Scrollbar"),
            new ButtonData(typeof(Dropdown), "GameObject/UI/Dropdown"),
            new ButtonData(typeof(InputField), "GameObject/UI/Input Field"),
            new ButtonData(typeof(Canvas), "GameObject/UI/Canvas"),
            new ButtonData(typeof(ScrollRect), "GameObject/UI/Scroll View"),

            new ButtonData(typeof(Outline), "Component/UI/Effects/Outline"),
            new ButtonData(typeof(PositionAsUV1), "Component/UI/Effects/Position As UV1"),
            new ButtonData(typeof(Shadow), "Component/UI/Effects/Shadow"),
            new ButtonData(typeof(Mask), "Component/UI/Mask"),
            new ButtonData(typeof(RectMask2D), "Component/UI/Rect Mask 2D"),
        };

        private static readonly GUILayoutOption[] BUTTON_OPTIONS =
        {
            GUILayout.Height(32),
        };

        private const string WINDOW_NAME = "UGUI Toolbar";
        private const int WINDOW_HEIGHT = 36;
        private const int WINDOW_WIDTH = 160;
        private const int SCROLL_VIEW_SIZE = 12;

        private const string PREFS_IS_VERTICAL = "UGUI Toolbar IsVertical";
        private static bool IsVertical
        {
            get { return EditorPrefs.GetBool(PREFS_IS_VERTICAL, false); }
            set
            {
                EditorPrefs.SetBool(PREFS_IS_VERTICAL, value);

                UGUIToolbar uGUIToolbar = GetWindow<UGUIToolbar>(WINDOW_NAME);
                if (IsVertical)
                {
                    Vector2 size = new Vector2(WINDOW_WIDTH, WINDOW_HEIGHT);
                    uGUIToolbar.minSize = size;

                    size = uGUIToolbar.maxSize;
                    size.x = WINDOW_WIDTH;
                    uGUIToolbar.maxSize = size;
                }
                else
                {
                    Vector2 size = new Vector2(WINDOW_WIDTH, WINDOW_HEIGHT + SCROLL_VIEW_SIZE);
                    uGUIToolbar.minSize = size;

                    size = uGUIToolbar.maxSize;
                    size.y = WINDOW_HEIGHT + SCROLL_VIEW_SIZE;
                    uGUIToolbar.maxSize = size;
                }
            }
        }

        private const string PREFS_SHOW_TEXT = "UGUI Toolbar ShowText";
        private static bool ShowText
        {
            get { return EditorPrefs.GetBool(PREFS_SHOW_TEXT, true); }
            set { EditorPrefs.SetBool(PREFS_SHOW_TEXT, value); }
        }

        [MenuItem("TEDCore/Toolbar/UGUI Toolbar")]
        private static void OpenWindow()
        {
            UGUIToolbar uGUIToolbar = GetWindow<UGUIToolbar>(WINDOW_NAME);
            uGUIToolbar.Show();

            if (IsVertical)
            {
                Rect position = uGUIToolbar.position;
                position.width = WINDOW_WIDTH;
                uGUIToolbar.position = position;

                Vector2 size = new Vector2(WINDOW_WIDTH, WINDOW_HEIGHT);
                uGUIToolbar.minSize = size;

                size = uGUIToolbar.maxSize;
                size.x = WINDOW_WIDTH;
                uGUIToolbar.maxSize = size;
            }
            else
            {
                Rect position = uGUIToolbar.position;
                position.height = WINDOW_HEIGHT + SCROLL_VIEW_SIZE;
                uGUIToolbar.position = position;

                Vector2 size = new Vector2(WINDOW_WIDTH, WINDOW_HEIGHT + SCROLL_VIEW_SIZE);
                uGUIToolbar.minSize = size;

                size = uGUIToolbar.maxSize;
                size.y = WINDOW_HEIGHT + SCROLL_VIEW_SIZE;
                uGUIToolbar.maxSize = size;
            }
        }

        private Vector2 m_scrollPosition;
        private void OnGUI()
        {
            m_scrollPosition = EditorGUILayout.BeginScrollView(m_scrollPosition);

            if (IsVertical)
            {
                m_scrollPosition.x = 0;
                EditorGUILayout.BeginVertical();
            }
            else
            {
                m_scrollPosition.y = 0;
                EditorGUILayout.BeginHorizontal();
            }

            GameObject parent = Selection.activeGameObject;

            for (int i = 0; i < BUTTON_DATAS.Length; i++)
            {
                GUIContent guiContent = EditorGUIUtility.ObjectContent(null, BUTTON_DATAS[i].Type);

                if (ShowText)
                {
                    guiContent.text = BUTTON_DATAS[i].Type.Name;
                }
                else
                {
                    guiContent.text = string.Empty;
                }

                guiContent.tooltip = BUTTON_DATAS[i].MenuItemPath;
                if (GUILayout.Button(guiContent, BUTTON_OPTIONS))
                {
                    EditorApplication.ExecuteMenuItem(BUTTON_DATAS[i].MenuItemPath);
                    GameObject instance = Selection.activeGameObject;
                    instance.name = BUTTON_DATAS[i].Type.Name;
                    GameObjectUtility.SetParentAndAlign(instance, parent);
                }
            }

            if (IsVertical)
            {
                EditorGUILayout.EndVertical();
            }
            else
            {
                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.EndScrollView();
        }

        public void AddItemsToMenu(GenericMenu menu)
        {
            menu.AddItem(new GUIContent("Show Text"), ShowText, () =>
            {
                ShowText = !ShowText;
            });

            menu.AddItem(new GUIContent("Vertical"), IsVertical, () =>
            {
                IsVertical = !IsVertical;
            });
        }
    }
}