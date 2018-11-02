using TEDCore.Timer;

namespace TEDCore.Performance
{
    public class PerformanceManager : Singleton<PerformanceManager>
    {
        private const float UPDATE_DURATION = 1.0f;

        public FpsPerformanceData FpsData { get { return m_fpsData; } }
        private FpsPerformanceData m_fpsData = new FpsPerformanceData();

        private BaseTimer m_updateTimer;

        public PerformanceManager()
        {
            m_updateTimer = TimerManager.Instance.Schedule(UPDATE_DURATION, UpdateData);
        }


        private void OnDestroy()
        {
            if (m_updateTimer != null)
            {
                Singleton<TimerManager>.Instance.Remove(m_updateTimer);
                m_updateTimer = null;
            }
        }

        
        public void Reset()
        {
            m_fpsData.ResetValue();
        }


        private void UpdateData()
        {
            m_fpsData.Update();
            m_updateTimer = TimerManager.Instance.Schedule(UPDATE_DURATION, UpdateData);
        }
    }
}