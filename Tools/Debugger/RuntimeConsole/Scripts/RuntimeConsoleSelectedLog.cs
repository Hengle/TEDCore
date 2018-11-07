using UnityEngine;
using UnityEngine.UI;

public class RuntimeConsoleSelectedLog : MonoBehaviour
{
    [SerializeField] private Text m_stackTraceText;

    public void SetStackTrace(string text)
    {
        m_stackTraceText.text = text;
    }
}
