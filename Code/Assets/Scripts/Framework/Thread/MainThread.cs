using UnityEngine;
using System.Collections;
using System;

namespace FootStudio.Threading
{
	public class MainThread : MonoBehaviour {

		private Looper looper;

		private int mainThreadId;

		public bool IsMainThread {
			get {
				return GameThread.CurrentThreadId == this.mainThreadId;
			}
		}

		protected virtual void Awake() {
			this.looper = Looper.ThreadInstance;
			this.mainThreadId = GameThread.CurrentThreadId;
		}

		protected virtual void Update() {
			this.looper.LoopOnce ();
		}

		public void RunOnMainThread(Action action)
		{
			if (this.IsMainThread)
			{
				action();
			}
			else
			{
				this.Post(action);
			}
		}
		
		public void Post(Action action)
		{
			this.PostAtTime(action, DateTime.UtcNow);
		}
		
		public void PostDelayed(Action action, double delaySecs)
		{
			this.PostAtTime(action, DateTime.UtcNow.AddSeconds(delaySecs));
		}
		
		public void PostAtTime(Action action, DateTime time)
		{
			this.looper.Schedule(action, time, false);
		}
		
		public int RemoveAllSchedules(Action action)
		{
			return this.looper.RemoveAllSchedules(action);
		}
	}

}
