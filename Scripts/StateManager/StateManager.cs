
namespace TEDCore.StateManagement
{
    public class StateManager : IUpdate, IFixedUpdate, ILateUpdate
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
            if (CurrentState != null)
            {
                CurrentState.Destroy();
            }

            CurrentState = newState;
        }


        #region IUpdate
        public void Update(float deltaTime)
        {
            if (CurrentState != null && !IsPause)
            {
                CurrentState.Update(deltaTime);
            }
        }
        #endregion


        #region IFixedUpdate
        public void FixedUpdate(float deltaTime)
        {
            if (CurrentState != null && !IsPause)
            {
                CurrentState.FixedUpdate(deltaTime);
            }
        }
        #endregion


        #region ILateUpdate
        public void LateUpdate(float deltaTime)
        {
            if (CurrentState != null && !IsPause)
            {
                CurrentState.LateUpdate(deltaTime);
            }
        }
        #endregion
    }
}