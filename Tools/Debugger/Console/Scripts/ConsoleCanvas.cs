using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace TEDCore.Debugger.Console
{
    public class ConsoleCanvas : MonoBehaviour
    {
        [SerializeField] private GameObject m_screen;
        [SerializeField] private Button m_clearButton;
        [SerializeField] private Toggle m_collapseToggle;
        [SerializeField] private Button m_scrollToBottomButton;
        [SerializeField] private InputField m_searchFilter;
        [SerializeField] private ConsoleToggle m_consoleToggleLog;
        [SerializeField] private ConsoleToggle m_consoleToggleWarning;
        [SerializeField] private ConsoleToggle m_consoleToggleError;
        [SerializeField] private ConsoleLogPool m_consoleLogPool;
        [SerializeField] private ScrollRect m_logScrollRect;

        private ConsoleLogFilter m_consoleLogFilter;
        private List<ConsoleLogData> m_consoleLogDatas = new List<ConsoleLogData>();
        private List<ConsoleLog> m_displayElements = new List<ConsoleLog>();

        private bool m_collapseToggleValue;
        private bool m_forceScrollToBottom = true;
        private int m_currentDisplayIndex;
        private int m_displayIndex;

        private void Awake()
        {
            m_consoleLogFilter = new ConsoleLogFilter();
            m_consoleLogFilter.OnUpdateFinished += OnUpdateFinished;

            m_clearButton.onClick.AddListener(OnClearButtonClick);
            m_collapseToggle.onValueChanged.AddListener(OnCollapseToggleValueChanged);
            m_scrollToBottomButton.onClick.AddListener(OnScrollToBottomButtonClick);
            m_searchFilter.onValueChanged.AddListener(OnSearchFilterValueChanged);
            m_consoleToggleLog.SetToggleValueChanged(OnLogToggleValueChanged);
            m_consoleToggleWarning.SetToggleValueChanged(OnWarningToggleValueChanged);
            m_consoleToggleError.SetToggleValueChanged(OnErrorToggleValueChanged);

            Application.logMessageReceived += m_consoleLogFilter.OnLogMessageReceived;
            Application.logMessageReceivedThreaded += m_consoleLogFilter.LogMessageReceivedThreaded;
        }

        private void OnDestroy()
        {
            Application.logMessageReceived -= m_consoleLogFilter.OnLogMessageReceived;
            Application.logMessageReceivedThreaded += m_consoleLogFilter.LogMessageReceivedThreaded;
        }

        private void Update()
        {
            if(!m_screen.activeInHierarchy)
            {
                return;
            }

            m_forceScrollToBottom = m_logScrollRect.verticalNormalizedPosition < 0.1f;
            UpdateElementDisplayRange();
        }

        private void OnClearButtonClick()
        {
            foreach (ConsoleLog log in m_displayElements)
            {
                m_consoleLogPool.Recovery(log);
            }

            m_displayElements.Clear();
            m_consoleLogFilter.Clear();
        }

        private void OnCollapseToggleValueChanged(bool value)
        {
            m_collapseToggleValue = value;
            m_consoleLogFilter.SetCollapseToggle(value);
        }

        private void OnScrollToBottomButtonClick()
        {
            m_forceScrollToBottom = true;
            OnScrollToBottom();
        }

        private void OnSearchFilterValueChanged(string value)
        {
            m_consoleLogFilter.SetSearchFilter(value);
        }

        private void OnLogToggleValueChanged(bool value)
        {
            m_consoleLogFilter.SetLogToggle(value);
        }

        private void OnWarningToggleValueChanged(bool value)
        {
            m_consoleLogFilter.SetWarningToggle(value);
        }

        private void OnErrorToggleValueChanged(bool value)
        {
            m_consoleLogFilter.SetErrorToggle(value);
        }

        private void OnUpdateFinished(List<ConsoleLogData> consoleLogDatas, int logCount, int warningCount, int errorCount)
        {
            m_consoleLogDatas = consoleLogDatas;
            m_consoleToggleLog.SetCount(logCount);
            m_consoleToggleWarning.SetCount(warningCount);
            m_consoleToggleError.SetCount(errorCount);

            UpdateUIElements();
        }

        private void UpdateUIElements()
        {
            int filterLogDataCount = m_consoleLogDatas.Count;
            int displayElementCount = m_displayElements.Count;

            if (filterLogDataCount != displayElementCount)
            {
                if (displayElementCount > filterLogDataCount)
                {
                    for (int i = 0; i < displayElementCount - filterLogDataCount; i++)
                    {
                        ConsoleLog consoleLog = m_displayElements[0];
                        m_consoleLogPool.Recovery(consoleLog);
                        m_displayElements.Remove(consoleLog);
                    }
                }
                else
                {
                    for (int i = 0; i < filterLogDataCount - displayElementCount; i++)
                    {
                        m_displayElements.Add(m_consoleLogPool.Get());
                    }
                }

                for (int i = 0; i < m_consoleLogDatas.Count; i++)
                {
                    m_displayElements[i].SetLogType(m_consoleLogDatas[i].LogType);
                    m_displayElements[i].SetLogString(m_consoleLogDatas[i].LogString);
                    m_displayElements[i].SetStackTrace(m_consoleLogDatas[i].StackTrace);
                    m_displayElements[i].SetBackgroundColor(i);
                    m_displayElements[i].SetCollapsed(m_collapseToggleValue);
                    m_displayElements[i].SetCollapsedCount(m_consoleLogDatas[i].CollapseCount);
                }

                m_currentDisplayIndex = -1;
            }
            else
            {
                for (int i = 0; i < m_consoleLogDatas.Count; i++)
                {
                    m_displayElements[i].SetCollapsedCount(m_consoleLogDatas[i].CollapseCount);
                }
            }

            UpdateElementDisplayRange();
            OnScrollToBottom();
        }

        private void UpdateElementDisplayRange()
        {
            m_displayIndex = (int)(Mathf.Clamp(1 - m_logScrollRect.verticalNormalizedPosition, 0, 1) * m_consoleLogDatas.Count);
            if (m_currentDisplayIndex != m_displayIndex)
            {
                m_currentDisplayIndex = m_displayIndex;
                for (int i = 0; i < m_displayElements.Count; i++)
                {
                    m_displayElements[i].SetEnable(i >= m_currentDisplayIndex - 15 && i <= m_currentDisplayIndex + 15);
                }
            }
        }

        private void OnScrollToBottom()
        {
            if (!m_forceScrollToBottom)
            {
                return;
            }

            Canvas.ForceUpdateCanvases();
            m_logScrollRect.verticalNormalizedPosition = 0;
        }
    }
}
