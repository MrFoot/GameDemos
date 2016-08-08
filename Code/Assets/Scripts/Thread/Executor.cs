using System;
namespace Soulgame.Threading
{
	public class Executor
	{
		protected Looper looper;
		
		public Executor() : this(true)
		{
		}
		
		protected Executor(bool attachLooper)
		{
			if (!attachLooper)
			{
				return;
			}
			if (!Looper.DoesLooperExistForCurrentThread)
			{
				throw new InvalidOperationException("Looper does not exist for current thread");
			}
			this.looper = Looper.ThreadInstance;
		}
		
		public virtual void Post(Action action)
		{
			this.PostAtTime(action, DateTime.UtcNow);
		}
		
		public virtual void PostDelayed(Action action, double delaySecs)
		{
			this.PostAtTime(action, DateTime.UtcNow.AddSeconds(delaySecs));
		}
		
		public virtual void PostAtTime(Action action, DateTime time)
		{
			this.looper.Schedule(action, time, false);
		}
		
		public virtual void PostAtFrontQueue(Action action)
		{
			this.looper.Schedule(action, DateTime.UtcNow, true);
		}
		
		public int RemoveAllSchedules(Action action)
		{
			return this.looper.RemoveAllSchedules(action);
		}
	}
}

