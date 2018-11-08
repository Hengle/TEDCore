using UnityEngine;

namespace TEDCore.Debugger
{
    [ExecuteInEditMode]
    public class DebuggerTitleSizeAdjuster : MonoBehaviour
    {
        [SerializeField] private RectTransform[] m_titleRectTransforms;
        [SerializeField] private RectTransform[] m_contentRectTransforms;
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

            if (m_titleRectTransforms != null && m_titleRectTransforms.Length != 0)
            {
                for (int i = 0; i < m_titleRectTransforms.Length; i++)
                {
                    Vector2 sizeDelta = m_titleRectTransforms[i].sizeDelta;
                    sizeDelta.y = m_width * 0.08f * 0.5f;
                    m_titleRectTransforms[i].sizeDelta = sizeDelta;
                }
            }

            if (m_contentRectTransforms != null && m_contentRectTransforms.Length != 0)
            {
                for (int i = 0; i < m_contentRectTransforms.Length; i++)
                {
                    m_contentRectTransforms[i].offsetMax = new Vector2(0, -m_width * 0.08f * 0.5f);
                }
            }
        }
    }
}
