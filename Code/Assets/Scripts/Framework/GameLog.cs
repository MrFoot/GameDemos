using System;
using FootStudio.Threading;
using System.Diagnostics;
using System.Text;
using UnityEngine;
using FootStudio.Util;

namespace FootStudio.Framework
{
	public static class GameLog
	{
		public enum LogLevel
		{
			VERBOSE,
			DEBUG,
			INFO,
			WARN,
			ERROR
		}
		
		private static int MainThreadId = -1;
		
		public static int MaxUnityLogLength = 5120;
		
		public static GameLog.LogLevel Level
		{
			get;
			set;
		}
		
		public static bool VerboseEnabled
		{
			get
			{
				return GameLog.LogLevel.VERBOSE >= GameLog.Level;
			}
		}
		
		public static bool DebugEnabled
		{
			get
			{
				return GameLog.LogLevel.DEBUG >= GameLog.Level;
			}
		}
		
		public static bool InfoEnabled
		{
			get
			{
				return GameLog.LogLevel.INFO >= GameLog.Level;
			}
		}
		
		public static bool WarnEnabled
		{
			get
			{
				return GameLog.LogLevel.WARN >= GameLog.Level;
			}
		}
		
		public static bool ErrorEnabled
		{
			get
			{
				return GameLog.LogLevel.ERROR >= GameLog.Level;
			}
		}
		
		public static void SetMainThread()
		{
			GameLog.MainThreadId = GameThread.CurrentThreadId;
		}
		
		[Conditional("DEBUG")]
		public static void Verbose(string message)
		{
			GameLog.Log(GameLog.LogLevel.VERBOSE, null, message, null, null);
		}
		
		[Conditional("DEBUG")]
		public static void Verbose(string message, params object[] args)
		{
			GameLog.Log(GameLog.LogLevel.VERBOSE, null, message, null, args);
		}
		
		[Conditional("DEBUG")]
		public static void Verbose(Exception e, string message)
		{
			GameLog.Log(GameLog.LogLevel.VERBOSE, null, message, e, null);
		}
		
		[Conditional("DEBUG")]
		public static void Verbose(Exception e, string message, params object[] args)
		{
			GameLog.Log(GameLog.LogLevel.VERBOSE, null, message, e, args);
		}
		
		[Conditional("DEBUG")]
		public static void VerboseT(string tag, string message)
		{
			GameLog.Log(GameLog.LogLevel.VERBOSE, tag, message, null, null);
		}
		
		[Conditional("DEBUG")]
		public static void VerboseT(string tag, string message, params object[] args)
		{
			GameLog.Log(GameLog.LogLevel.VERBOSE, tag, message, null, args);
		}
		
		[Conditional("DEBUG")]
		public static void VerboseT(string tag, Exception e, string message)
		{
			GameLog.Log(GameLog.LogLevel.VERBOSE, tag, message, e, null);
		}
		
		[Conditional("DEBUG")]
		public static void VerboseT(string tag, Exception e, string message, params object[] args)
		{
			GameLog.Log(GameLog.LogLevel.VERBOSE, tag, message, e, args);
		}
		
		[Conditional("DEBUG")]
		public static void Debug(string message)
		{
			GameLog.Log(GameLog.LogLevel.DEBUG, null, message, null, null);
		}
		
		[Conditional("DEBUG")]
		public static void Debug(string message, params object[] args)
		{
			GameLog.Log(GameLog.LogLevel.DEBUG, null, message, null, args);
		}
		
		[Conditional("DEBUG")]
		public static void Debug(Exception e, string message)
		{
			GameLog.Log(GameLog.LogLevel.DEBUG, null, message, e, null);
		}
		
		[Conditional("DEBUG")]
		public static void Debug(Exception e, string message, params object[] args)
		{
			GameLog.Log(GameLog.LogLevel.DEBUG, null, message, e, args);
		}
		
		[Conditional("DEBUG")]
		public static void DebugT(string tag, string message)
		{
			GameLog.Log(GameLog.LogLevel.DEBUG, tag, message, null, null);
		}
		
		[Conditional("DEBUG")]
		public static void DebugT(string tag, string message, params object[] args)
		{
			GameLog.Log(GameLog.LogLevel.DEBUG, tag, message, null, args);
		}
		
		[Conditional("DEBUG")]
		public static void DebugT(string tag, Exception e, string message)
		{
			GameLog.Log(GameLog.LogLevel.DEBUG, tag, message, e, null);
		}
		
		[Conditional("DEBUG")]
		public static void DebugT(string tag, Exception e, string message, params object[] args)
		{
			GameLog.Log(GameLog.LogLevel.DEBUG, tag, message, e, args);
		}
		
		[Conditional("DEBUG")]
		public static void Info(string message)
		{
			GameLog.Log(GameLog.LogLevel.INFO, null, message, null, null);
		}
		
		[Conditional("DEBUG")]
		public static void Info(string message, params object[] args)
		{
			GameLog.Log(GameLog.LogLevel.INFO, null, message, null, args);
		}
		
		[Conditional("DEBUG")]
		public static void Info(Exception e, string message)
		{
			GameLog.Log(GameLog.LogLevel.INFO, null, message, e, null);
		}
		
		[Conditional("DEBUG")]
		public static void Info(Exception e, string message, params object[] args)
		{
			GameLog.Log(GameLog.LogLevel.INFO, null, message, e, args);
		}
		
		[Conditional("DEBUG")]
		public static void InfoT(string tag, string message)
		{
			GameLog.Log(GameLog.LogLevel.INFO, tag, message, null, null);
		}
		
		[Conditional("DEBUG")]
		public static void InfoT(string tag, string message, params object[] args)
		{
			GameLog.Log(GameLog.LogLevel.INFO, tag, message, null, args);
		}
		
		[Conditional("DEBUG")]
		public static void InfoT(string tag, Exception e, string message)
		{
			GameLog.Log(GameLog.LogLevel.INFO, tag, message, e, null);
		}
		
		[Conditional("DEBUG")]
		public static void InfoT(string tag, Exception e, string message, params object[] args)
		{
			GameLog.Log(GameLog.LogLevel.INFO, tag, message, e, args);
		}
		
		[Conditional("DEBUG")]
		public static void Warn(string message)
		{
			GameLog.Log(GameLog.LogLevel.WARN, null, message, null, null);
		}
		
		[Conditional("DEBUG")]
		public static void Warn(string message, params object[] args)
		{
			GameLog.Log(GameLog.LogLevel.WARN, null, message, null, args);
		}
		
		[Conditional("DEBUG")]
		public static void Warn(Exception e, string message)
		{
			GameLog.Log(GameLog.LogLevel.WARN, null, message, e, null);
		}
		
		[Conditional("DEBUG")]
		public static void Warn(Exception e, string message, params object[] args)
		{
			GameLog.Log(GameLog.LogLevel.WARN, null, message, e, args);
		}
		
		[Conditional("DEBUG")]
		public static void WarnT(string tag, string message)
		{
			GameLog.Log(GameLog.LogLevel.WARN, tag, message, null, null);
		}
		
		[Conditional("DEBUG")]
		public static void WarnT(string tag, string message, params object[] args)
		{
			GameLog.Log(GameLog.LogLevel.WARN, tag, message, null, args);
		}
		
		[Conditional("DEBUG")]
		public static void WarnT(string tag, Exception e, string message)
		{
			GameLog.Log(GameLog.LogLevel.WARN, tag, message, e, null);
		}
		
		[Conditional("DEBUG")]
		public static void WarnT(string tag, Exception e, string message, params object[] args)
		{
			GameLog.Log(GameLog.LogLevel.WARN, tag, message, e, args);
		}
		
		public static void Error(string message)
		{
			GameLog.Log(GameLog.LogLevel.ERROR, null, message, null, null);
		}
		
		public static void Error(string message, params object[] args)
		{
			GameLog.Log(GameLog.LogLevel.ERROR, null, message, null, args);
		}
		
		public static void Error(Exception e, string message)
		{
			GameLog.Log(GameLog.LogLevel.ERROR, null, message, e, null);
		}
		
		public static void Error(Exception e, string message, params object[] args)
		{
			GameLog.Log(GameLog.LogLevel.ERROR, null, message, e, args);
		}
		
		public static void ErrorT(string tag, string message)
		{
			GameLog.Log(GameLog.LogLevel.ERROR, tag, message, null, null);
		}
		
		public static void ErrorT(string tag, string message, params object[] args)
		{
			GameLog.Log(GameLog.LogLevel.ERROR, tag, message, null, args);
		}
		
		public static void ErrorT(string tag, Exception e, string message)
		{
			GameLog.Log(GameLog.LogLevel.ERROR, tag, message, e, null);
		}
		
		public static void ErrorT(string tag, Exception e, string message, params object[] args)
		{
			GameLog.Log(GameLog.LogLevel.ERROR, tag, message, e, args);
		}
		
		private static void Log(GameLog.LogLevel level, string tag, string format, Exception e, params object[] args)
		{
			if (level < GameLog.Level)
			{
				return;
			}
			StringBuilder stringBuilder = new StringBuilder(512);
			stringBuilder.Append(level);
			stringBuilder.Append(" ");
			string value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
			stringBuilder.Append(value);
			stringBuilder.Append(" (t");
			stringBuilder.Append(GameThread.CurrentThreadId);
			if (GameThread.CurrentThreadId == GameLog.MainThreadId)
			{
				stringBuilder.Append(" f");
				stringBuilder.Append(Time.frameCount);
			}
			stringBuilder.Append(") -- ");
			if (tag != null)
			{
				stringBuilder.Append("#");
				stringBuilder.Append(tag);
				stringBuilder.Append(" -- ");
			}
			string value2 = format;
			if (args != null)
			{
				value2 = string.Format(format, args);
			}
			stringBuilder.Append(value2);
			if (e != null)
			{
				stringBuilder.Append(" <");
				stringBuilder.Append(e.ToString());
				stringBuilder.Append(">");
			}
			string text = stringBuilder.ToString();
			FileLogger.Log(text, level == GameLog.LogLevel.ERROR);
			if (text.Length > GameLog.MaxUnityLogLength)
			{
				text = text.Substring(0, GameLog.MaxUnityLogLength);
			}
			switch (level)
			{
			case GameLog.LogLevel.VERBOSE:
			case GameLog.LogLevel.DEBUG:
			case GameLog.LogLevel.INFO:
				UnityEngine.Debug.Log(text);
				break;
			case GameLog.LogLevel.WARN:
				UnityEngine.Debug.LogWarning(text);
				break;
			case GameLog.LogLevel.ERROR:
				UnityEngine.Debug.LogError(text);
				break;
			}
		}
	}
}

