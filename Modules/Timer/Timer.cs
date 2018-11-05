using System;

namespace TEDCore.Timer
{
    public class Timer : IUpdate
    {
        public bool IsDone;

        private float m_duration;
        private Action<float> m_onUpdate;
        private Action m_onComplete;

        public void Update(float deltaTime)
        {
            m_duration -= deltaTime;

            if (m_duration <= 0 && !IsDone)
            {
                IsDone = true;

                if (m_onComplete != null)
                {
                    m_onComplete();
                }
            }
            else
            {
                if (m_onUpdate != null)
                {
                    m_onUpdate(m_duration);
                }
            }
        }

        public Timer SetDuration(float duration)
        {
            m_duration = duration;
            return this;
        }

        public Timer OnUpdate(Action<float> onUpdate)
        {
            m_onUpdate = onUpdate;
            return this;
        }

        public Timer OnComplete(Action onComplete)
        {
            m_onComplete = onComplete;
            return this;
        }
    }
}