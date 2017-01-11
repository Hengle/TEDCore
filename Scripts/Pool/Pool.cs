using UnityEngine;
using System.Collections.Generic;
using TEDCore;
using TEDCore.Resource;

namespace TEDCore.Pool
{
	public class Pool
	{
		private GameObject m_root;
		private readonly string m_path;
		private Queue<GameObject> m_pool;

		public Pool(string path, int initialSize)
		{
			m_root = new GameObject(string.Format("[Pool] {0}", path));
			m_path = path;
			m_pool = new Queue<GameObject>();

			GameObject temp;

			for(int cnt = 0; cnt < initialSize; cnt++)
			{
				temp = MonoBehaviourManager.Get<ResourceManager>().CheckOutAndInstantiate(m_path, true);
				ResetObject(temp);

				m_pool.Enqueue(temp);
			}

			MonoBehaviourManager.Get<ResourceManager>().CheckIn(m_path);
		}


		public GameObject GetObject()
		{
			GameObject temp;

			if(m_pool.Count > 0)
			{
				temp = m_pool.Dequeue();
				temp.SetActive(true);
			}
			else
			{
				temp = MonoBehaviourManager.Get<ResourceManager>().CheckOutAndInstantiate(m_path, true);
				MonoBehaviourManager.Get<ResourceManager>().CheckIn(m_path);
			}

			return temp;
		}


		public void PutObject(GameObject recovery)
		{
			ResetObject(recovery);
			m_pool.Enqueue(recovery);
		}


		private void ResetObject(GameObject resetObject)
		{
			resetObject.transform.parent = m_root.transform;
			resetObject.transform.localPosition = Vector3.zero;
			resetObject.SetActive(false);
		}
	}
}