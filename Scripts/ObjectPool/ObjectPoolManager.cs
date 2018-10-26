using UnityEngine;
using System.Collections.Generic;

namespace TEDCore.ObjectPool
{
    public class ObjectPoolManager : Singleton<ObjectPoolManager>
	{
        private Dictionary<string, ObjectPool> m_objectPools;

        private void Awake()
		{
			m_objectPools = new Dictionary<string, ObjectPool>();
		}


        public void Clear()
        {
            foreach (KeyValuePair<string, ObjectPool> kvp in m_objectPools)
            {
                kvp.Value.Destroy();
            }

            m_objectPools = new Dictionary<string, ObjectPool>();
        }


        public void RegisterPool(string key, GameObject reference, int initialSize)
		{
			if(m_objectPools.ContainsKey(key))
			{
				Debug.LogWarning(string.Format("[ObjectPoolManager] - ObjectPool \"{0}\" already exists.", key));
				return;
			}

			m_objectPools[key] = new ObjectPool(key, reference, initialSize);
		}


        public GameObject GetObject(string key)
		{
            if(!HasPool(key))
            {
                Debug.LogErrorFormat(string.Format("[ObjectPoolManager] - ObjectPool \"{0}\" doesn't exist. Please register it first.", key));
                return null;
            }

            return m_objectPools[key].GetObject();
        }


        public void RecoveryObject(string key, GameObject instance)
        {
            if (!HasPool(key))
            {
                Debug.LogErrorFormat(string.Format("[ObjectPoolManager] - ObjectPool \"{0}\" doesn't exist. Destroy the GameObject directly", key));
                GameObject.Destroy(instance);
            }

            m_objectPools[key].RecoveryObject(instance);
        }


        private bool HasPool(string key)
		{
			return m_objectPools.ContainsKey(key);
		}
	}
}