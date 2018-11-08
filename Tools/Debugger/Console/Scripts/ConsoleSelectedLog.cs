using UnityEngine;
using UnityEngine.UI;

namespace TEDCore.Debugger.Console
{
    public class ConsoleSelectedLog : MonoBehaviour
    {
        [SerializeField] private Text m_stackTraceText;

        public void SetStackTrace(string text)
        {
            m_stackTraceText.text = text;
        }
    }
}
