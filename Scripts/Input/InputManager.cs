using UnityEngine;

namespace TEDCore.Input
{
	public enum InputStatus
	{
		None,
		Pressed,
		Held,
		Released
	}

	public class InputData
	{
		public InputStatus Status;
		public Vector2 PressedPosition;
		public Vector2 ReleasedPosition;
		public float HeldTime;
		
		public InputData(InputStatus status, Vector2 pressPosition, Vector2 releasePosition, float heldTime)
		{
			Status = status;
			PressedPosition = pressPosition;
			ReleasedPosition = releasePosition;
			HeldTime = heldTime;
		}
	}

    public class InputManager : IUpdate
	{
		public const string MOUSE_INPUT = "MOUSE_INPUT";
		public const string TOUCH_INPUT = "TOUCH_INPUT";
		public const string SWAP_LEFT = "SWAP_LEFT";
		public const string SWAP_RIGHT = "SWAP_RIGHT";

		public TouchInput Touch { get; private set; }
		public MouseInput Mouse { get; private set; }

		public InputManager()
		{
			if(UnityEngine.Application.platform == UnityEngine.RuntimePlatform.Android ||
			   UnityEngine.Application.platform == UnityEngine.RuntimePlatform.IPhonePlayer ||
			   UnityEngine.Application.platform == UnityEngine.RuntimePlatform.WP8Player)
			{
				Touch = new TouchInput();
				Mouse = null;
			}
			else
			{
				Touch = null;
				Mouse = new MouseInput();
			}
		}


		public void Update(float deltaTime)
		{
			if(Touch != null)
			{
				Touch.Update(deltaTime);
			}

			if(Mouse != null)
			{
				Mouse.Update(deltaTime);
			}
		}
	}
}