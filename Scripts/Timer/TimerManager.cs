using System.Collections.Generic;
using UnityEngine;
using System;

namespace TEDCore.Timer
{
    public class TimerManager : MonoSingleton<TimerManager>
	{
        private List<BaseTimer> m_timers;
        private Queue<BaseTimer> m_removingTimers;
        private BaseTimer m_cacheTimer;

        private void Awake()
		{
            m_timers = new List<BaseTimer>();
            m_removingTimers = new Queue<BaseTimer>();
		}

        public BaseTimer Schedule(float duration, Action onTimerFinished)
        {
            NormalTimer normalTimer = new NormalTimer(duration, onTimerFinished);
            Add(normalTimer);

            return normalTimer;
        }

        public BaseTimer Schedule<T>(float duration, Action<T> onTimerFinished, T timerData)
        {
            NormalTimer<T> normalTimer = new NormalTimer<T>(duration, onTimerFinished, timerData);
            Add(normalTimer);

            return normalTimer;
        }

        public void Add(BaseTimer timer)
		{
			m_timers.Add(timer);
		}

        public void Remove(BaseTimer timer)
		{
			m_timers.Remove(timer);
		}

        private void Update()
        {
            if (null == m_timers)
            {
                return;
            }

            if (m_timers.Count == 0)
            {
                return;
            }

            for (int cnt = 0; cnt < m_timers.Count; cnt++)
            {
                m_cacheTimer = m_timers [cnt];
                m_cacheTimer.Update (Time.deltaTime);

                if(m_cacheTimer.IsDone)
                {
                    m_removingTimers.Enqueue(m_cacheTimer);
                }
            }

            while (m_removingTimers.Count > 0)
            {
                m_timers.Remove(m_removingTimers.Dequeue());
            }
        }
	}
}