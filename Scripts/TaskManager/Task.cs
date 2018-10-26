using TEDCore.Event;

namespace TEDCore.StateManagement
{
    public abstract class Task : EventListener, IUpdate, IFixedUpdate, ILateUpdate
	{
		public TaskManager TaskManager;
		public StateManager StateManager { get { return TaskManager.StateManager; } }

        protected Task() : base()
		{

		}


		protected override void OnActiveChanged ()
		{
			Show (Active);
		}


        public abstract void Show(bool show);
        public abstract void Update(float deltaTime);
        public abstract void FixedUpdate(float fixedDeltaTime);
        public abstract void LateUpdate(float deltaTime);
    }
}