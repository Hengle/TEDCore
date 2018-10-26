using TEDCore.UnitTesting;
using TEDCore.ObjectPool;
using TEDCore.Resource;
using UnityEngine;
using System.Collections.Generic;

public class UnitTesting_ObjectPool : BaseUnitTesting
{
    private string m_poolKey;
    private Queue<GameObject> m_objects = new Queue<GameObject>();

    [TestInputField]
    public void RegisterPool(string value)
    {
        m_poolKey = value;
        ResourceSystem.Instance.LoadAsync<GameObject>(value, OnResourcePoolLoad);
    }

    private void OnResourcePoolLoad(GameObject asset)
    {
        ObjectPoolManager.Instance.RegisterPool(m_poolKey, asset, 10);
    }

    [TestButton]
    public void GetObject()
    {
        m_objects.Enqueue(ObjectPoolManager.Instance.Get(m_poolKey));
    }

    [TestButton]
    public void RecoveryObject()
    {
        if(m_objects.Count == 0)
        {
            return;
        }

        ObjectPoolManager.Instance.Recycle(m_poolKey, m_objects.Dequeue());
    }

    [TestButton]
    public void Clear()
    {
        ObjectPoolManager.Instance.Clear();
    }
}
