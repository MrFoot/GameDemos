using System;
using System.Collections.Generic;
using FootStudio.Framework;


namespace FootStudio.Threading
{
	public sealed class Looper
	{
		private struct LooperTask
		{
			public Action Action;
			public DateTime When;
			public LooperTask(Action action, DateTime when) {
				this.Action = action;
				this.When = when.ToUniversalTime(); //减了时区的时间
			}
		}

		private const string Tag = "Looper";

		private static readonly object Guard = new object();

		[ThreadStatic]
		private static Looper looper;

		private readonly object Locker = new object();
		
		private readonly LinkedList<Looper.LooperTask> TaskQueue = new LinkedList<Looper.LooperTask>();
		
		private readonly List<Looper.LooperTask> ReadyTasks = new List<Looper.LooperTask>();

		public static bool DoesLooperExistForCurrentThread
		{
			get
			{
				return Looper.looper != null;
			}
		}
		
		public static Looper ThreadInstance
		{
			get
			{
				object guard = Looper.Guard;
				Looper result;
				lock (guard)
				{
					if (Looper.looper == null)
					{
						Looper.looper = new Looper();
					}
					result = Looper.looper;
				}
				return result;
			}
		}

		public int LoopOnce()
		{
			if (this.TaskQueue.Count == 0)
			{
				return 0;
			}
			if (this.ReadyTasks.Count > 0)
			{
				throw new InvalidOperationException("Concurrent call is not allowed");
			}
			object locker = this.Locker;
			lock (locker)
			{
				DateTime utcNow = DateTime.UtcNow;
				LinkedListNode<Looper.LooperTask> linkedListNode = this.TaskQueue.First;
				while (linkedListNode != null)
				{
					Looper.LooperTask value = linkedListNode.Value;
					if (value.When <= utcNow)
					{
						this.ReadyTasks.Add(value);
						LinkedListNode<Looper.LooperTask> node = linkedListNode;
						linkedListNode = linkedListNode.Next;
						this.TaskQueue.Remove(node);
					}
					else
					{
						linkedListNode = linkedListNode.Next;
					}
				}
			}
			int count = this.ReadyTasks.Count;
			if (count == 0)
			{
				return 0;
			}
			for (int i = 0; i < count; i++)
			{
				this.ReadyTasks[i].Action();
			}
			this.ReadyTasks.Clear();
			return count;
		}
		
		public void Schedule(Action action, DateTime when, bool atFrontQueue)
		{
			Assert.NotNull(action, "action");
			object locker = this.Locker;
			lock (locker)
			{
				Looper.LooperTask value = new Looper.LooperTask(action, when);
				if (atFrontQueue)
				{
					this.TaskQueue.AddFirst(value);
				}
				else
				{
					this.TaskQueue.AddLast(value);
				}
			}
		}
		
		public int RemoveAllSchedules(Action action)
		{
			Assert.NotNull(action, "action");
			object locker = this.Locker;
			int result;
			lock (locker)
			{
				int num = 0;
				LinkedListNode<Looper.LooperTask> linkedListNode = this.TaskQueue.First;
				while (linkedListNode != null)
				{
					if (linkedListNode.Value.Action == action)
					{
						LinkedListNode<Looper.LooperTask> node = linkedListNode;
						linkedListNode = linkedListNode.Next;
						this.TaskQueue.Remove(node);
						num++;
					}
					else
					{
						linkedListNode = linkedListNode.Next;
					}
				}
				result = num;
			}
			return result;
		}
	}
}

