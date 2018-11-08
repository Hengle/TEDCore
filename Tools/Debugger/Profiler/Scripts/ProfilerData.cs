using UnityEngine;

namespace TEDCore.Debugger.Profiler
{
    public abstract class ProfilerData
    {
        public float CurValue { get { return m_curValue; } }
        public float MinValue { get { return m_minValue; } }
        public float MaxValue { get { return m_maxValue; } }
        public float AvgValue { get { return m_avgValue; } }
        private float m_curValue;
        private float m_minValue;
        private float m_maxValue;
        private float m_avgValue;

        private float m_totalValue;
        private int m_totalCount;

        public ProfilerData()
        {
            ResetValue();
        }


        public void ResetValue()
        {
            m_curValue = -1;
            m_minValue = -1;
            m_maxValue = -1;
            m_avgValue = -1;
            m_totalValue = 0;
            m_totalCount = 0;
        }


        public abstract void Update();


        protected void UpdateValue(float value)
        {
            m_curValue = value;

            if (m_curValue < m_minValue || m_minValue == -1)
            {
                m_minValue = m_curValue;
            }

            if (m_curValue > m_maxValue || m_maxValue == -1)
            {
                m_maxValue = m_curValue;
            }

            m_totalValue += m_curValue;
            m_totalCount++;

            m_avgValue = m_totalValue / m_totalCount;
            m_avgValue = Mathf.Round(m_avgValue * 100) / 100;
        }
    }
}