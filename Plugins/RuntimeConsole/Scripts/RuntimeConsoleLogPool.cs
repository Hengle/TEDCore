using System.Collections.Generic;
using UnityEngine;

public class RuntimeConsoleLogPool : MonoBehaviour
{
    [SerializeField] private RuntimeConsoleLog m_reference;
    [SerializeField] private Transform m_scrollContent;
    private Queue<RuntimeConsoleLog> m_pools;

    private void Awake()
    {
        m_pools = new Queue<RuntimeConsoleLog>();
    }

    private void CreateLog()
    {
        Recovery(Instantiate(m_reference));
    }

    public void Recovery(RuntimeConsoleLog templateLog)
    {
        templateLog.transform.SetParent(transform, false);
        templateLog.gameObject.SetActive(false);
        m_pools.Enqueue(templateLog);
    }

    public RuntimeConsoleLog Get()
    {
        if (m_pools.Count == 0)
        {
            CreateLog();
        }

        RuntimeConsoleLog templateLog = m_pools.Dequeue();
        templateLog.transform.SetParent(m_scrollContent, false);
        templateLog.transform.localScale = Vector3.one;
        templateLog.transform.localEulerAngles = Vector3.zero;
        templateLog.gameObject.SetActive(true);

        return templateLog;
    }
}
