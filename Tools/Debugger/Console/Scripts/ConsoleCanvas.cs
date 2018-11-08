using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace TEDCore.Debugger.Console
{
    public class ConsoleCanvas : MonoBehaviour
    {
        [SerializeField] private Button m_clearButton;
        [SerializeField] private Button m_scrollToBottomButton;
        [SerializeField] private InputField m_searchFilter;
        [SerializeField] private ConsoleToggle m_consoleToggleLog;
        [SerializeField] private ConsoleToggle m_consoleToggleWarning;
        [SerializeField] private ConsoleToggle m_consoleToggleError;

        [SerializeField] private ConsoleLogPool m_consoleLogPool;

        [SerializeField] private ScrollRect m_logScrollRect;

        private List<ConsoleLog> m_allLogs;
        private List<ConsoleLog> m_searchFilterLogs;
        private List<ConsoleLog> m_displayLogs;
        private string m_searchFilterText;
        private bool m_logToggleValue = true;
        private bool m_warningToggleValue = true;
        private bool m_errorToggleValue = true;
        private bool m_forceScrollToBottom = true;

        private void Awake()
        {
            m_allLogs = new List<ConsoleLog>();
            m_searchFilterLogs = new List<ConsoleLog>();
            m_displayLogs = new List<ConsoleLog>();

            m_clearButton.onClick.AddListener(OnClearButtonClick);
            m_scrollToBottomButton.onClick.AddListener(OnScrollToBottomButtonClick);
            m_searchFilter.onValueChanged.AddListener(OnSearchFilterValueChanged);
            m_consoleToggleLog.SetToggleValueChanged(OnLogToggleValueChanged);
            m_consoleToggleWarning.SetToggleValueChanged(OnWarningToggleValueChanged);
            m_consoleToggleError.SetToggleValueChanged(OnErrorToggleValueChanged);

            UpdateCount();

            Application.logMessageReceived += HandleLog;
        }

        private void Update()
        {
            m_forceScrollToBottom = m_logScrollRect.verticalNormalizedPosition < 0.1f;
            int index = (int)(Mathf.Clamp(1 - m_logScrollRect.verticalNormalizedPosition, 0, 1) * m_displayLogs.Count);

            for (int i = 0; i < m_displayLogs.Count; i++)
            {
                m_displayLogs[i].SetBackgroundActive(i >= index - 15 && i <= index + 15);
            }
        }

        private void OnClearButtonClick()
        {
            foreach (ConsoleLog log in m_allLogs)
            {
                m_consoleLogPool.Recovery(log);
            }

            m_allLogs.Clear();
            UpdateCount();
        }

        private void OnScrollToBottomButtonClick()
        {
            m_forceScrollToBottom = true;
            OnScrollToBottom();
        }

        private void OnSearchFilterValueChanged(string value)
        {
            m_searchFilterText = value;
            UpdateLogs();
        }

        private void OnLogToggleValueChanged(bool value)
        {
            m_logToggleValue = value;
            UpdateLogs();
        }

        private void OnWarningToggleValueChanged(bool value)
        {
            m_warningToggleValue = value;
            UpdateLogs();
        }

        private void OnErrorToggleValueChanged(bool value)
        {
            m_errorToggleValue = value;
            UpdateLogs();
        }

        private void HandleLog(string logString, string stackTrace, LogType type)
        {
            ConsoleLog templateLog = m_consoleLogPool.Get();
            templateLog.SetLogType(type);
            templateLog.SetLogString(logString);
            templateLog.SetStackTrace(stackTrace);

            m_allLogs.Add(templateLog);

            UpdateLogs();
            UpdateCount();
        }

        private void UpdateLogs()
        {
            UpdateSearchFilterLogs();
            UpdateDisplayLogs();
            UpdateBackgroundColors();
            OnScrollToBottom();
        }

        private void UpdateSearchFilterLogs()
        {
            for (int i = 0; i < m_allLogs.Count; i++)
            {
                m_allLogs[i].gameObject.SetActive(false);
            }

            m_searchFilterLogs.Clear();

            if (string.IsNullOrEmpty(m_searchFilterText))
            {
                m_searchFilterLogs.AddRange(m_allLogs);
            }
            else
            {
                m_searchFilterLogs = m_allLogs.FindAll(templateLog => templateLog.GetLogString().Contains(m_searchFilterText));
            }
        }

        private void UpdateDisplayLogs()
        {
            m_displayLogs.Clear();

            for (int i = 0; i < m_searchFilterLogs.Count; i++)
            {
                switch (m_searchFilterLogs[i].GetLogType())
                {
                    case LogType.Log:
                        if (m_logToggleValue)
                        {
                            m_displayLogs.Add(m_searchFilterLogs[i]);
                        }
                        break;
                    case LogType.Warning:
                        if (m_warningToggleValue)
                        {
                            m_displayLogs.Add(m_searchFilterLogs[i]);
                        }
                        break;
                    case LogType.Error:
                    case LogType.Exception:
                    case LogType.Assert:
                        if (m_errorToggleValue)
                        {
                            m_displayLogs.Add(m_searchFilterLogs[i]);
                        }
                        break;
                }
            }

            for (int i = 0; i < m_displayLogs.Count; i++)
            {
                m_displayLogs[i].gameObject.SetActive(true);
            }
        }

        private void UpdateBackgroundColors()
        {
            int index = -1;

            for (int i = 0; i < m_displayLogs.Count; i++)
            {
                index++;
                m_displayLogs[i].SetBackgroundColor(index);
            }
        }

        private void UpdateCount()
        {
            int logCount = 0;
            int warningCount = 0;
            int errorCount = 0;

            for (int i = 0; i < m_allLogs.Count; i++)
            {
                switch (m_allLogs[i].GetLogType())
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
