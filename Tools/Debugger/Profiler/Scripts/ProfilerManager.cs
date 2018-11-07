using TEDCore.Coroutine;
using System;

namespace TEDCore.Debugger
{
    public class ProfilerManager : Singleton<ProfilerManager>
    {
        public FpsProfilerData FpsData { get { return m_fpsData; } }
        private FpsProfilerData m_fpsData = new FpsProfilerData();

        private float m_updateDuration = 1.0f;
        private Action m_onUpdate;
        private CoroutineChain m_coroutineChain;

        public ProfilerManager()
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

        public void SetUpdateCallback(Action callback)
        {
            m_onUpdate = callback;
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
            if(m_onUpdate != null)
            {
                m_onUpdate();
            }

            m_coroutineChain = CoroutineManager.Instance.Create()
                                                    .Enqueue(CoroutineUtils.WaitForSeconds(m_updateDuration))
                                                    .Enqueue(UpdateData)
                                                    .StartCoroutine();
        }
    }
}