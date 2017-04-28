using UnityEngine;
using System.Collections;
using System;
using FootStudio.Util;

public static class ServiceTime {

	private static long s_unixTime;

	private static float s_startTime;

	/// <summary>
	/// DataTime类型的时间
	/// </summary>
	/// <value>The current time.</value>
	public static DateTime CurTime {
		get {
			return Utils.Unix2DateTime (ServiceTime.CurUnixTime);
		}
	}

	public static long CurUnixTime {
		set {
			s_unixTime = value;
			s_startTime = UnityEngine.Time.realtimeSinceStartup;
		}
		get {
			return s_unixTime + (long)(UnityEngine.Time.realtimeSinceStartup - s_startTime);
		}
	}

	/// <summary>
	/// 返回剩余成熟时间
	/// </summary>
	/// <returns>MatureRemain.TotalSeconds < 1表示已经成熟</returns>
	/// <param name="seedTime">播种时间</param>
	/// <param name="needMinute">需要多少秒成熟</param>
	public static TimeSpan MatureRemain(DateTime seedTime, int needSecond) {
		TimeSpan span = seedTime.AddSeconds (needSecond) - CurTime;
		if (span < TimeSpan.Zero)
			span = TimeSpan.Zero;
		return new TimeSpan (span.Hours, span.Minutes, span.Seconds);
	}

	public static TimeSpan MatureRemain(long seedTime, int needSecond) {
		TimeSpan span = Utils.Unix2DateTime(seedTime).AddSeconds (needSecond) - CurTime;
		if (span < TimeSpan.Zero)
			span = TimeSpan.Zero;
		return new TimeSpan (span.Hours, span.Minutes, span.Seconds);
	}
}
