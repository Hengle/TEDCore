using TEDCore;
using TEDCore.Event;
using UnityEngine;

namespace TEDCore.Input
{
	public class TouchInput : InputBase, IInput
	{
		private int m_fistTouchID;
		private bool m_touch = false;

		private bool m_pressed = false;
		private Vector2 m_pressPosition;

		public TouchInput()
		{
			Status = InputStatus.None;
			PressPosition = Vector2.zero;
			ReleasePosition = Vector2.zero;
			HeldTime = 0.0f;
		}
		
		public void Update (float deltaTime)
		{
			if(UnityEngine.Input.touchCount == 0)
			{
				Status = InputStatus.None;
				return;
			}

			Touch touch = UnityEngine.Input.GetTouch(0);

			// If players are not pressing the screen then the first finger
			if(Status == InputStatus.None)
			{
				m_fistTouchID = touch.fingerId;
			}

			m_touch = false;

			// Find the right touch finger id
			for(int i = 0; i < UnityEngine.Input.touchCount; i++)
			{
				if(UnityEngine.Input.GetTouch(i).fingerId == m_fistTouchID)
				{
					touch = UnityEngine.Input.GetTouch(i);
					m_touch = true;
				}
			}

			if(!m_touch)
				return;
			
			switch(touch.phase)
			{
			case TouchPhase.Began:
				Status = InputStatus.Pressed;
				HeldTime = deltaTime;
				PressPosition = touch.position;
				ReleasePosition = touch.position;
				
				SendEvent();
				break;
			case TouchPhase.Canceled:
			case TouchPhase.Ended:
				Status = InputStatus.Released;
				ReleasePosition = touch.position;
				
				SendEvent();
				break;
			case TouchPhase.Moved:
			case TouchPhase.Stationary:
				Status = InputStatus.Held;
				HeldTime += deltaTime;
				ReleasePosition = touch.position;

				SendEvent();
				break;
			default:
				Status = InputStatus.None;
				break;
			}
		}
		
		
		public void SendEvent()
		{
			Services.Get<EventManager>().SendEvent(InputManager.TOUCH_INPUT, new InputData(Status, PressPosition, ReleasePosition, HeldTime));

			if(Status == InputStatus.Pressed)
			{
				m_pressed = true;
				m_pressPosition = PressPosition;
			}
			else if(Status == InputStatus.Released && m_pressed)
			{
				m_pressed = false;
				if(ReleasePosition.x - m_pressPosition.x > Screen.width * 0.1f)
				{
					Services.Get<EventManager>().SendEvent(InputManager.SWAP_LEFT);
				}
				
				if(ReleasePosition.x - m_pressPosition.x < Screen.width * -0.1f)
				{
					Services.Get<EventManager>().SendEvent(InputManager.SWAP_RIGHT);
				}

				m_pressPosition = Vector2.zero;
			}
		}
	}
}