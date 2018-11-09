using UnityEngine;

namespace TEDCore.Debugger.Console
{
    public class ConsoleLogData
    {
        public string LogString;
        public string StackTrace;
        public LogType LogType;
        public int CollapseCount;

        public ConsoleLogData(string logString, string stackTrace, LogType logType)
        {
            LogString = logString;
            StackTrace = stackTrace;
            LogType = logType;
            CollapseCount = 0;
        }
    }
}