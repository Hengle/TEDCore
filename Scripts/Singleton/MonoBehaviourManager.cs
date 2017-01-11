using System;
using System.Collections.Generic;
using UnityEngine;
using TEDCore.Utils;

namespace TEDCore
{
    public class MonoBehaviourManager
    {
        private static MonoBehaviourManager _instance = null;
        public static MonoBehaviourManager Instance
        {
            get
            {
                if(_instance == null)
                {
                    _instance = new MonoBehaviourManager();
                }

                return _instance;
            }
        }

		private GameObject _root;
        private Dictionary<Type, MonoBehaviour> _monoBehaviours;

		public MonoBehaviourManager()
		{
			_root = new GameObject ("[MonoBehavourManager]");
			_monoBehaviours = new Dictionary<Type, MonoBehaviour>();
		}


		public static void Set<T>() where T : MonoBehaviour
		{
			if (Has<T>())
			{
				Debugger.LogException (new Exception (string.Format ("[MonoBehaviourManager] - MonoBehaviour of {0} is already exist!", typeof(T).Name)));
			}
			else
			{
				Instance._monoBehaviours [typeof(T)] = Instance._root.AddComponent<T> ();
			}
		}


		public static T Get<T>() where T : MonoBehaviour
		{
			if(Has<T>())
			{
				return Instance._monoBehaviours [typeof(T)] as T;
			}

			Debugger.LogException(new Exception(string.Format("[MonoBehaviourManager] - MonoBehaviour of {0} doesn't exist!", typeof(T).Name)));

			return null;
		}


		public static bool Has<T>() where T : MonoBehaviour
		{
			return Instance._monoBehaviours.ContainsKey(typeof(T));
		}
    }
}