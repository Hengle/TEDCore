using UnityEngine;
using UnityEngine.EventSystems;

namespace TEDCore.UI
{
	public class UIEventTrigger : EventTrigger
	{
		public delegate void VoidDelegate (GameObject go);
		public delegate void PositionDelegate (GameObject go, Vector2 position);

		public VoidDelegate onClick;
		public PositionDelegate onDown;
		public VoidDelegate onEnter;
		public VoidDelegate onExit;
		public PositionDelegate onUp;
		public VoidDelegate onSelect;
		public VoidDelegate onUpdateSelect;

        public static UIEventTrigger Get (GameObject go)
		{
			UIEventTrigger listener = go.GetComponent<UIEventTrigger>();

			if (listener == null)
            {
                listener = go.AddComponent<UIEventTrigger>();
            }

			return listener;
		}

		public override void OnPointerClick(PointerEventData eventData)
		{
			if(onClick != null)
            {
                onClick(gameObject);
            }
		}

		public override void OnPointerDown(PointerEventData eventData)
		{
			if(onDown != null)
            {
                onDown(gameObject, eventData.position);
            }
		}

        public override void OnPointerEnter(PointerEventData eventData)
        {
            if (onEnter != null)
            {
                onEnter(gameObject);
            }
        }

		public override void OnPointerExit(PointerEventData eventData)
		{
			if(onExit != null)
            {
                onExit(gameObject);
            }
		}

		public override void OnPointerUp(PointerEventData eventData)
		{
			if(onUp != null)
            {
                onUp(gameObject, eventData.position);
            }
		}

		public override void OnSelect(BaseEventData eventData)
		{
			if(onSelect != null)
            {
                onSelect(gameObject);
            }
		}

		public override void OnUpdateSelected(BaseEventData eventData)
		{
			if(onUpdateSelect != null)
            {
                onUpdateSelect(gameObject);
            }
		}
	}
}