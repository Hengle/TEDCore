using UnityEngine;
using System.Collections.Generic;

namespace TEDCore.ObjectPool
{
    public class ObjectPool : IDestroyable
    {
        private GameObject m_root;
        private GameObject m_reference;
        private Queue<GameObject> m_pool;

        public ObjectPool(string key, GameObject reference, int initialSize)
        {
            m_root = new GameObject(string.Format("[ObjectPool] - {0}", key));
            m_root.transform.parent = ObjectPoolManager.Instance.transform;
            m_reference = reference;
            m_pool = new Queue<GameObject>();

            GameObject temp;

            for (int i = 0; i < initialSize; i++)
            {
                temp = CreateNewObject();
                RecoveryObject(temp);
            }
        }

        private GameObject CreateNewObject()
        {
            return GameObject.Instantiate(m_reference);
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
                temp = CreateNewObject();
			}

            temp.transform.parent = null;

			return temp;
		}


        public void RecoveryObject(GameObject recovery)
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

        public void Destroy()
        {
            GameObject.Destroy(m_root);
        }
    }
}