using UnityEngine;
using System.Collections.Generic;

namespace TEDCore.ObjectPool
{
    public abstract class ObjectPool<T> : IDestroyable where T : Object
    {
        protected GameObject m_root;
        private T m_referenceAsset;
        protected Queue<T> m_pool;

        public ObjectPool(string key, T referenceAsset, int initialSize)
        {
            if(string.IsNullOrEmpty(key))
            {
                TEDDebug.LogError("[ObjectPool] - The key is null or empty.");
                return;
            }

            if (referenceAsset == null)
            {
                TEDDebug.LogError("[ObjectPool] - The reference asset is null.");
                return;
            }

            m_root = new GameObject(string.Format("[ObjectPool ({0})] - {1}", typeof(T).Name, key));
            m_root.transform.SetParent(ObjectPoolManager.Instance.transform, false);
            m_referenceAsset = referenceAsset;
            m_pool = new Queue<T>();

            Spawn(initialSize);
        }

        protected void Spawn(int spawnSize)
        {
            if(m_pool.Count >= spawnSize)
            {
                TEDDebug.Log("[ObjectPool] - The pool size is bigger than the spawn size, don't need to create new object.");
                return;
            }

            for (int i = 0; i < spawnSize; i++)
            {
                Create();
            }
        }

        private void Create()
        {
            Recycle(Object.Instantiate(m_referenceAsset));
        }


        public abstract T Get();
        public abstract void Recycle(T recovery);

        public void Destroy()
        {
            Object.Destroy(m_root);
        }
    }
}