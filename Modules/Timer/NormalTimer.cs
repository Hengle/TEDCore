using System;

namespace TEDCore.Timer
{
    public class NormalTimer : BaseTimer
    {
        private Action m_onTimerFinished;

        public NormalTimer(float duration, Action onTimerFinished) : base(duration)
        {
            m_onTimerFinished = onTimerFinished;
        }

        public override void OnTimerFinished()
        {
            if(m_onTimerFinished == null)
            {
                return;
            }

            m_onTimerFinished();
        }
    }

    public class NormalTimer<T> : BaseTimer
    {
        private Action<T> m_onTimerFinished;
        private T m_timerData;

        public NormalTimer(float duration, Action<T> onTimerFinished, T timerData) : base(duration)
        {
            m_onTimerFinished = onTimerFinished;
            m_timerData = timerData;
        }

        public override void OnTimerFinished()
        {
            if (m_onTimerFinished == null)
            {
                return;
            }

            m_onTimerFinished(m_timerData);
        }
    }
}