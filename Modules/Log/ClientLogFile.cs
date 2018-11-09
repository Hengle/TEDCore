using TEDCore.PlayerData;
using System.IO;
using UnityEngine;

namespace TEDCore.Log
{
    public class ClientLogFile
    {
        private const string DEFAULT_LINE_SEPARATOR = "------------------------------------------------------------------------------------------";
        private string m_filePath;
        private StreamWriter m_streamWriter;

        public ClientLogFile()
        {
            CreateLogFile();
        }

        public void AddLog(string logString, string stackTrace, LogType type)
        {
            m_streamWriter.WriteLine(DEFAULT_LINE_SEPARATOR);
            m_streamWriter.WriteLine(System.DateTime.Now.ToString() + "\t" + type.ToString());
            m_streamWriter.WriteLine(logString);
            m_streamWriter.WriteLine(stackTrace);
        }

        private void CreateLogFile()
        {
            m_filePath = GetFilePath();

            if (File.Exists(m_filePath))
            {
                File.Delete(m_filePath);
            }

            string logDir = Path.GetDirectoryName(m_filePath);
            if (!Directory.Exists(logDir))
            {
                Directory.CreateDirectory(logDir);
            }

            m_streamWriter = new StreamWriter(m_filePath);
            m_streamWriter.AutoFlush = true;
        }

        private string GetFilePath()
        {
            return string.Format("{0}Log_{1}.txt", GetLogPath(), System.DateTime.Now.ToString("yyyyMMddHHmmss"));
        }

        private string GetLogPath()
        {
            return PlayerDataUtils.GetPersistentDataPath() + "/LogData/";
        }
    }
}
