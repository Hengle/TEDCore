using UnityEngine;
using UnityEngine.UI;

namespace TEDCore.Debugger.Console
{
    public class ConsoleLog : MonoBehaviour
    {
        [SerializeField]
        private Color[] m_backgroundColors =
        {
            new Color(200f / 256, 200f / 256, 200f / 256, 1f),
            new Color(175f / 256, 175f / 256, 175f / 256, 1f)
        };
        [SerializeField] private Sprite[] m_logTypeSprites;
        [SerializeField] private ConsoleSelectedLog m_consoleSelectedLog;

        [SerializeField] private Image m_backgroundImage;
        [SerializeField] private Image m_logTypeImage;
        [SerializeField] private Text m_logStringText;
        [SerializeField] private Text m_stackTraceText;
        [SerializeField] private RectTransform m_collapseParent;
        [SerializeField] private Text m_collapseText;
        private int m_index;
        private LogType m_logType;
        private string m_logString;
        private string m_stackTrace;
        private int m_collapseCount;

        private void Awake()
        {
            Button button = GetComponent<Button>();
            button.onClick.AddListener(OnClick);
        }

        public void SetBackgroundColor(int index)
        {
            m_backgroundImage.color = m_backgroundColors[index % m_backgroundColors.Length];
        }

        public void SetBackgroundActive(bool value)
        {
            m_backgroundImage.gameObject.SetActive(value);
        }

        public LogType GetLogType()
        {
            return m_logType;
        }

        public void SetLogType(LogType logType)
        {
            m_logType = logType;

            switch (m_logType)
            {
                case LogType.Log:
                    m_index = 0;
                    break;
                case LogType.Warning:
                    m_index = 1;
                    break;
                case LogType.Error:
                case LogType.Exception:
                case LogType.Assert:
                    m_index = 2;
                    break;
            }

            m_logTypeImage.sprite = m_logTypeSprites[m_index];
        }

        public string GetLogString()
        {
            return m_logString;
        }

        public void SetLogString(string text)
        {
            m_logString = text;
            m_logStringText.text = m_logString;
        }

        public string GetStackTrace()
        {
            return m_stackTrace;
        }

        public void SetStackTrace(string text)
        {
            m_stackTrace = text;
            m_stackTraceText.text = m_stackTrace.Split(new char[] { '\n' }, System.StringSplitOptions.RemoveEmptyEntries)[0];
        }

        public void SetCollapsed(bool value)
        {
            m_collapseParent.gameObject.SetActive(value);
        }

        public void AddCollapseCount()
        {
            m_collapseCount++;
            m_collapseText.text = m_collapseCount.ToString();
            UpdateCollapseWidth();
        }

        public void ResetCollapseCount()
        {
            m_collapseCount = 1;
            m_collapseText.text = m_collapseCount.ToString();
            UpdateCollapseWidth();
        }

        private void UpdateCollapseWidth()
        {
            int digit = m_collapseText.text.Length - 2;
            if(digit < 0)
            {
                digit = 0;
            }

            Vector2 sizeDelta = m_collapseParent.sizeDelta;
            sizeDelta.x = 70 + digit * 35;
            m_collapseParent.sizeDelta = sizeDelta;
        }

        private void OnClick()
        {
            m_consoleSelectedLog.SetStackTrace(string.Format("{0}\n{1}", m_logString, m_stackTrace));
        }
    }
}
