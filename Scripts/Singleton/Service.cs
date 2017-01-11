using System;
using System.Collections.Generic;
using TEDCore.Utils;

namespace TEDCore
{
	public class Services
	{
		private static Services _instance = null;
		public static Services Instance
		{
			get
			{
				if(_instance == null)
				{
					_instance = new Services();
				}
				
				return _instance;
			}
		}

		private Dictionary<Type, Object> _services;
		
		
		public Services()
		{
			_services = new Dictionary<Type, Object>();
		}
		
		
        public static T Set<T>(T singleton) where T : class
		{			
			if(Has<T>())
			{
				Debugger.LogException(new Exception(string.Format("[Services] - Services of {0} is already exist!", typeof(T).Name)));
                return (T)Instance._services[typeof(T)];
			}
			
            Instance._services [typeof(T)] = singleton;
			
			return singleton;
		}
		
		
		public static T Get<T>() where T : class
		{			
			if(Has<T>())
			{
                return (T)Instance._services[typeof(T)];
			}
			
			Debugger.LogException(new Exception(string.Format("[Services] - Services of {0} doesn't exist!", typeof(T).Name)));
			
			return null;
		}
		
		
		public static bool Has<T>() where T : class
		{
            return Instance._services.ContainsKey(typeof(T));
		}
	}
}

