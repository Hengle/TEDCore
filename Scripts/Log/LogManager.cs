﻿using UnityEngine;
using System.IO;

namespace TEDCore.Log
{
    public class LogManager : MonoSingleton<LogManager>
    {
        private const string DEFAULT_LINE_SEPARATOR = "------------------------------------------------------------------------------------------";
        private StreamWriter m_streamWriter;

        public void StartRecording()
        {
            CreateFile();
            Application.logMessageReceived += HandleLog;
        }


        private void CreateFile()
        {
            string logPath = GetLogPath();

            if (File.Exists(logPath))
            {
                File.Delete(logPath);
            }

            string logDir = Path.GetDirectoryName(logPath);
            if (!Directory.Exists(logDir))
            {
                Directory.CreateDirectory(logDir);
            }

            m_streamWriter = new StreamWriter(logPath);
            m_streamWriter.AutoFlush = true;
        }


        private string GetLogPath()
        {
            System.DateTime now = System.DateTime.Now;
            string logName = System.DateTime.Now.ToString("yyyyMMddHHmmss");
            return string.Format("{0}/Logs/{1}.txt", GetLogFolderPath(), logName);
        }


        private string GetLogFolderPath()
        {
#if UNITY_EDITOR
            return Application.dataPath.Replace("Assets", "");
#else
            return Application.persistentDataPath;
#endif
        }


        private void HandleLog(string logString, string stackTrace, LogType type)
        {
            if (type == LogType.Log || type == LogType.Warning)
            {
                return;
            }

            m_streamWriter.WriteLine(DEFAULT_LINE_SEPARATOR);
            m_streamWriter.WriteLine(System.DateTime.Now.ToString() + "\t" + logString);
            m_streamWriter.WriteLine(stackTrace);
        }


        protected override void OnDestroy()
        {
            base.OnDestroy();
            Application.logMessageReceived -= HandleLog;
        }
    }
}