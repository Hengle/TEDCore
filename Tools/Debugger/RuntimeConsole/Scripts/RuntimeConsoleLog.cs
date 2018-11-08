﻿using UnityEngine;
using UnityEngine.UI;

public class RuntimeConsoleLog : MonoBehaviour
{
    [SerializeField] private Color[] m_backgroundColors =
    {
        new Color(200f / 256, 200f / 256, 200f / 256, 1f),
        new Color(175f / 256, 175f / 256, 175f / 256, 1f)
    };
    [SerializeField] private Sprite[] m_logTypeSprites;
    [SerializeField] private RuntimeConsoleSelectedLog m_selectedLog;

    [SerializeField] private Image m_backgroundImage;
    [SerializeField] private Image m_logTypeImage;
    [SerializeField] private Text m_logStringText;
    [SerializeField] private Text m_stackTraceText;
    private int m_index;
    private LogType m_logType;
    private string m_logString;
    private string m_stackTrace;

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

    public void SetStackTrace(string text)
    {
        m_stackTrace = text;
        m_stackTraceText.text = m_stackTrace.Split(new char[] { '\n' }, System.StringSplitOptions.RemoveEmptyEntries)[0];
    }

    private void OnClick()
    {
        m_selectedLog.SetStackTrace(string.Format("{0}\n{1}", m_logString, m_stackTrace));
    }
}