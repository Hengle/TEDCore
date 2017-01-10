using UnityEngine;

namespace TEDCore.Input
{
	public class InputBase
	{
		public InputStatus Status { get; set; }
		public Vector2 PressPosition { get; set; }
		public Vector2 ReleasePosition { get; set; }
		public float HeldTime { get; set; }
	}
}