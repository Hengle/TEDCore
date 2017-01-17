using TEDCore.Event;

namespace TEDCore.StateManagement
{
    public abstract class Task : EventListener, IUpdate
	{
		public TaskManager TaskManager;
		public StateManager StateManager { get { return TaskManager.StateManager; } }

		public Task() : base()
		{

		}

		protected override void OnActiveChanged ()
		{
			Show (Active);
		}

		public bool Visible
		{
			get { return m_visible; }
			set
			{
				m_visible = value;
				Show(m_visible);
			}
		}

		private bool m_visible;

		public abstract void Show(bool show);
		public abstract void Update(float deltaTime);
	}
}