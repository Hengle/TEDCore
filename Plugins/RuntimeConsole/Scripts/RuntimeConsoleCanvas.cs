using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class RuntimeConsoleCanvas : MonoBehaviour
{
    [SerializeField] private Button m_clearButton;
    [SerializeField] private Button m_scrollToBottomButton;
    [SerializeField] private InputField m_searchFilter;
    [SerializeField] private RuntimeConsoleToggle m_logToggle;
    [SerializeField] private RuntimeConsoleToggle m_warningToggle;
    [SerializeField] private RuntimeConsoleToggle m_errorToggle;

    [SerializeField] private RuntimeConsoleLogPool m_runtimeConsolePool;

    [SerializeField] private ScrollRect m_logScrollRect;

    private List<RuntimeConsoleLog> m_logs;
    private List<RuntimeConsoleLog> m_searchFilterLogs;
    private List<RuntimeConsoleLog> m_displayLogs;
    private string m_searchFilterText;
    private bool m_logToggleValue = true;
    private bool m_warningToggleValue = true;
    private bool m_errorToggleValue = true;
    private bool m_forceScrollToBottom = true;

    private void Awake()
    {
        m_logs = new List<RuntimeConsoleLog>();
        m_searchFilterLogs = new List<RuntimeConsoleLog>();
        m_displayLogs = new List<RuntimeConsoleLog>();

        m_clearButton.onClick.AddListener(OnClearButtonClick);
        m_scrollToBottomButton.onClick.AddListener(OnScrollToBottomButtonClick);
        m_searchFilter.onValueChanged.AddListener(OnSearchFilterValueChanged);
        m_logToggle.SetToggleValueChanged(OnLogToggleValueChanged);
        m_warningToggle.SetToggleValueChanged(OnWarningToggleValueChanged);
        m_errorToggle.SetToggleValueChanged(OnErrorToggleValueChanged);

        UpdateCount();

        Application.logMessageReceived += HandleLog;
    }

    private void Update()
    {
        m_forceScrollToBottom = m_logScrollRect.verticalNormalizedPosition < 0.1f;
        int index = (int)((1 - m_logScrollRect.verticalNormalizedPosition) * m_displayLogs.Count);

        for (int i = 0; i < m_displayLogs.Count; i++)
        {
            m_displayLogs[i].SetBackgroundActive(i >= index - 15 && i <= index + 15);
        }
    }

    private void OnClearButtonClick()
    {
        foreach (RuntimeConsoleLog log in m_logs)
        {
            m_runtimeConsolePool.Recovery(log);
        }

        m_logs.Clear();
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
        RuntimeConsoleLog templateLog = m_runtimeConsolePool.Get();
        templateLog.SetLogType(type);
        templateLog.SetLogString(logString);
        templateLog.SetStackTrace(stackTrace);

        m_logs.Add(templateLog);

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
        for (int i = 0; i < m_logs.Count; i++)
        {
            m_logs[i].gameObject.SetActive(false);
        }

        m_searchFilterLogs.Clear();

        if (string.IsNullOrEmpty(m_searchFilterText))
        {
            m_searchFilterLogs.AddRange(m_logs);
        }
        else
        {
            m_searchFilterLogs = m_logs.FindAll(templateLog => templateLog.GetLogString().Contains(m_searchFilterText));
        }
    }

    private void UpdateDisplayLogs()
    {
        m_displayLogs.Clear();

        for (int i = 0; i < m_searchFilterLogs.Count; i++)
        {
            switch(m_searchFilterLogs[i].GetLogType())
            {
                case LogType.Log:
                    if(m_logToggleValue)
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

        for (int i = 0; i < m_logs.Count; i++)
        {
            switch(m_logs[i].GetLogType())
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

        m_logToggle.SetCount(logCount);
        m_warningToggle.SetCount(warningCount);
        m_errorToggle.SetCount(errorCount);
    }

    private void OnScrollToBottom()
    {
        if(!m_forceScrollToBottom)
        {
            return;
        }

        Canvas.ForceUpdateCanvases();
        m_logScrollRect.verticalNormalizedPosition = 0;
    }
}
