using UnityEngine;

namespace TEDCore.Performance
{
    public class PerformanceHUD : MonoBehaviour
    {
        private const int LINE_COUNT = 4;
        private const int TEXT_WIDTH = 20;
        private const float TEXT_HEIGHT = 1f;
        private const float BUTTON_HEIGHT = 2f;
        private const float BUTTON_BORDER = 2f;
        private const int PERFORMANCE_HIGH = 30;
        private const int PERFORMANCE_MIDIUM = 20;

        private Rect m_startRect;
        private bool m_updateColor = true;
        private bool m_allowDrag = true;

        private Rect m_dragWindowRect;
        private Rect m_fpsRect;
        private string m_fpsFormat = "FPS: Cur {0}, Min {1}, Max {2}, Avg {3}";
        private Rect m_resetButtonRect;
        private string m_resetButtonText = "Reset";
        private GUIStyle m_textStyle;
        private GUIStyle m_buttonStyle;

        private PerformanceManager m_performanceManager;

        private void Awake()
        {
            m_startRect = new Rect(10, 10, (float)Screen.height * 0.4f, (float)Screen.height / 30 * LINE_COUNT);
            m_dragWindowRect = new Rect(0, 0, Screen.width, Screen.height);

            m_performanceManager = PerformanceManager.Instance;
        }


        private void OnGUI()
        {
            if (m_textStyle == null)
            {
                m_textStyle = new GUIStyle(GUI.skin.label);
                m_textStyle.normal.textColor = Color.white;
                m_textStyle.alignment = TextAnchor.MiddleLeft;
                m_textStyle.fontSize = (int)((m_startRect.width - 5) / TEXT_WIDTH);

                m_fpsRect = new Rect(5, 0, m_startRect.width - 10, m_startRect.height * TEXT_HEIGHT / LINE_COUNT);
                m_resetButtonRect = new Rect(BUTTON_BORDER, m_startRect.height * TEXT_HEIGHT * 2 / LINE_COUNT, m_startRect.width - BUTTON_BORDER * 2, m_startRect.height * BUTTON_HEIGHT / LINE_COUNT - BUTTON_BORDER);
            }

            if (m_buttonStyle == null)
            {
                m_buttonStyle = new GUIStyle(GUI.skin.button);
                m_buttonStyle.fontSize = (int)((m_startRect.width - 5) / (TEXT_WIDTH * 0.5f));
            }

            if (m_updateColor)
            {
                if (m_performanceManager.FpsData.CurValue >= PERFORMANCE_HIGH)
                {
                    GUI.color = Color.green;
                }
                else if (m_performanceManager.FpsData.CurValue >= PERFORMANCE_MIDIUM)
                {
                    GUI.color = Color.yellow;
                }
                else
                {
                    GUI.color = Color.red;
                }
            }
            else
            {
                GUI.color = Color.white;
            }

            m_startRect = GUI.Window(0, m_startRect, DrawWindow, "");
        }


        private void DrawWindow(int windowID)
        {
            GUI.Label(m_fpsRect, string.Format(m_fpsFormat, m_performanceManager.FpsData.CurValue, m_performanceManager.FpsData.MinValue, m_performanceManager.FpsData.MaxValue, m_performanceManager.FpsData.AvgValue), m_textStyle);

            if (GUI.Button(m_resetButtonRect, m_resetButtonText, m_buttonStyle))
            {
                m_performanceManager.Reset();
            }

            if (m_allowDrag)
            {
                GUI.DragWindow(m_dragWindowRect);
            }
        }
    }
}
