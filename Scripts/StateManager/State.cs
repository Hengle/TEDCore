using TEDCore.Event;
using System.Collections.Generic;

namespace TEDCore.StateManagement
{
    public abstract class State : IDestroyable, IUpdate, IEventListener
	{
		public delegate EventResult EventCallback(string eventName, object eventData);

		public StateManager StateManager { get; private set; }
		public TaskManager TaskManager { get; private set; }
		public bool AlreadyInit { get; set; }

		protected Dictionary<string, EventCallback> m_eventListeners;


		public State(StateManager stateManager)
		{
			StateManager = stateManager;
			TaskManager = new TaskManager(stateManager);
			AlreadyInit = false;

			m_eventListeners = new Dictionary<string, EventCallback>();
		}


		public abstract void Init();


		#region IDestroyable
		public virtual void Destroy ()
		{
			foreach(string eventName in m_eventListeners.Keys)
			{
				GameSystemManager.Get<EventManager>().UnregisterListener(eventName, this);
			}

			m_eventListeners.Clear();
			TaskManager.Destroy();
		}
		#endregion


		#region IUpdate
		public virtual void Update (float deltaTime)
		{
			TaskManager.Update(deltaTime);
		}
		#endregion


		#region IEventListener
		public EventResult OnEvent (string eventName, object eventData)
		{
			if(m_eventListeners.ContainsKey(eventName))
			{
				EventCallback callback = m_eventListeners[eventName];

				if(callback != null)
				{
					return callback(eventName, eventData);
				}
			}

			return null;
		}
		#endregion


		public void ListenForEvent(string eventName, EventCallback callback, int priority = 0)
		{
			m_eventListeners[eventName] = callback;

			GameSystemManager.Get<EventManager>().RegisterListener(eventName, this, priority);
		}
	}
}