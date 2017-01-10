using TEDCore;
using TEDCore.Event;
using UnityEngine;

namespace TEDCore.Input
{
	public class MouseInput : InputBase, IInput
	{
		private bool m_pressed = false;
		private Vector2 m_pressPosition;

		public MouseInput()
		{
			Status = InputStatus.None;
			PressPosition = Vector2.zero;
			ReleasePosition = Vector2.zero;
			HeldTime = 0.0f;
		}

		public void Update (float deltaTime)
		{
			if(UnityEngine.Input.GetMouseButtonDown(0))
			{
				Status = InputStatus.Pressed;
				PressPosition = (Vector2)UnityEngine.Input.mousePosition;

				SendEvent();
				return;
			}
			else if(UnityEngine.Input.GetMouseButtonUp(0))
			{
				Status = InputStatus.Released;
				ReleasePosition = (Vector2)UnityEngine.Input.mousePosition;

				SendEvent();
				return;
			}

			switch(Status)
			{
			case InputStatus.Pressed:
				Status = InputStatus.Held;
				HeldTime = deltaTime;
				ReleasePosition = (Vector2)UnityEngine.Input.mousePosition;

				SendEvent();
				break;
			case InputStatus.Held:
				HeldTime += deltaTime;
				ReleasePosition = (Vector2)UnityEngine.Input.mousePosition;

				SendEvent();
				break;
			case InputStatus.Released:
				Status = InputStatus.None;
				break;
			}
		}


		public void SendEvent()
		{
			Services.Get<EventManager>().SendEvent(InputManager.MOUSE_INPUT, new InputData(Status, PressPosition, ReleasePosition, HeldTime));

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