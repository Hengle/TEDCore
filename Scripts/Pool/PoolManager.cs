using UnityEngine;
using System.Collections.Generic;
using TEDCore.Utils;

namespace TEDCore.Pool
{
	public class PoolManager
	{
		private Dictionary<string, Pool> m_pools;

		public PoolManager()
		{
			m_pools = new Dictionary<string, Pool>();
		}


		public void RegisterPool(string poolName, int initialSize)
		{
			if(m_pools.ContainsKey(poolName))
			{
				Debugger.LogWarning(string.Format("[PoolManager] - Pool \"{0}\" already exists.", poolName));
				return;
			}

			m_pools[poolName] = new Pool(poolName, initialSize);
		}


		public Pool GetPool(string poolName)
		{
			if(m_pools.ContainsKey(poolName))
			{
				return m_pools[poolName];
			}
			else
			{
				Debugger.LogWarning(string.Format("[PoolManager] - Pool \"{0}\" doesn't exist. Register a new \"{1}\" pool with initial size 1.", poolName, poolName));
				RegisterPool(poolName, 1);
				return m_pools[poolName];
			}
		}


		public bool ContainPool(string poolName)
		{
			return m_pools.ContainsKey(poolName);
		}
	}
}