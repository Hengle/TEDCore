using UnityEngine;
using TEDCore.Performance;
using UnityEngine.UI;

namespace TEDCore.Debugger
{
    public class PerformanceCanvas : MonoBehaviour
    {
        private const int HIGH_FPS = 60;
        private const int MIDIUM_FPS = 30;
        private const string FPS_FORMAT = "Cur FPS: {0}\nMin FPS: {1}\nMax FPS: {2}\nAvg FPS: {3}";

        [SerializeField] private bool m_updateColor;
        [SerializeField] private float m_updateDuration = 1.0f;
        [SerializeField] private Image m_colorBackground;
        [SerializeField] private Text m_fpsText;
        [SerializeField] private Button m_resetButton;

        private PerformanceManager m_performanceManager;
        private Color m_color;

        private void Awake()
        {
            m_performanceManager = PerformanceManager.Instance;
            m_performanceManager.SetUpdateDuration(m_updateDuration);
            m_resetButton.onClick.AddListener(() => m_performanceManager.Reset());
        }

        private void Update()
        {
            if (m_updateColor)
            {
                if (m_performanceManager.FpsData.CurValue >= HIGH_FPS)
                {
                    m_color = Color.green;
                }
                else if (m_performanceManager.FpsData.CurValue >= MIDIUM_FPS)
                {
                    m_color = Color.yellow;
                }
                else
                {
                    m_color = Color.red;
                }
            }
            else
            {
                m_color = Color.white;
            }

            m_colorBackground.color = m_color * 0.5f;
            m_fpsText.text = string.Format(FPS_FORMAT, m_performanceManager.FpsData.CurValue, m_performanceManager.FpsData.MinValue, m_performanceManager.FpsData.MaxValue, m_performanceManager.FpsData.AvgValue);
        }
    }
}
