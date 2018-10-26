using TEDCore.Event;
using System.Collections.Generic;

namespace TEDCore.StateManagement
{
	public abstract class State : EventListener, IUpdate
	{
		public StateManager StateManager { get; private set; }
		public TaskManager TaskManager { get; private set; }

		public State(StateManager stateManager) : base()
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
	}
}