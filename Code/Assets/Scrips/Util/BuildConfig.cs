using System;
using UnityEngine;


namespace Soulgame.Util
{
	public static class BuildConfig
	{
		public static bool IsDebug
		{
			get
			{
				return Debug.isDebugBuild;
			}
		}
		
		public static bool IsRelease
		{
			get
			{
				return !BuildConfig.IsDebug;
			}
		}
	}
}

