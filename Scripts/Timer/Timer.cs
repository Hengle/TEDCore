using System;

namespace TEDCore.Timer
{
	public class Timer
	{
		public float Duration { get { return _duration; } }
		public object Data { get { return _data; } }
		public bool HaveFinished { get { return _haveFinished; } }

		private float _duration;
		private Action<object> _onTimerFinished;
		private object _data;
		private bool _haveFinished = false;

		public Timer(float duration, Action<object> onTimerFinished = null, object data = null)
		{
			_duration = duration;
			_onTimerFinished = onTimerFinished;
			_data = data;
		}


		public void Update(float deltaTime)
		{
			_duration -= deltaTime;

			if (_duration <= 0 && !_haveFinished)
			{
				_haveFinished = true;

				if (null != _onTimerFinished)
				{
					_onTimerFinished (_data);
				}
			}
		}
	}
}