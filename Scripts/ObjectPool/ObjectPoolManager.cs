using UnityEngine;
using System.Collections.Generic;

namespace TEDCore.ObjectPool
{
    public class ObjectPoolManager : MonoSingleton<ObjectPoolManager>
	{
        public int DEFAULT_SPAWN_SIZE = 5;
        private Dictionary<string, GameObjectPool> m_gameObjectPools;

        private void Awake()
		{
			m_gameObjectPools = new Dictionary<string, GameObjectPool>();
		}


        public void Clear()
        {
            foreach (KeyValuePair<string, GameObjectPool> kvp in m_gameObjectPools)
            {
                kvp.Value.Destroy();
            }

            m_gameObjectPools.Clear();
        }


        public void RegisterPool(string key, GameObject referenceAsset, int initialSize)
		{
			if(m_gameObjectPools.ContainsKey(key))
			{
				return;
			}

            if (string.IsNullOrEmpty(key))
            {
                TEDDebug.LogError("[ObjectPoolManager] - The key is null or empty, register fail.");
                return;
            }

            if (referenceAsset == null)
            {
                TEDDebug.LogError("[ObjectPoolManager] - The reference asset is null, register fail.");
                return;
            }

            m_gameObjectPools[key] = new GameObjectPool(key, referenceAsset, initialSize);
		}


        public GameObject Get(string key)
		{
            if(!HasPool(key))
            {
                TEDDebug.LogErrorFormat(string.Format("[ObjectPoolManager] - ObjectPool \"{0}\" doesn't exist. Please register it first.", key));
                return null;
            }

            return m_gameObjectPools[key].Get();
        }


        public void Recycle(string key, GameObject instance)
        {
            if (!HasPool(key))
            {
                TEDDebug.LogErrorFormat(string.Format("[ObjectPoolManager] - ObjectPool \"{0}\" doesn't exist. Destroy the GameObject directly", key));
                GameObject.Destroy(instance);
            }

            m_gameObjectPools[key].Recycle(instance);
        }


        private bool HasPool(string key)
		{
			return m_gameObjectPools.ContainsKey(key);
		}
	}
}