using System;
using System.Collections.Generic;
using TEDCore.Utils;
using UnityEngine;

namespace TEDCore
{
    public class GameSystemManager
    {
        private static GameSystemManager _instance = null;
        public static GameSystemManager Instance
        {
            get
            {
                if(_instance == null)
                {
                    _instance = new GameSystemManager();
                }

                return _instance;
            }
        }

        private GameObject _root;
        private Dictionary<Type, MonoBehaviour> _monoBehaviours;
		private Dictionary<Type, System.Object> _objects;

        public GameSystemManager()
        {
            _root = new GameObject ("[GameSystemManager]");
			GameObject.DontDestroyOnLoad (_root);

            _monoBehaviours = new Dictionary<Type, MonoBehaviour>();
			_objects = new Dictionary<Type, System.Object>();
        }


		public static void Set<T>(System.Object entity) where T : class
        {           
			if (Has<T> ())
			{
				Debug.LogException (new Exception (string.Format ("[GameSystemManager] - GameSystem \"{0}\" has already existed!", typeof(T).Name)));
			}
			else
			{
				Debug.LogFormat (string.Format ("[GameSystemManager] - GameSystem \"{0}\" has set up.", typeof(T).Name));
				Instance._objects.Add(typeof(T), entity);
			}
        }


		public static void Set<T>() where T : MonoBehaviour
		{
			if (Has<T>())
			{
				Debug.LogException (new Exception (string.Format ("[GameSystemManager] - GameSystem \"{0}\" has already existed!", typeof(T).Name)));
			}
			else
			{
				Debug.LogFormat (string.Format ("[GameSystemManager] - GameSystem \"{0}\" has set up.", typeof(T).Name));
				Instance._monoBehaviours.Add(typeof(T), Instance._root.AddComponent<T> ());
			}
		}


		public static T Get<T>() where T : class
        {           
            if(Has<T>())
            {
				if (IsInheritMonoBehaviour<T> ())
				{
					return Instance._root.GetComponent<T> ();
				}
				else
				{
					return Instance._objects[typeof(T)] as T;
				}
            }

			Debug.LogException(new Exception(string.Format("[GameSystemManager] - GameSystemManager of {0} doesn't exist!", typeof(T).Name)));

            return null;
        }


        public static bool Has<T>() where T : class
        {
			if (IsInheritMonoBehaviour<T> ())
			{
				return Instance._monoBehaviours.ContainsKey (typeof(T));
			}
			else
			{
				return Instance._objects.ContainsKey(typeof(T));
			}
        }


		private static bool IsInheritMonoBehaviour<T>() where T : class
		{
			return typeof(T).IsSubclassOf(typeof(MonoBehaviour));
		}
    }
}

