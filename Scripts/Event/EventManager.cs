using System;
using System.Collections.Generic;

namespace TEDCore.Event
{
    public class EventManager : Singleton<EventManager>
	{
        private class ListenerContainer : IComparable<ListenerContainer>
		{
			public IEventListener Listener { get; private set; }
			public int Priority { get; private set; }

			public ListenerContainer(IEventListener listener, int priority)
			{
				Listener = listener;
				Priority = priority;
			}


            public int CompareTo(ListenerContainer listener)
            {
                if (null == listener)
                {
                    return 0;
                }
                else
                {
                    return listener.Priority.CompareTo(this.Priority);
                }
            }
		}

        private Dictionary<int, List<ListenerContainer>> m_eventListeners;
        private Dictionary<int, ListenerContainer[]> m_eventListenerArrays;

        public EventManager()
		{
            m_eventListeners = new Dictionary<int, List<ListenerContainer>>();
            m_eventListenerArrays = new Dictionary<int, ListenerContainer[]>();
		}


        public void RegisterListener(int eventName, IEventListener listener, int priority = 0)
		{
			if(!m_eventListeners.ContainsKey(eventName))
			{
				m_eventListeners[eventName] = new List<ListenerContainer>();
			}

			List<ListenerContainer> listeners = m_eventListeners[eventName];
            int listenerCount = listeners.Count;
            for (int i = 0; i < listenerCount; i++)
            {
                if (listeners[i].Listener == listener)
                {
                    TEDDebug.LogException(new Exception("[EventManager] - Listener is already registered for this object."));
                    return;
                }
            }

			listeners.Add(new ListenerContainer(listener, priority));
            listeners.Sort();

            m_eventListeners[eventName] = listeners;

            if (m_eventListenerArrays.ContainsKey(eventName))
            {
                m_eventListenerArrays[eventName] = listeners.ToArray();
            }
            else
            {
                m_eventListenerArrays.Add(eventName, listeners.ToArray());
            }
		}


        public void RemoveListener(int eventName, IEventListener listener)
		{
			if(m_eventListeners.ContainsKey(eventName))
			{
				ListenerContainer tempListener;
                int listenerCount = m_eventListeners[eventName].Count;

                List<ListenerContainer> removeListeners = new List<ListenerContainer>();

                for(int i = 0; i < listenerCount; i++)
				{
                    tempListener = m_eventListeners[eventName][i];

					if(tempListener.Listener == listener)
					{
                        removeListeners.Add(tempListener);
					}
				}

                listenerCount = removeListeners.Count;
                for (int i = 0; i < listenerCount; i++)
                {
                    m_eventListeners[eventName].Remove(removeListeners[i]);
                }

                if (m_eventListenerArrays.ContainsKey(eventName))
                {
                    m_eventListenerArrays[eventName] = m_eventListeners[eventName].ToArray();
                }
                else
                {
                    m_eventListenerArrays.Add(eventName, m_eventListeners[eventName].ToArray());
                }
			}
		}


        public EventResult SendEvent(int eventName, object eventData = null)
		{
            if(m_eventListenerArrays.ContainsKey(eventName))
			{
				EventResult result;

                int listenerCount = m_eventListenerArrays[eventName].Length;

                for(int i = 0; i < listenerCount; i++)
				{
                    result = m_eventListenerArrays[eventName][i].Listener.OnEvent(eventName, eventData);

                    if (null == result)
                    {
                        continue;
                    }

                    return result;
				}
			}

			return null;
		}
	}
}