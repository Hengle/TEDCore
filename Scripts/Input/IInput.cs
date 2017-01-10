using UnityEngine;

namespace TEDCore.Input
{
    public interface IInput : IUpdate
	{
		void SendEvent();
	}
}