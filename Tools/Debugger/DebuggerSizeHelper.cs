using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

namespace TEDCore.Debugger
{
    [DisallowMultipleComponent]
    public class DebuggerSizeHelper : MonoBehaviour
    {
        private const float FULL_SIZE_RATIO = 1.0f;

        [SerializeField] private float m_duration = 0.1f;
        [SerializeField] private GameObject m_dragImage;
        [SerializeField] private GameObject m_fullScreen;

        private RectTransform m_rectTransform;
        private Canvas m_canvas;
        private Vector2 m_startPosition;
        private Vector2 m_targetPosition;

        #region Screen
        private Vector2 m_initSizeDelta;
        private Vector2 m_fullSizeDelta;
        private float m_pointerDownTime;
        private Vector2 m_startSizeDelta;
        private Vector2 m_targetSizeDelta;
        private float m_sizeTimer;
        private bool m_isSizing;
        #endregion

        private void Awake()
        {
            m_rectTransform = GetComponent<RectTransform>();

            EventTrigger eventTrigger = GetComponent<EventTrigger>();
            EventTrigger.Entry entryPointerDown = new EventTrigger.Entry();
            entryPointerDown.eventID = EventTriggerType.PointerDown;
            entryPointerDown.callback.AddListener(OnPointerDown);
            eventTrigger.triggers.Add(entryPointerDown);

            EventTrigger.Entry entryPointerUp = new EventTrigger.Entry();
            entryPointerUp.eventID = EventTriggerType.PointerUp;
            entryPointerUp.callback.AddListener(OnPointerUp);
            eventTrigger.triggers.Add(entryPointerUp);

            m_dragImage.SetActive(true);
            m_fullScreen.SetActive(false);
        }

        private IEnumerator Start()
        {
            yield return new WaitForEndOfFrame();

            m_canvas = GetComponentInParent<Canvas>();
            m_canvas.sortingOrder = 99;

            Vector2 sizeDelta = m_canvas.GetComponent<RectTransform>().sizeDelta;

            m_initSizeDelta = m_rectTransform.sizeDelta;
            m_fullSizeDelta = sizeDelta * FULL_SIZE_RATIO;
        }

        private void Update()
        {
            if (m_isSizing)
            {
                m_sizeTimer += Time.deltaTime;
                m_rectTransform.anchoredPosition = Vector2.Lerp(m_startPosition, m_targetPosition, m_sizeTimer / m_duration);
                m_rectTransform.sizeDelta = Vector2.Lerp(m_startSizeDelta, m_targetSizeDelta, m_sizeTimer / m_duration);

                if (m_sizeTimer >= m_duration)
                {
                    m_isSizing = false;
                    m_rectTransform.anchoredPosition = m_targetPosition;
                    m_rectTransform.sizeDelta = m_targetSizeDelta;

                    if (m_targetSizeDelta == m_initSizeDelta)
                    {
                        m_dragImage.SetActive(true);
                        m_fullScreen.SetActive(false);
                    }
                    else if (m_targetSizeDelta == m_fullSizeDelta)
                    {
                        m_fullScreen.SetActive(true);
                    }
                }
            }
        }

        private bool IsSizing()
        {
            return m_isSizing;
        }

        public bool IsFullScreen()
        {
            return m_fullScreen.activeInHierarchy;
        }

        public void OnPointerDown(BaseEventData eventData)
        {
            if (IsSizing() || IsFullScreen())
            {
                return;
            }

            m_pointerDownTime = Time.realtimeSinceStartup;
        }

        public void OnPointerUp(BaseEventData eventData)
        {
            if (IsSizing() || IsFullScreen())
            {
                return;
            }

            float pointerUpTime = Time.realtimeSinceStartup;
            if (pointerUpTime - m_pointerDownTime > 0.1f)
            {
                return;
            }

            m_canvas.sortingOrder = 999;

            m_dragImage.SetActive(false);

            m_startPosition = m_rectTransform.anchoredPosition;
            m_targetPosition = Vector2.zero;
            m_startSizeDelta = m_initSizeDelta;
            m_targetSizeDelta = m_fullSizeDelta;

            m_sizeTimer = 0;
            m_isSizing = true;
        }

        public void OnFullScreenCloseButtonClicked()
        {
            if (IsSizing())
            {
                return;
            }

            m_canvas.sortingOrder = 99;

            m_targetPosition = m_startPosition;
            m_startPosition = Vector2.zero;
            m_startSizeDelta = m_fullSizeDelta;
            m_targetSizeDelta = m_initSizeDelta;

            m_sizeTimer = 0;
            m_isSizing = true;
        }
    }
}
