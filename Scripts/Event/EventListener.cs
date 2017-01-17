using System.Collections.Generic;

namespace TEDCore.Event
{
	public class EventListener : IEventListener, IDestroyable
	{
		public delegate EventResult EventCallback(string eventName, object eventData);


		protected class EventListenerData
		{
			public EventCallback Callback;
			public bool CallWhenInactive;
		}


		protected Dictionary<string, EventListenerData> m_eventListeners;


		private bool m_active;
		public bool Active
		{
			get { return m_active; }
			set
			{
				m_active = value;
				OnActiveChanged();
			}
		}


		protected virtual void OnActiveChanged()
		{

		}


		public EventListener()
		{
			m_eventListeners = new Dictionary<string, EventListenerData>();
		}


		public void ListenForEvent(string eventName, EventCallback callback, bool callWhenInactive = false, int priority = 0)
		{
			EventListenerData eventListenerData = new EventListenerData();
			eventListenerData.Callback = callback;
			eventListenerData.CallWhenInactive = callWhenInactive;

			m_eventListeners[eventName] = eventListenerData;

			GameSystemManager.Get<EventManager>().RegisterListener(eventName, this, priority);
		}


		public void StopListenForEvent(string eventName)
		{
			if(m_eventListeners.ContainsKey(eventName))
			{
				m_eventListeners.Remove(eventName);

				GameSystemManager.Get<EventManager>().UnregisterListener(eventName, this);
			}
		}


		#region IEventListener
		public EventResult OnEvent (string eventName, object eventData)
		{
			if(m_eventListeners.ContainsKey(eventName))
			{
				EventListenerData eventListenerData = m_eventListeners[eventName];
				
				if(!Active && !eventListenerData.CallWhenInactive)
				{
					return null;
				}
				
				if(eventListenerData.Callback != null)
				{
					return eventListenerData.Callback(eventName, eventData);
				}
			}
			
			return null;
		}
		#endregion


		#region IDestroyable
		public virtual void Destroy ()
		{
			foreach(string eventName in m_eventListeners.Keys)
			{
				GameSystemManager.Get<EventManager>().UnregisterListener(eventName, this);
			}

			m_eventListeners.Clear();
		}
		#endregion
	}
}