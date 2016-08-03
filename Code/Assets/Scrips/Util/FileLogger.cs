using System;
using System.IO;
using UnityEngine;


namespace Soulgame.Util
{
	public static class FileLogger
	{
		private static readonly object Lock;
		
		private static StreamWriter File;
		
		public static string LogPath
		{
			get;
			private set;
		}
		
		static FileLogger()
		{
			FileLogger.Lock = new object();
			FileLogger.LogPath = Application.persistentDataPath + "/SoulGameLog.txt";
			if (BuildConfig.IsDebug)
			{
				Debug.Log("### Log path: " + FileLogger.LogPath);
			}
		}
		
		public static void Start()
		{
			object @lock = FileLogger.Lock;
			lock (@lock)
			{
				if (FileLogger.File == null)
				{
					FileLogger.File = new StreamWriter(FileLogger.LogPath, true);
					FileLogger.File.AutoFlush = true;
					FileLogger.File.WriteLine("################### STARTED NEW LOG SESSION ###################");
				}
			}
		}
		
		public static void Stop()
		{
			object @lock = FileLogger.Lock;
			lock (@lock)
			{
				if (FileLogger.File != null)
				{
					FileLogger.File.Flush();
					FileLogger.File.Dispose();
					FileLogger.File = null;
				}
			}
		}
		
		public static void Clear()
		{
			object @lock = FileLogger.Lock;
			lock (@lock)
			{
				FileLogger.Stop();
				GameFile.Delete(FileLogger.LogPath);
				FileLogger.Start();
			}
		}
		
		public static void Log(string msg)
		{
			FileLogger.Log(msg, false);
		}
		
		public static void Log(string msg, bool flushImmediately)
		{
			object @lock = FileLogger.Lock;
			lock (@lock)
			{
				if (FileLogger.File == null)
				{
					FileLogger.Start();
				}
				FileLogger.File.WriteLine(msg);
				if (flushImmediately)
				{
					FileLogger.File.Flush();
				}
			}
		}
	}
}

