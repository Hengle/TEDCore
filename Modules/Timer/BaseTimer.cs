
namespace TEDCore.Timer
{
    public abstract class BaseTimer
    {
        public bool IsDone { get { return m_isDone; } }

        private float m_duration;
        private bool m_isDone = false;

        protected BaseTimer(float duration)
        {
            m_duration = duration;
        }

        public void Update(float deltaTime)
        {
            m_duration -= deltaTime;

            if (m_duration <= 0 && !m_isDone)
            {
                m_isDone = true;

                OnTimerFinished();
            }
        }

        public abstract void OnTimerFinished();
    }
}