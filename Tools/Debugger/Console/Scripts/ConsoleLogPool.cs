using System.Collections.Generic;
using UnityEngine;

namespace TEDCore.Debugger.Console
{
    public class ConsoleLogPool : MonoBehaviour
    {
        [SerializeField] private ConsoleLog m_templateConsoleLog;
        [SerializeField] private Transform m_scrollContent;
        private Queue<ConsoleLog> m_consoleLogPool;

        private void Awake()
        {
            m_consoleLogPool = new Queue<ConsoleLog>();
        }

        private void CreateConsoleLog()
        {
            Recovery(Instantiate(m_templateConsoleLog));
        }

        public void Recovery(ConsoleLog templateLog)
        {
            templateLog.transform.SetParent(transform, false);
            m_consoleLogPool.Enqueue(templateLog);
        }

        public ConsoleLog Get()
        {
            if (m_consoleLogPool.Count == 0)
            {
                CreateConsoleLog();
            }

            ConsoleLog templateLog = m_consoleLogPool.Dequeue();
            templateLog.transform.SetParent(m_scrollContent, false);
            templateLog.transform.localScale = Vector3.one;
            templateLog.transform.localEulerAngles = Vector3.zero;

            return templateLog;
        }
    }
}
