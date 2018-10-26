
namespace TEDCore.StateManagement
{
    public class StateManager : IUpdate, IFixedUpdate
	{
		public bool IsPause { get; set; }
		public State CurrentState { get; private set; }


		public StateManager()
		{
			IsPause = false;
			CurrentState = null;
		}


		public void ChangeState(State newState)
		{
			if(CurrentState != null)
			{
				CurrentState.Destroy();
			}

			CurrentState = newState;
		}


		public void Update (float deltaTime)
		{
			if(CurrentState != null && !IsPause)
			{
				CurrentState.Update(deltaTime);
			}
		}


        public void FixedUpdate(float fixedDeltaTime)
        {
            if (CurrentState != null && !IsPause)
            {
                CurrentState.FixedUpdate(fixedDeltaTime);
            }
        }
    }
}