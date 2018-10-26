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


        public abstract void Show(bool show);
        public abstract void Update(float deltaTime);
	}
}