using UnityEngine;

namespace TEDCore.Debugger
{
    [ExecuteInEditMode]
    public class DebuggerTitleSizeAdjuster : MonoBehaviour
    {
        [SerializeField] private RectTransform m_titleRectTransform;
        [SerializeField] private RectTransform m_contentRectTransform;
        private RectTransform m_canvasRectTransform;
        private float m_width;

        private void Update()
        {
            if (m_canvasRectTransform == null)
            {
                m_canvasRectTransform = GetComponent<RectTransform>();
            }

            if (m_width == m_canvasRectTransform.sizeDelta.x)
            {
                return;
            }

            m_width = m_canvasRectTransform.sizeDelta.x;

            if (m_titleRectTransform != null)
            {
                Vector2 sizeDelta = m_titleRectTransform.sizeDelta;
                sizeDelta.y = m_width * 0.08f * 0.5f;
                m_titleRectTransform.sizeDelta = sizeDelta;
            }

            if (m_contentRectTransform != null)
            {
                m_contentRectTransform.offsetMax = new Vector2(0, -m_width * 0.08f * 0.5f);
            }
        }
    }
}
