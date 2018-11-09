using UnityEngine;
using System.Collections.Generic;
using System;
using System.Threading;

namespace TEDCore.Debugger.Console
{
    public class ConsoleLogFilter
    {
        public Action<List<ConsoleLogData>, int, int, int> OnUpdateFinished;

        private int m_mainThreadId;
        private List<ConsoleLogData> m_allLogDatas;
        private List<ConsoleLogData> m_collapseLogDatas;
        private List<ConsoleLogData> m_toggleLogDatas;
        private List<ConsoleLogData> m_filterLogDatas;
        private bool m_collapseToggle;
        private string m_searchFilter;
        private bool m_logToggle = true;
        private bool m_warningToggle = true;
        private bool m_errorToggle = true;

        private int m_allLogCount;
        private int m_allWarningCount;
        private int m_allErrorCount;
        private int m_collapsedLogCount;
        private int m_collapsedWarningCount;
        private int m_collapsedErrorCount;

        public ConsoleLogFilter()
        {
            m_mainThreadId = Thread.CurrentThread.ManagedThreadId;

            m_allLogDatas = new List<ConsoleLogData>();
            m_collapseLogDatas = new List<ConsoleLogData>();
            m_toggleLogDatas = new List<ConsoleLogData>();
            m_filterLogDatas = new List<ConsoleLogData>();
        }

        public void Clear()
        {
            m_allLogDatas.Clear();
            m_collapseLogDatas.Clear();
            m_allLogCount = 0;
            m_allWarningCount = 0;
            m_allErrorCount = 0;
            m_collapsedLogCount = 0;
            m_collapsedWarningCount = 0;
            m_collapsedErrorCount = 0;

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
            if(m_mainThreadId != Thread.CurrentThread.ManagedThreadId)
            {
                return;
            }

            UpdateLogDatas(new ConsoleLogData(logString, stackTrace, type));
        }

        public void LogMessageReceivedThreaded(string logString, string stackTrace, LogType type)
        {
            if (m_mainThreadId == Thread.CurrentThread.ManagedThreadId)
            {
                return;
            }

            UpdateLogDatas(new ConsoleLogData(logString, stackTrace, type));
        }

        private void UpdateLogDatas(ConsoleLogData consoleLogData = null)
        {
            UpdateAllLogDatas(consoleLogData);
            UpdateCollapseLogDatas(consoleLogData);
            UpdateToggleLogDatas();
            UpdateFilterLogDatas();
        }

        private void UpdateAllLogDatas(ConsoleLogData consoleLogData)
        {
            if(consoleLogData == null)
            {
                return;
            }

            m_allLogDatas.Add(consoleLogData);

            switch(consoleLogData.LogType)
            {
                case LogType.Log:
                    m_allLogCount++;
                    break;
                case LogType.Warning:
                    m_allWarningCount++;
                    break;
                case LogType.Error:
                case LogType.Exception:
                case LogType.Assert:
                    m_allErrorCount++;
                    break;
            }
        }

        private void UpdateCollapseLogDatas(ConsoleLogData consoleLogData)
        {
            if (consoleLogData == null)
            {
                return;
            }

            ConsoleLogData cacheData = m_collapseLogDatas.Find((ConsoleLogData obj) => obj.LogString == consoleLogData.LogString && obj.StackTrace == consoleLogData.StackTrace && obj.LogType == consoleLogData.LogType);
            if (cacheData != null)
            {
                cacheData.CollapseCount++;
            }
            else
            {
                cacheData = consoleLogData;
                cacheData.CollapseCount = 1;
                m_collapseLogDatas.Add(cacheData);

                switch (consoleLogData.LogType)
                {
                    case LogType.Log:
                        m_collapsedLogCount++;
                        break;
                    case LogType.Warning:
                        m_collapsedWarningCount++;
                        break;
                    case LogType.Error:
                    case LogType.Exception:
                    case LogType.Assert:
                        m_collapsedErrorCount++;
                        break;
                }
            }
        }

        private void UpdateToggleLogDatas()
        {
            List<ConsoleLogData> consoleLogDatas = new List<ConsoleLogData>();
            if(m_collapseToggle)
            {
                consoleLogDatas.AddRange(m_collapseLogDatas);
            }
            else
            {
                consoleLogDatas.AddRange(m_allLogDatas);
            }

            m_toggleLogDatas.Clear();

            ConsoleLogData cacheData = null;
            for (int i = 0; i < consoleLogDatas.Count; i++)
            {
                cacheData = consoleLogDatas[i];
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
                if(m_collapseToggle)
                {
                    OnUpdateFinished(m_filterLogDatas, m_collapsedLogCount, m_collapsedWarningCount, m_collapsedErrorCount);
                }
                else
                {
                    OnUpdateFinished(m_filterLogDatas, m_allLogCount, m_allWarningCount, m_allErrorCount);
                }
            }
        }
    }
}