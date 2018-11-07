using UnityEngine;

namespace TEDCore.Debugger
{
    public class FpsProfilerData : ProfilerData
    {
        private float m_lastTime;
        private float m_lastFrameCount;
        private float m_timeSpan;
        private float m_frameCount;
        private float m_cacheValue;

        public override void Update()
        {
            m_timeSpan = Time.realtimeSinceStartup - m_lastTime;
            m_frameCount = Time.frameCount - m_lastFrameCount;

            m_lastFrameCount = Time.frameCount;
            m_lastTime = Time.realtimeSinceStartup;

            m_cacheValue = Mathf.RoundToInt(m_frameCount / m_timeSpan);

            UpdateValue(m_cacheValue);
        }
    }
}
