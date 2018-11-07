using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

namespace TEDCore.Debugger
{
    [DisallowMultipleComponent]
    public class DebuggerDragHelper : MonoBehaviour
    {
        private const float ANIMATION_DURATION = 0.1f;
        private const float FADING_DURATION = 3.0f;
        private const float ALPHA_FULL = 1.0f;
        private const float ALPHA_HALF = 0.5f;

        [SerializeField] private bool m_moveToBorder = true;

        private DebuggerSizeHelper m_debuggerSizeHelper;
        private RectTransform m_rectTransform;
        private Vector2 m_fullWindowSize;

        #region Drag
        private Vector2 m_startPosition;
        private Vector2 m_targetPosition;
        private float m_moveTimer;
        private bool m_isMoving;
        #endregion

        #region Fading
        private CanvasGroup m_canvasGroup;
        private float m_fadingTimer;
        private bool m_isFading;
        #endregion

        private void Awake()
        {
            m_debuggerSizeHelper = GetComponent<DebuggerSizeHelper>();
            m_rectTransform = GetComponent<RectTransform>();

            EventTrigger eventTrigger = GetComponent<EventTrigger>();
            EventTrigger.Entry entryDrag = new EventTrigger.Entry();
            entryDrag.eventID = EventTriggerType.Drag;
            entryDrag.callback.AddListener(OnDrag);
            eventTrigger.triggers.Add(entryDrag);

            EventTrigger.Entry entryEndDrag = new EventTrigger.Entry();
            entryEndDrag.eventID = EventTriggerType.EndDrag;
            entryEndDrag.callback.AddListener(OnEndDrag);
            eventTrigger.triggers.Add(entryEndDrag);

            m_canvasGroup = GetComponent<CanvasGroup>();
            m_canvasGroup.alpha = ALPHA_HALF;

            UpdateWindowSize();
        }

        private IEnumerator Start()
        {
            yield return new WaitForEndOfFrame();

            UpdateWindowSize();
            OnEndDrag(null);
        }

        private void UpdateWindowSize()
        {
            m_fullWindowSize = GetComponentInParent<Canvas>().GetComponent<RectTransform>().sizeDelta / 2;
        }

        private void Update()
        {
            if(m_isFading)
            {
                m_fadingTimer -= Time.deltaTime;
                if(m_fadingTimer <= 0)
                {
                    m_fadingTimer = 0;
                    m_isFading = false;
                }

                m_canvasGroup.alpha = Mathf.Lerp(ALPHA_HALF, ALPHA_FULL, m_fadingTimer / 0.2f);

                if (m_debuggerSizeHelper != null && m_debuggerSizeHelper.IsFullScreen())
                {
                    m_isFading = false;
                    m_canvasGroup.alpha = 1;
                }
            }

            if (m_isMoving)
            {
                m_moveTimer += Time.deltaTime;
                m_rectTransform.anchoredPosition = Vector2.Lerp(m_startPosition, m_targetPosition, m_moveTimer / ANIMATION_DURATION);

                if (m_moveTimer >= ANIMATION_DURATION)
                {
                    m_isMoving = false;
                    m_rectTransform.anchoredPosition = m_targetPosition;
                }
            }
        }

        public bool IsMoving()
        {
            return m_isMoving;
        }

        private bool CantMove()
        {
            if(m_debuggerSizeHelper != null)
            {
                return IsMoving() || m_debuggerSizeHelper.IsFullScreen();
            }
            else
            {
                return IsMoving();
            }
        }

        private void OnDrag(BaseEventData eventData)
        {
            if (CantMove())
            {
                return;
            }

            m_isFading = true;
            m_fadingTimer = FADING_DURATION;

            transform.position = Input.mousePosition;
            m_isMoving = false;
        }

        private void OnEndDrag(BaseEventData eventData)
        {
            if (!m_moveToBorder)
            {
                m_targetPosition = m_rectTransform.anchoredPosition;
                return;
            }

            if (CantMove())
            {
                return;
            }

            m_isFading = true;
            m_fadingTimer = FADING_DURATION;

            Vector2 position = m_rectTransform.anchoredPosition;

            float[] borderDistance = new float[4];
            borderDistance[0] = m_fullWindowSize.y - position.y;
            borderDistance[1] = position.y + m_fullWindowSize.y;
            borderDistance[2] = position.x + m_fullWindowSize.x;
            borderDistance[3] = m_fullWindowSize.x - position.x;

            int closestIndex = 0;
            float closestDistance = borderDistance[closestIndex];
            for (int i = 1; i < borderDistance.Length; i++)
            {
                if (closestDistance > borderDistance[i])
                {
                    closestIndex = i;
                    closestDistance = borderDistance[i];
                }
            }

            SetTargetPosition(closestIndex);
        }

        private void SetTargetPosition(int direction)
        {
            m_startPosition = m_rectTransform.anchoredPosition;
            m_targetPosition = m_rectTransform.anchoredPosition;
            switch (direction)
            {
                case 0:
                    m_targetPosition.y = m_fullWindowSize.y - m_rectTransform.sizeDelta.y / 2;
                    m_targetPosition.x = Mathf.Clamp(m_targetPosition.x, -m_fullWindowSize.x + m_rectTransform.sizeDelta.x / 2, m_fullWindowSize.x - m_rectTransform.sizeDelta.x / 2);
                    break;
                case 1:
                    m_targetPosition.y = -m_fullWindowSize.y + m_rectTransform.sizeDelta.y / 2;
                    m_targetPosition.x = Mathf.Clamp(m_targetPosition.x, -m_fullWindowSize.x + m_rectTransform.sizeDelta.x / 2, m_fullWindowSize.x - m_rectTransform.sizeDelta.x / 2);
                    break;
                case 2:
                    m_targetPosition.x = -m_fullWindowSize.x + m_rectTransform.sizeDelta.x / 2;
                    m_targetPosition.y = Mathf.Clamp(m_targetPosition.y, -m_fullWindowSize.y + m_rectTransform.sizeDelta.y / 2, m_fullWindowSize.y - m_rectTransform.sizeDelta.y / 2);
                    break;
                case 3:
                    m_targetPosition.x = m_fullWindowSize.x - m_rectTransform.sizeDelta.x / 2;
                    m_targetPosition.y = Mathf.Clamp(m_targetPosition.y, -m_fullWindowSize.y + m_rectTransform.sizeDelta.y / 2, m_fullWindowSize.y - m_rectTransform.sizeDelta.y / 2);
                    break;
            }

            m_moveTimer = 0;
            m_isMoving = true;
        }
    }
}
