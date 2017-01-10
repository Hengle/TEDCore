using TEDCore.Event;

namespace TEDCore.StateManagement
{
    public abstract class Task : TaskEventListener, IUpdate
	{
		public TaskManager TaskManager;
		public StateManager StateManager { get { return TaskManager.StateManager; } }

		public Task() : base()
		{

		}

		protected override void OnActiveChanged ()
		{
			if(Active)
			{
				Resume();
			}
			else
			{
				Pause();
			}
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

		public abstract void Pause();
		public abstract void Resume();
		public abstract void Show(bool show);
		public abstract void Update(float deltaTime);
	}
}