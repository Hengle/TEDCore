using TEDCore.Coroutine;

namespace TEDCore.Performance
{
    public class PerformanceManager : Singleton<PerformanceManager>
    {
        private const float UPDATE_DURATION = 1.0f;

        public FpsPerformanceData FpsData { get { return m_fpsData; } }
        private FpsPerformanceData m_fpsData = new FpsPerformanceData();

        private CoroutineChain m_coroutineChain;

        public PerformanceManager()
        {
            m_coroutineChain = CoroutineChainManager.Instance.Create()
                                                    .Enqueue(CoroutineUtils.WaitForSeconds(UPDATE_DURATION))
                                                    .Enqueue(UpdateData)
                                                    .StartCoroutine();
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
            m_coroutineChain = CoroutineChainManager.Instance.Create()
                                                    .Enqueue(CoroutineUtils.WaitForSeconds(UPDATE_DURATION))
                                                    .Enqueue(UpdateData)
                                                    .StartCoroutine();
        }
    }
}