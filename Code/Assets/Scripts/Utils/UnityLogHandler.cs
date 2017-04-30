using System;
using FootStudio.Threading;
using UnityEngine;
using FootStudio.Framework;

namespace FootStudio.Util
{
	public static class UnityLogHandler
	{
		private static bool IsExceptionAlreadyDispatched;
		
		public static MainThread MainExecutor
		{
			get;
			set;
		}
		
		public static void RegisterMe()
		{
			Application.logMessageReceivedThreaded += new Application.LogCallback(UnityLogHandler.HandleLog);
		}
		
		private static void HandleLog(string logString, string stackTrace, LogType type)
		{
			switch (type)
			{
			case LogType.Assert:
			case LogType.Exception:
				if (UnityLogHandler.IsExceptionAlreadyDispatched)
				{
					return;
				}
				UnityLogHandler.IsExceptionAlreadyDispatched = true;
				GameLog.Error("{0} with message: '{1}' and stack trace: <{2}>", new object[]{
					type,
					logString,
					stackTrace
				});
				break;
			}
		}
		
		public static void HandleException(string message, Exception e)
		{
			if (!object.ReferenceEquals(UnityLogHandler.MainExecutor, null))
			{
				UnityLogHandler.MainExecutor.RunOnMainThread(delegate{
					UnityLogHandler.HandleLog(message, e.StackTrace, LogType.Exception);
				});
			}
			GameLog.Error(e, message);
		}
	}
}

