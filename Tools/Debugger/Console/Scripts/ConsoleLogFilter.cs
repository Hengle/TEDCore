using UnityEngine;
using System.Collections.Generic;
using System;

namespace TEDCore.Debugger.Console
{
    public class ConsoleLogFilter
    {
        public Action<List<ConsoleLogData>, int, int, int> OnUpdateFinished;

        private List<ConsoleLogData> m_allLogDatas;
        private List<ConsoleLogData> m_collapseLogDatas;
        private List<ConsoleLogData> m_toggleLogDatas;
        private List<ConsoleLogData> m_filterLogDatas;
        private bool m_collapseToggle;
        private string m_searchFilter;
        private bool m_logToggle = true;
        private bool m_warningToggle = true;
        private bool m_errorToggle = true;
        private int m_logCount;
        private int m_warningCount;
        private int m_errorCount;

        public ConsoleLogFilter()
        {
            m_allLogDatas = new List<ConsoleLogData>();
            m_collapseLogDatas = new List<ConsoleLogData>();
            m_toggleLogDatas = new List<ConsoleLogData>();
            m_filterLogDatas = new List<ConsoleLogData>();
        }

        public void Clear()
        {
            m_allLogDatas.Clear();
            UpdateLogDatas();
        }

        public void SetCollapseToggle(bool value)
        {
            m_collapseToggle = value;
            UpdateLogDatas();
        }

        public void SetSearchFilter(string value)
        {
            m_searchFilter = value;
            UpdateLogDatas();
        }

        public void SetLogToggle(bool value)
        {
            m_logToggle = value;
            UpdateLogDatas();
        }

        public void SetWarningToggle(bool value)
        {
            m_warningToggle = value;
            UpdateLogDatas();
        }

        public void SetErrorToggle(bool value)
        {
            m_errorToggle = value;
            UpdateLogDatas();
        }

        public void OnLogMessageReceived(string logString, string stackTrace, LogType type)
        {
            m_allLogDatas.Add(new ConsoleLogData(logString, stackTrace, type));
            UpdateLogDatas();
        }

        private void UpdateLogDatas()
        {
            UpdateCollapseLogDatas();
            UpdateToggleLogDatas();
            UpdateToggleCount();
            UpdateFilterLogDatas();
        }

        private void UpdateCollapseLogDatas()
        {
            m_collapseLogDatas.Clear();

            ConsoleLogData cacheData = null;
            for (int i = 0; i < m_allLogDatas.Count; i++)
            {
                if (m_collapseToggle)
                {
                    cacheData = m_collapseLogDatas.Find((ConsoleLogData obj) => obj.LogString == m_allLogDatas[i].LogString && obj.StackTrace == m_allLogDatas[i].StackTrace && obj.LogType == m_allLogDatas[i].LogType);
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

        private void UpdateToggleLogDatas()
        {
            m_toggleLogDatas.Clear();

            ConsoleLogData cacheData = null;
            for (int i = 0; i < m_collapseLogDatas.Count; i++)
            {
                cacheData = m_collapseLogDatas[i];
                switch (cacheData.LogType)
                {
                    case LogType.Log:
                        if (m_logToggle)
                        {
                            m_toggleLogDatas.Add(cacheData);
                        }
                        break;
                    case LogType.Warning:
                        if (m_warningToggle)
                        {
                            m_toggleLogDatas.Add(cacheData);
                        }
                        break;
                    case LogType.Error:
                    case LogType.Exception:
                    case LogType.Assert:
                        if (m_errorToggle)
                        {
                            m_toggleLogDatas.Add(cacheData);
                        }
                        break;
                }
            }
        }

        private void UpdateToggleCount()
        {
            m_logCount = 0;
            m_warningCount = 0;
            m_errorCount = 0;

            for (int i = 0; i < m_collapseLogDatas.Count; i++)
            {
                switch (m_collapseLogDatas[i].LogType)
                {
                    case LogType.Log:
                        m_logCount++;
                        break;
                    case LogType.Warning:
                        m_warningCount++;
                        break;
                    case LogType.Error:
                    case LogType.Exception:
                    case LogType.Assert:
                        m_errorCount++;
                        break;
                }
            }
        }

        private void UpdateFilterLogDatas()
        {
            m_filterLogDatas.Clear();

            if (string.IsNullOrEmpty(m_searchFilter))
            {
                m_filterLogDatas.AddRange(m_toggleLogDatas);
            }
            else
            {
                m_filterLogDatas = m_toggleLogDatas.FindAll(templateLog => templateLog.LogString.Contains(m_searchFilter));
            }

            if(OnUpdateFinished != null)
            {
                OnUpdateFinished(m_filterLogDatas, m_logCount, m_warningCount, m_errorCount);
            }
        }
    }
}