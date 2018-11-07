using TEDCore.Coroutine;

namespace TEDCore.Performance
{
    public class PerformanceManager : Singleton<PerformanceManager>
    {
        public FpsPerformanceData FpsData { get { return m_fpsData; } }
        private FpsPerformanceData m_fpsData = new FpsPerformanceData();

        private float m_updateDuration = 1.0f;
        private CoroutineChain m_coroutineChain;

        public PerformanceManager()
        {
            m_coroutineChain = CoroutineManager.Instance.Create()
                                                    .Enqueue(CoroutineUtils.WaitForSeconds(m_updateDuration))
                                                    .Enqueue(UpdateData)
                                                    .StartCoroutine();
        }

        public void SetUpdateDuration(float duration)
        {
            m_updateDuration = duration;
        }

        private void OnDestroy()
        {
            if (m_coroutineChain != null)
            {
                m_coroutineChain.StopCoroutine();
                m_coroutineChain = null;
            }
        }

        
        public void Reset()
        {
            m_fpsData.ResetValue();
        }


        private void UpdateData()
        {
            m_fpsData.Update();
            m_coroutineChain = CoroutineManager.Instance.Create()
                                                    .Enqueue(CoroutineUtils.WaitForSeconds(m_updateDuration))
                                                    .Enqueue(UpdateData)
                                                    .StartCoroutine();
        }
    }
}