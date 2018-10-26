﻿using System.Collections.Generic;
using System;

namespace TEDCore.StateManagement
{
    public class TaskManager : IUpdate, IFixedUpdate, IDestroyable
	{
		public StateManager StateManager { get; private set; }
		public long CurrentState { get; private set; }

		private class TaskData
		{
			public Task Task;
			public long ActiveStates;
		}

		private List<TaskData> m_tasks;

		public TaskManager(StateManager stateManager)
		{
			m_tasks = new List<TaskData>();
			StateManager = stateManager;
		}


        public void AddTask(Task task, params Enum[] activeStates)
        {
            task.TaskManager = this;

            TaskData td = new TaskData();
            td.Task = task;
            td.ActiveStates = TaskUtils.GetStateId(activeStates);

            m_tasks.Add(td);
        }


        public void AddTask(Task task, long activeStates)
        {
            task.TaskManager = this;

            TaskData td = new TaskData();
            td.Task = task;
            td.ActiveStates = activeStates;

            m_tasks.Add(td);
        }


        public void ChangeState(Enum state)
		{
            CurrentState = TaskUtils.GetStateId(state);

			// Deactive
			for(int cnt = 0; cnt < m_tasks.Count; cnt++)
			{
				if(!ContainState(m_tasks[cnt].ActiveStates, CurrentState))
				{
					m_tasks[cnt].Task.Active = false;
				}
			}

			// Active
			for(int cnt = 0; cnt < m_tasks.Count; cnt++)
			{
                if(ContainState(m_tasks[cnt].ActiveStates, CurrentState))
				{
					m_tasks[cnt].Task.Active = true;
				}
			}
		}


        private static bool ContainState(long stateFlags, long stateId)
        {
            return (stateFlags & stateId) == stateId;
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


        #region IFixedUpdate
        public void FixedUpdate(float fixedDeltaTime)
        {
            for (int cnt = 0; cnt < m_tasks.Count; cnt++)
            {
                if (m_tasks[cnt].Task.Active)
                {
                    m_tasks[cnt].Task.FixedUpdate(fixedDeltaTime);
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
		}
        #endregion
    }
}
