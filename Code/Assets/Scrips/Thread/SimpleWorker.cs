using System;
using Soulgame.Util;
using System.Threading;

namespace Soulgame.Threading
{
	public static class SimpleWorker
	{
		private const string Tag = "SimpleWorker";

		public static void RunAsync(Action job) {
			Assert.NotNull (job, "job");
			ThreadPool.QueueUserWorkItem (delegate{
				try
				{
					job();
				} catch (Exception e)
				{
					GameLog.Error(e.Message);
				}
			});
		}
	}
}

