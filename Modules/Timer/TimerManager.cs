using UnityEngine;
using System.Collections.Generic;

namespace TEDCore.Timer
{
    public class TimerManager : MonoSingleton<TimerManager>
    {
        private List<Timer> m_timers;
        private Queue<Timer> m_removeTimers;
        private Timer m_cacheTimer;

        private void Awake()
        {
            m_timers = new List<Timer>();
            m_removeTimers = new Queue<Timer>();
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

            for (int i = 0; i < m_timers.Count; i++)
            {
                m_cacheTimer = m_timers[i];
                m_cacheTimer.Update(Time.deltaTime);

                if (m_cacheTimer.IsDone)
                {
                    m_removeTimers.Enqueue(m_cacheTimer);
                }
            }

            while (m_removeTimers.Count > 0)
            {
                m_timers.Remove(m_removeTimers.Dequeue());
            }

        }

        public Timer Create()
        {
            Timer newTimer = new Timer();
            m_timers.Add(newTimer);

            return newTimer;
        }
    }
}