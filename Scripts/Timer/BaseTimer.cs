using System;

namespace TEDCore.Timer
{
	public class BaseTimer
	{
		public bool IsDone { get { return m_isDone; } }

        private float m_duration;
		private Action<object> m_onTimerFinished;
        private object m_timerData;
        private bool m_isDone = false;

        public BaseTimer(float duration, Action<object> onTimerFinished, object timerData = null)
		{
            m_duration = duration;
            m_onTimerFinished = onTimerFinished;
            m_timerData = timerData;
		}

		public void Update(float deltaTime)
		{
            m_duration -= deltaTime;

            if (m_duration <= 0 && !m_isDone)
			{
                m_isDone = true;

                if (null != m_onTimerFinished)
				{
                    m_onTimerFinished (m_timerData);
				}
			}
		}
	}
}