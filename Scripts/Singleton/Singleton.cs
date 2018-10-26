using UnityEngine;

namespace TEDCore
{
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static object m_lock = new object();

        private static T m_instance;
        public static T Instance
        {
            get
            {
                lock (m_lock)
                {
                    if (null == m_instance)
                    {
                        string singletonName = typeof(T).Name;

                        GameObject singleton = new GameObject(string.Format("[Singleton] - {0}", singletonName));
                        GameObject.DontDestroyOnLoad(singleton);

                        m_instance = singleton.AddComponent<T>();

                        TEDDebug.LogFormat ("[Singleton] - \"{0}\" has set up.", singletonName);
                    }
                }

                return m_instance;
            }
        }


        public static bool Has()
        {
            return m_instance != null;
        }


        public static void Remove()
        {
            if (null != m_instance)
            {
                GameObject.Destroy(m_instance.gameObject);
                m_instance = null;
            }
        }
    }
}