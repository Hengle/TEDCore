using UnityEngine;
using TEDCore.StateManagement;

namespace TEDCore
{
    public abstract class BaseEngine : MonoBehaviour, IInitialize
	{
		protected StateManager m_stateManager;

		private void Awake()
		{
            m_stateManager = new StateManager();

            Initialize();
		}


        public abstract void Initialize();


		public virtual void Update()
		{
			if (null != m_stateManager.CurrentState)
			{
				m_stateManager.Update (Time.deltaTime);
			}
		}
	}
}