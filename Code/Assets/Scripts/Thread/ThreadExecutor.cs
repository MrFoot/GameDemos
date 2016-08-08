using System;
using Soulgame.Util;
using System.Threading;


namespace Soulgame.Threading
{
	public class ThreadExecutor : Executor
	{
		private const string Tag = "ThreadExecutor";
		
		private readonly object Lock = new object();
		
		private int sleepMillis = 50;
		
		public string Name
		{
			get;
			private set;
		}
		
		public int SleepMillis
		{
			get
			{
				return this.sleepMillis;
			}
			set
			{
				Assert.IsTrue(value >= 0, "value must be >= 0", new object[0]);
				this.sleepMillis = value;
			}
		}
		
		public bool IsQuit
		{
			get;
			protected set;
		}
		
		public ThreadExecutor(string name) : base(false)
		{
			this.Name = name;
			GameThread gameThread = new GameThread(new Action(this.Run));
			gameThread.Name = name;
			gameThread.IsBackground = true;
			gameThread.Start();
			object @lock = this.Lock;
			lock (@lock)
			{
				if (gameThread.IsAlive && this.looper == null)
				{
					Monitor.Wait(this.Lock);
				}
			}
		}
		
		protected virtual void Run()
		{
			object @lock = this.Lock;
			lock (@lock)
			{
				this.looper = Looper.ThreadInstance;
				Monitor.Pulse(this.Lock);
			}
			while (!this.IsQuit)
			{
				try
				{
					this.looper.LoopOnce();
				}
				catch (Exception ex)
				{
					UnityLogHandler.HandleException("Inside ThreadExecutor named '" + this.Name + "': " + ex.Message, ex);
					this.Quit();
				}
				GameThread.Sleep(this.sleepMillis);
			}
		}
		
		public override void PostAtTime(Action action, DateTime time)
		{
			if (this.IsQuit)
			{
				return;
			}
			base.PostAtTime(action, time);
		}
		
		public override void PostAtFrontQueue(Action action)
		{
			if (this.IsQuit)
			{
				return;
			}
			base.PostAtFrontQueue(action);
		}
		
		public virtual void Quit()
		{
			this.IsQuit = true;
		}
	}
}

