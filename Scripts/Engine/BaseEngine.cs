using UnityEngine;
using TEDCore.StateManagement;
using TEDCore.Input;

namespace TEDCore
{
	public class BaseEngine : MonoBehaviour, IInitialize
	{
		protected StateManager m_stateManager;

		private static BaseEngine m_instance;
		public static BaseEngine Instance { get { return m_instance; } }

		private void Awake()
		{
			m_instance = this;

			Initialize ();
		}


		public virtual void Initialize()
		{
			m_stateManager = new StateManager();
		}


		public virtual void Update()
		{
			if (null != m_stateManager.CurrentState)
			{
				m_stateManager.Update (Time.deltaTime);
			}
		}
	}
}