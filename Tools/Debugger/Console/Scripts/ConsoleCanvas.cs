using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace TEDCore.Debugger.Console
{
    public class ConsoleCanvas : MonoBehaviour
    {
        [System.Serializable]
        private class LogData
        {
            public string LogString;
            public string StackTrace;
            public LogType LogType;
            public int CollapseCount;

            public LogData(string logString, string stackTrace, LogType logType)
            {
                LogString = logString;
                StackTrace = stackTrace;
                LogType = logType;
                CollapseCount = 0;
            }
        }

        [SerializeField] private Button m_clearButton;
        [SerializeField] private Toggle m_collapseToggle;
        [SerializeField] private Button m_scrollToBottomButton;
        [SerializeField] private InputField m_searchFilter;
        [SerializeField] private ConsoleToggle m_consoleToggleLog;
        [SerializeField] private ConsoleToggle m_consoleToggleWarning;
        [SerializeField] private ConsoleToggle m_consoleToggleError;

        [SerializeField] private ConsoleLogPool m_consoleLogPool;

        [SerializeField] private ScrollRect m_logScrollRect;

        private List<LogData> m_allLogDatas;
        private List<LogData> m_collapseLogDatas;
        private List<LogData> m_toggleLogDatas;
        private List<LogData> m_filterLogDatas;
        private List<ConsoleLog> m_displayElements = new List<ConsoleLog>();

        private bool m_collapseToggleValue;
        private string m_searchFilterText;
        private bool m_logToggleValue = true;
        private bool m_warningToggleValue = true;
        private bool m_errorToggleValue = true;
        private bool m_forceScrollToBottom = true;
        private int m_currentDisplayIndex;
        private int m_displayIndex;

        private void Awake()
        {
            m_allLogDatas = new List<LogData>();
            m_collapseLogDatas = new List<LogData>();
            m_toggleLogDatas = new List<LogData>();
            m_filterLogDatas = new List<LogData>();

            m_clearButton.onClick.AddListener(OnClearButtonClick);
            m_collapseToggle.onValueChanged.AddListener(OnCollapseToggleValueChanged);
            m_scrollToBottomButton.onClick.AddListener(OnScrollToBottomButtonClick);
            m_searchFilter.onValueChanged.AddListener(OnSearchFilterValueChanged);
            m_consoleToggleLog.SetToggleValueChanged(OnLogToggleValueChanged);
            m_consoleToggleWarning.SetToggleValueChanged(OnWarningToggleValueChanged);
            m_consoleToggleError.SetToggleValueChanged(OnErrorToggleValueChanged);

            UpdateLogDatas();

            Application.logMessageReceived += HandleLog;
        }

        private void Update()
        {
            m_forceScrollToBottom = m_logScrollRect.verticalNormalizedPosition < 0.1f;
            UpdateUIElements();
        }

        private void OnClearButtonClick()
        {
            foreach (ConsoleLog log in m_displayElements)
            {
                m_consoleLogPool.Recovery(log);
            }

            m_displayElements.Clear();
            m_allLogDatas.Clear();

            UpdateLogDatas();
        }

        private void OnCollapseToggleValueChanged(bool value)
        {
            m_collapseToggleValue = value;
            UpdateLogDatas();
        }

        private void OnScrollToBottomButtonClick()
        {
            m_forceScrollToBottom = true;
            OnScrollToBottom();
        }

        private void OnSearchFilterValueChanged(string value)
        {
            m_searchFilterText = value;
            UpdateLogDatas();
        }

        private void OnLogToggleValueChanged(bool value)
        {
            m_logToggleValue = value;
            UpdateLogDatas();
        }

        private void OnWarningToggleValueChanged(bool value)
        {
            m_warningToggleValue = value;
            UpdateLogDatas();
        }

        private void OnErrorToggleValueChanged(bool value)
        {
            m_errorToggleValue = value;
            UpdateLogDatas();
        }

        private void HandleLog(string logString, string stackTrace, LogType type)
        {
            m_allLogDatas.Add(new LogData(logString, stackTrace, type));
            UpdateLogDatas();
        }

        private void UpdateLogDatas()
        {
            UpdateCollapseLogDatas();
            UpdateToggleLogDatas();
            UpdateToggleCount();
            UpdateFilterLogDatas();
            UpdateUIElements();
            OnScrollToBottom();
        }

        private void UpdateCollapseLogDatas()
        {
            m_collapseLogDatas.Clear();

            LogData cacheData = null;
            for (int i = 0; i < m_allLogDatas.Count; i++)
            {
                if(m_collapseToggleValue)
                {
                    cacheData = m_collapseLogDatas.Find((LogData obj) => obj.LogString == m_allLogDatas[i].LogString && obj.StackTrace == m_allLogDatas[i].StackTrace && obj.LogType == m_allLogDatas[i].LogType);
                    if (cacheData != null)
                    {
                        cacheData.CollapseCount++;
                    }
                    else
                    {
                        cacheData = m_allLogDatas[i];
                        cacheData.CollapseCount = 1;
                        m_collapseLogDatas.Add(cacheData);
                    }
                }
                else
                {
                    cacheData = m_allLogDatas[i];
                    cacheData.CollapseCount = 0;
                    m_collapseLogDatas.Add(cacheData);
                }
            }
        }

        private void UpdateToggleCount()
        {
            int logCount = 0;
            int warningCount = 0;
            int errorCount = 0;

            for (int i = 0; i < m_collapseLogDatas.Count; i++)
            {
                switch (m_collapseLogDatas[i].LogType)
                {
                    case LogType.Log:
                        logCount++;
                        break;
                    case LogType.Warning:
                        warningCount++;
                        break;
                    case LogType.Error:
                    case LogType.Exception:
                    case LogType.Assert:
                        errorCount++;
                        break;
                }
            }

            m_consoleToggleLog.SetCount(logCount);
            m_consoleToggleWarning.SetCount(warningCount);
            m_consoleToggleError.SetCount(errorCount);
        }

        private void UpdateToggleLogDatas()
        {
            m_toggleLogDatas.Clear();

            LogData cacheData = null;
            for (int i = 0; i < m_collapseLogDatas.Count; i++)
            {
                cacheData = m_collapseLogDatas[i];
                switch (cacheData.LogType)
                {
                    case LogType.Log:
                        if (m_logToggleValue)
                        {
                            m_toggleLogDatas.Add(cacheData);
                        }
                        break;
                    case LogType.Warning:
                        if (m_warningToggleValue)
                        {
                            m_toggleLogDatas.Add(cacheData);
                        }
                        break;
                    case LogType.Error:
                    case LogType.Exception:
                    case LogType.Assert:
                        if (m_errorToggleValue)
                        {
                            m_toggleLogDatas.Add(cacheData);
                        }
                        break;
                }
            }
        }

        private void UpdateFilterLogDatas()
        {
            m_filterLogDatas.Clear();

            if (string.IsNullOrEmpty(m_searchFilterText))
            {
                m_filterLogDatas.AddRange(m_toggleLogDatas);
            }
            else
            {
                m_filterLogDatas = m_toggleLogDatas.FindAll(templateLog => templateLog.LogString.Contains(m_searchFilterText));
            }
        }

        private void UpdateUIElements()
        {
            int filterLogDataCount = m_filterLogDatas.Count;
            int displayElementCount = m_displayElements.Count;

            if (filterLogDataCount != displayElementCount)
            {
                if(displayElementCount > filterLogDataCount)
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

                for (int i = 0; i < m_filterLogDatas.Count; i++)
                {
                    m_displayElements[i].SetLogType(m_filterLogDatas[i].LogType);
                    m_displayElements[i].SetLogString(m_filterLogDatas[i].LogString);
                    m_displayElements[i].SetStackTrace(m_filterLogDatas[i].StackTrace);
                    m_displayElements[i].SetBackgroundColor(i);
                    m_displayElements[i].SetCollapsed(m_collapseToggleValue);
                    m_displayElements[i].SetCollapsedCount(m_filterLogDatas[i].CollapseCount);
                }

                m_currentDisplayIndex = -1;
            }
            else
            {
                for (int i = 0; i < m_filterLogDatas.Count; i++)
                {
                    m_displayElements[i].SetCollapsedCount(m_filterLogDatas[i].CollapseCount);
                }
            }

            m_displayIndex = (int)(Mathf.Clamp(1 - m_logScrollRect.verticalNormalizedPosition, 0, 1) * m_filterLogDatas.Count);
            if (m_currentDisplayIndex != m_displayIndex)
            {
                m_currentDisplayIndex = m_displayIndex;
                for (int i = 0; i < m_displayElements.Count; i++)
                {
                    if (i < m_currentDisplayIndex - 15 || i > m_currentDisplayIndex + 15)
                    {
                        m_displayElements[i].SetBackgroundActive(false);
                    }
                    else
                    {
                        m_displayElements[i].SetBackgroundActive(true);
                    }
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
