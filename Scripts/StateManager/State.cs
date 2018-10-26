using TEDCore.Event;

namespace TEDCore.StateManagement
{
    public abstract class State : EventListener, IUpdate, IFixedUpdate, ILateUpdate
	{
		public StateManager StateManager { get; private set; }
		public TaskManager TaskManager { get; private set; }

        protected State(StateManager stateManager) : base()
		{
			StateManager = stateManager;
			TaskManager = new TaskManager(stateManager);
		}


		public override void Destroy ()
		{
			base.Destroy ();
			TaskManager.Destroy();
		}


		#region IUpdate
		public virtual void Update (float deltaTime)
		{
			TaskManager.Update(deltaTime);
		}
        #endregion


        #region IFixedUpdate
        public virtual void FixedUpdate(float deltaTime)
        {
            TaskManager.FixedUpdate(deltaTime);
        }
        #endregion


        #region ILateUpdate
        public virtual void LateUpdate(float deltaTime)
        {
            TaskManager.LateUpdate(deltaTime);
        }
        #endregion
    }
}