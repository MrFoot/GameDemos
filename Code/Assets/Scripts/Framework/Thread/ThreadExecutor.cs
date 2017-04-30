using System;
using FootStudio.Framework;
using FootStudio.Util;
using System.Threading;

/*
 * 因为Monitor的开门关门机制很有意思，忍不住加点日志
 * Monitor的wait和pulse，可以参考http://www.cnblogs.com/zhycyq/articles/2679017.html
 * 
 * 构造函数和Run里面有个lock，看起来很吓人。
 * 说一下这个锁的用意：
 * constructor里面如果发现loop=null，不自己创建，而是先wait（会暂时释放锁、并进入等待队列），然后等待Run线程创建loop，
 * 再通过pulse唤醒主线程（使主线程从等待队列，进入就绪队列——注意，还是卡住的状态，需要等待Run线程释放了lock，才会走到下一步）
 * 总结一下：
 * 如果constructor先获得锁，又发现loop=null，就是wait，进入休眠并暂时释放锁。run获得锁，创建了loop，然后pulse唤醒了constructor的线程，
 * 然后释放了锁。constructor获得锁，从就绪队列进入执行队列，继续。
 * 如果run先获得锁，一切都没的说，因为constructor会等待，等到的时候loop!=null，所以啥都无所谓。
 * 但在这里，因为基类的构造函数会先执行，并初始化loop，所以连1也不会触发。
 * 不过这个机制很有意思，可以实现一些有意思的东西。
 */
namespace FootStudio.Threading
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

