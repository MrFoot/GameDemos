
using System;
using System.IO;


namespace Soulgame.Util
{
	public static class GameFile
	{
		public static bool Exists(string path)
		{
			return File.Exists(path);
		}
		
		public static bool Delete(string path)
		{
			if (!GameFile.Exists(path))
			{
				return false;
			}
			File.Delete(path);
			return true;
		}
		
		public static void Copy(string sourceFileName, string destFileName)
		{
			File.Copy(sourceFileName, destFileName);
		}
		
		public static DateTime GetLastWriteTime(string path)
		{
			return File.GetLastWriteTime(path);
		}
		
		public static string ReadAllText(string path)
		{
			return File.ReadAllText(path);
		}
		
		public static void WriteAllText(string path, string data)
		{
			File.WriteAllText(path, data);
		}
		
		public static byte[] ReadAllBytes(string path)
		{
			return File.ReadAllBytes(path);
		}
		
		public static void WriteAllBytes(string path, byte[] data)
		{
			File.WriteAllBytes(path, data);
		}
	}
}

