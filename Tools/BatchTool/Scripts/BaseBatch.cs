using UnityEngine;

public abstract class BaseBatch : IBatch
{
    protected Transform m_root;
    public BaseBatch(Transform root)
    {
        m_root = root;
    }

    public abstract string GetTypeName();
    public abstract string GetBatchKey();
}