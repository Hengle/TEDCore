using System;
using System.Collections.Generic;
using UnityEngine;

namespace TEDCore.Timer
{
	public class TimerManager
	{
		private List<Timer> _timers;
		private bool _pause = false;
		private Timer _timerCache;
		private float _lastRealTime = 0;

		public TimerManager()
		{
			_timers = new List<Timer> ();
		}


		public void Add(Timer timer)
		{
			_timers.Add (timer);
		}


		public void Remove(Timer timer)
		{
			_timers.Remove (timer);
		}


		public void Update()
		{
			_lastRealTime = Time.realtimeSinceStartup - _lastRealTime;

			if (_timers.Count != 0 && !_pause)
			{
				for (int cnt = 0; cnt < _timers.Count; cnt++)
				{
					_timerCache = _timers [cnt];
					_timerCache.Update (_lastRealTime);

					if(_timerCache.HaveFinished)
					{
						Remove (_timerCache);
					}
				}
			}

			_lastRealTime = Time.realtimeSinceStartup;
		}


		public void Pause()
		{
			_pause = true;
		}


		public void Resume()
		{
			_pause = false;
		}
	}
}