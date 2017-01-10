using System;
using System.Collections.Generic;
using TEDCore.Utils;

namespace TEDCore.Event
{
	public class EventManager
	{
		private class ListenerContainer
		{
			public IEventListener Listener { get; private set; }
			public int Priority { get; private set; }

			public ListenerContainer(IEventListener listener, int priority)
			{
				Listener = listener;
				Priority = priority;
			}
		}


		private Dictionary<string, List<ListenerContainer>> m_eventListeners;


		public EventManager()
		{
			m_eventListeners = new Dictionary<string, List<ListenerContainer>>();
		}


		public void RegisterListener(string eventName, IEventListener listener, int priority = 0)
		{
			if(!m_eventListeners.ContainsKey(eventName))
			{
				m_eventListeners[eventName] = new List<ListenerContainer>();
			}

			List<ListenerContainer> listeners = m_eventListeners[eventName];

			foreach(ListenerContainer lc in listeners)
			{
				if(lc.Listener == listener)
				{
					Debugger.LogException(new Exception("[EventManager] - Listener is already registered for this object."));
					return;
				}
			}

			listeners.Add(new ListenerContainer(listener, priority));
			Comparison<ListenerContainer> comparison = delegate(ListenerContainer x, ListenerContainer y) {
				return y.Priority.CompareTo(x.Priority);
			};
			listeners.Sort(comparison);
		}


		public void UnregisterListener(string eventName, IEventListener listener)
		{
			if(m_eventListeners.ContainsKey(eventName))
			{
				ListenerContainer tempListener;

				for(int cnt = 0; cnt < m_eventListeners[eventName].Count; cnt++)
				{
					tempListener = m_eventListeners[eventName][cnt];

					if(tempListener.Listener == listener)
					{
						m_eventListeners[eventName].Remove(tempListener);

					}
				}
			}
		}


		public EventResult SendEvent(string eventName, object data = null)
		{
			if(m_eventListeners.ContainsKey(eventName))
			{
				EventResult result;

				for(int cnt = 0; cnt < m_eventListeners[eventName].Count; cnt++)
				{
					result = m_eventListeners[eventName][cnt].Listener.OnEvent(eventName, data);

					if(result == null)
						continue;

					if(result.WasEaten)
						return result;
				}
			}

			return null;
		}
	}
}