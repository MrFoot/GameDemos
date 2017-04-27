using System;
using System.Threading;

namespace FootStudio.Threading
{
	public class GameThread
	{
		private readonly Thread Thread;
		
		public static int CurrentThreadId
		{
			get
			{
				return Thread.CurrentThread.ManagedThreadId;
			}
		}
		
		public string Name
		{
			get
			{
				return this.Thread.Name;
			}
			set
			{
				this.Thread.Name = value;
			}
		}
		
		public bool IsBackground
		{
			get
			{
				return this.Thread.IsBackground;
			}
			set
			{
				this.Thread.IsBackground = value;
			}
		}
		
		public bool IsAlive
		{
			get
			{
				return this.Thread.IsAlive;
			}
		}
		
		public GameThread(Action job)
		{
			this.Thread = new Thread(new ThreadStart(job.Invoke));
		}
		
		public static void Sleep(int millisecondsTimeout)
		{
			Thread.Sleep(millisecondsTimeout);
		}
		
		public static void Sleep(TimeSpan timeout)
		{
			long num = (long)timeout.TotalMilliseconds;
			if (num < -1L || num > 2147483647L)
			{
				throw new ArgumentOutOfRangeException("timeout");
			}
			Thread.Sleep((int)timeout.TotalMilliseconds);
		}
		
		public void Start()
		{
			this.Thread.Start();
		}
		
		public void Join()
		{
			this.Join(-1);
		}
		
		public void Join(int millisecondsTimeout)
		{
			this.Thread.Join(millisecondsTimeout);
		}
		
		public void Join(TimeSpan timeout)
		{
			long num = (long)timeout.TotalMilliseconds;
			if (num < -1L || num > 2147483647L)
			{
				throw new ArgumentOutOfRangeException("timeout");
			}
			this.Join((int)timeout.TotalMilliseconds);
		}
	}
}

