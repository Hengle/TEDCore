using System.Collections.Generic;

namespace TEDCore.StateManagement
{
    public class TaskManager : IUpdate, IDestroyable
	{
		public StateManager StateManager { get; private set; }
		public long CurrentState { get; private set; }

		private class TaskData
		{
			public Task Task;
			public long ActiveStates;
		}

		private List<TaskData> m_tasks;
		private int m_lastStateId = 0;

		public TaskManager(StateManager stateManager)
		{
			m_tasks = new List<TaskData>();
			StateManager = stateManager;
		}

		public long CreateTask()
		{
			long result = 1 << m_lastStateId;
			m_lastStateId++;

			return result;
		}

		public long AllStates()
		{
			long result = 0;

			for(int cnt = 0; cnt <= m_lastStateId; cnt++)
			{
				result |= 1L << cnt;
			}

			return result;
		}

		public static bool ContainsState(long stateFlags, long stateId)
		{
			return (stateFlags & stateId) == stateId;
		}

		public void ChangeState(long stateId)
		{
			CurrentState = stateId;

			// Deactive
			for(int cnt = 0; cnt < m_tasks.Count; cnt++)
			{
				if(!ContainsState(m_tasks[cnt].ActiveStates, stateId))
				{
					m_tasks[cnt].Task.Active = false;
				}
			}

			// Active
			for(int cnt = 0; cnt < m_tasks.Count; cnt++)
			{
				if(ContainsState(m_tasks[cnt].ActiveStates, stateId))
				{
					m_tasks[cnt].Task.Active = true;
				}
			}
		}

		public void AddTask(Task task, long activeStates)
		{
			task.TaskManager = this;

			TaskData td = new TaskData();
			td.Task = task;
			td.ActiveStates = activeStates;

			m_tasks.Add(td);
		}


		public void RemoveTask<T>() where T : Task
		{
			for(int cnt = 0; cnt < m_tasks.Count; cnt++)
			{
				T result = m_tasks[cnt].Task as T;
				
				if(result != null)
				{
					m_tasks[cnt].Task.Destroy();
					m_tasks.RemoveAt(cnt);
				}
			}
		}


		public T GetTask<T>() where T : Task
		{
			for(int cnt = 0; cnt < m_tasks.Count; cnt++)
			{
				T result = m_tasks[cnt].Task as T;

				if(result != null)
				{
					return result;
				}
			}

			return null;
		}

		#region IUpdate
		public void Update (float deltaTime)
		{
			for(int cnt = 0; cnt < m_tasks.Count; cnt++)
			{
				if(m_tasks[cnt].Task.Active)
				{
					m_tasks[cnt].Task.Update(deltaTime);
				}
			}
		}
		#endregion

		#region IDestroyable
		public void Destroy ()
		{
			for(int cnt = 0; cnt < m_tasks.Count; cnt++)
			{
				m_tasks[cnt].Task.TaskManager = null;
				m_tasks[cnt].Task.Destroy();
			}

			m_tasks.Clear();
			m_lastStateId = 0;
		}
		#endregion
	}
}
