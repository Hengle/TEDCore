using UnityEngine;
using System.Threading;

namespace TEDCore.Log
{
    public class LogManager : MonoSingleton<LogManager>
    {
        private int m_threadId;
        private ClientLogFile m_clientLogFile;

        private void Awake()
        {
            m_threadId = Thread.CurrentThread.ManagedThreadId;
        }

        public void StartRecording()
        {
            if (m_clientLogFile == null)
            {
                m_clientLogFile = new ClientLogFile();
                Application.logMessageReceived += OnLogMessageReceived;
                Application.logMessageReceivedThreaded += OnLogMessageReceivedThreaded;
            }
        }

        private void OnLogMessageReceived(string logString, string stackTrace, LogType type)
        {
            if(m_threadId != Thread.CurrentThread.ManagedThreadId)
            {
                return;
            }

            m_clientLogFile.AddLog(logString, stackTrace, type);
        }

        private void OnLogMessageReceivedThreaded(string logString, string stackTrace, LogType type)
        {
            if (m_threadId == Thread.CurrentThread.ManagedThreadId)
            {
                return;
            }

            m_clientLogFile.AddLog(logString, stackTrace, type);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            m_clientLogFile = null;
            Application.logMessageReceived -= OnLogMessageReceived;
            Application.logMessageReceived -= OnLogMessageReceived;
        }
    }
}
