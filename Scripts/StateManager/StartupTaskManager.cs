using System.Collections.Generic;
using TEDCore.Utils;

namespace TEDCore.Startup
{
	public class StartupTaskManager
	{
		private Queue<IStartupTask> m_tasks;

		public StartupTaskManager()
		{
			m_tasks = new Queue<IStartupTask>();
		}


		public void AddTask(IStartupTask task)
		{
			m_tasks.Enqueue(task);
		}


		public void Update(float deltaTime)
		{
			if(m_tasks.Count <= 0)
			{
				return;
			}

			m_tasks.Peek().Update(deltaTime);

			if(m_tasks.Peek().IsDone)
			{
				Debugger.Log(string.Format("[StartupTaskManager] - {0} Done", m_tasks.Peek().ToString()));
				m_tasks.Peek().Destroy();
				m_tasks.Dequeue();
			}
		}


		public void Start()
		{
			if(m_tasks.Count > 0)
			{
				Debugger.Log(string.Format("[StartupTaskManager] - {0} Initialize", m_tasks.Peek().ToString()));
				if(!m_tasks.Peek().Init())
				{
					m_tasks.Dequeue();
					Start();
				}
			}
		}


		public bool IsDone()
		{
			return m_tasks.Count == 0;
		}
	}
}