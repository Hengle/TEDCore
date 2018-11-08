using System.Collections.Generic;
using UnityEngine;

namespace TEDCore.Debugger.Console
{
    public class ConsoleLogPool : MonoBehaviour
    {
        [SerializeField] private ConsoleLog m_templateConsoleLog;
        [SerializeField] private Transform m_scrollContent;
        private Queue<ConsoleLog> m_pools;

        private void Awake()
        {
            m_pools = new Queue<ConsoleLog>();
        }

        private void CreateLog()
        {
            Recovery(Instantiate(m_templateConsoleLog));
        }

        public void Recovery(ConsoleLog templateLog)
        {
            templateLog.transform.SetParent(transform, false);
            templateLog.gameObject.SetActive(false);
            m_pools.Enqueue(templateLog);
        }

        public ConsoleLog Get()
        {
            if (m_pools.Count == 0)
            {
                CreateLog();
            }

            ConsoleLog templateLog = m_pools.Dequeue();
            templateLog.transform.SetParent(m_scrollContent, false);
            templateLog.transform.localScale = Vector3.one;
            templateLog.transform.localEulerAngles = Vector3.zero;
            templateLog.gameObject.SetActive(true);

            return templateLog;
        }
    }
}
