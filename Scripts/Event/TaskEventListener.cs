using System.Collections.Generic;

namespace TEDCore.Event
{
	public class TaskEventListener : IEventListener, IDestroyable
	{
		public delegate EventResult EventCallback(string eventName, object eventData);


		protected class TaskEventListenerData
		{
			public EventCallback Callback;
			public bool CallWhenInactive;
		}


		protected Dictionary<string, TaskEventListenerData> m_eventListeners;


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


		public TaskEventListener()
		{
			m_eventListeners = new Dictionary<string, TaskEventListenerData>();
		}


		public void ListenForEvent(string eventName, EventCallback callback, bool callWhenInactive = false, int priority = 0)
		{
			TaskEventListenerData teld = new TaskEventListenerData();
			teld.Callback = callback;
			teld.CallWhenInactive = callWhenInactive;

			m_eventListeners[eventName] = teld;

			Services.Get<EventManager>().RegisterListener(eventName, this, priority);
		}


		public void StopListenForEvent(string eventName)
		{
			if(m_eventListeners.ContainsKey(eventName))
			{
				m_eventListeners.Remove(eventName);

				Services.Get<EventManager>().UnregisterListener(eventName, this);
			}
		}


		#region IEventListener
		public EventResult OnEvent (string eventName, object eventData)
		{
			if(m_eventListeners.ContainsKey(eventName))
			{
				TaskEventListenerData teld = m_eventListeners[eventName];
				
				if(!Active && !teld.CallWhenInactive)
				{
					return null;
				}
				
				if(teld.Callback != null)
				{
					return teld.Callback(eventName, eventData);
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
				Services.Get<EventManager>().UnregisterListener(eventName, this);
			}

			m_eventListeners.Clear();
		}
		#endregion
	}
}