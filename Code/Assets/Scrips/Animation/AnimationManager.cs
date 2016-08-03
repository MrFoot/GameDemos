
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using Soulgame.Asset;

public class AnimationManager
{
	private const string TAG = "AnimationManager";
	
	private const float UnloadTimeout = 1f;
	
	private static Dictionary<string, AnimationClipInfo> DictionaryAnimationClipInfos = new Dictionary<string, AnimationClipInfo>();
	
	private static List<AnimationClipInfo> AnimationClipInfos = new List<AnimationClipInfo>();
	
	private static List<AnimationClipInfo> RemoveAnimationClipInfos = new List<AnimationClipInfo>();
	
	private static float UnloadTime = 0f;
	
	private static void AddAnimationClipInfo(string name, AnimationClipInfo animationClipInfo)
	{
		AnimationManager.DictionaryAnimationClipInfos.Add(name, animationClipInfo);
		AnimationManager.AnimationClipInfos.Add(animationClipInfo);
	}
	
	private static void RemoveAnimationClipInfo(string name)
	{
		AnimationClipInfo item = AnimationManager.DictionaryAnimationClipInfos[name];
		AnimationManager.DictionaryAnimationClipInfos.Remove(name);
		AnimationManager.AnimationClipInfos.Remove(item);
	}
	
	public static AnimationClipInfo FindAnimationClipInfo(string name)
	{
		AnimationClipInfo result;
		if (AnimationManager.DictionaryAnimationClipInfos.TryGetValue(name, out result))
		{
			return result;
		}
		return null;
	}
	
	public static AnimationClipInfo LoadFromResources(string name, float timeout)
	{
		AnimationClipInfo animationClipInfo = AnimationManager.FindAnimationClipInfo(name);
		if (animationClipInfo == null)
		{
			animationClipInfo = new AnimationClipInfo(name, timeout);
			Stopwatch stopwatch = Stopwatch.StartNew();
			uint num = Profiler.GetTotalAllocatedMemory();
			animationClipInfo.LoadFromResources(name);
			stopwatch.Stop();
			AnimationManager.AddAnimationClipInfo(name, animationClipInfo);
			num = Profiler.GetTotalAllocatedMemory() - num;
			long elapsedTicks = stopwatch.ElapsedTicks;
		}
		return animationClipInfo;
	}
	
	public static AnimationClipInfo LoadAsyncFromResources(string name, float timeout)
	{
		AnimationClipInfo animationClipInfo = AnimationManager.FindAnimationClipInfo(name);
		if (animationClipInfo == null)
		{
			animationClipInfo = new AnimationClipInfo(name, timeout);
			Stopwatch stopwatch = Stopwatch.StartNew();
			uint num = Profiler.GetTotalAllocatedMemory();
			animationClipInfo.LoadAsyncFromResources(name);
			stopwatch.Stop();
			AnimationManager.AddAnimationClipInfo(name, animationClipInfo);
			num = Profiler.GetTotalAllocatedMemory() - num;
			long elapsedTicks = stopwatch.ElapsedTicks;
		}
		return animationClipInfo;
	}
	
	public static AnimationClipInfo LoadFromEditorResources(string name, bool hasEvents, float timeout)
	{
		AnimationClipInfo animationClipInfo = AnimationManager.FindAnimationClipInfo(name);
		if (animationClipInfo == null)
		{
			animationClipInfo = new AnimationClipInfo(name, timeout);
			Stopwatch stopwatch = Stopwatch.StartNew();
			uint num = Profiler.GetTotalAllocatedMemory();
			animationClipInfo.LoadFromEditorResources(name);
			stopwatch.Stop();
			AnimationManager.AddAnimationClipInfo(name, animationClipInfo);
			num = Profiler.GetTotalAllocatedMemory() - num;
			long elapsedTicks = stopwatch.ElapsedTicks;
		}
		return animationClipInfo;
	}
	
	public static AnimationClipInfo LoadFromBundle(AssetManager.Bundle bundle, string name, bool hasEvents, float timeout)
	{
		AnimationClipInfo animationClipInfo = AnimationManager.FindAnimationClipInfo(name);
		bool flag = false;
		if (animationClipInfo == null)
		{
			animationClipInfo = new AnimationClipInfo(name, timeout);
			flag = true;
		}
		if (!animationClipInfo.IsLoaded)
		{
			Stopwatch stopwatch = Stopwatch.StartNew();
			uint num = Profiler.GetTotalAllocatedMemory();
			animationClipInfo.LoadFromBundle(bundle, name);
			stopwatch.Stop();
			if (flag)
			{
				AnimationManager.AddAnimationClipInfo(name, animationClipInfo);
			}
			num = Profiler.GetTotalAllocatedMemory() - num;
			long elapsedTicks = stopwatch.ElapsedTicks;
		}
		return animationClipInfo;
	}
	
	public static AnimationClipInfo LoadAsyncFromBundle(AssetManager.Bundle bundle, string name, bool hasEvents, float timeout)
	{
		AnimationClipInfo animationClipInfo = AnimationManager.FindAnimationClipInfo(name);
		if (animationClipInfo == null)
		{
			animationClipInfo = new AnimationClipInfo(name, timeout);
			animationClipInfo.LoadAsyncFromBundle(bundle, name);
			AnimationManager.AddAnimationClipInfo(name, animationClipInfo);
		}
		return animationClipInfo;
	}
	
	public static void Unload(string name)
	{
		AnimationClipInfo animationClipInfo = AnimationManager.FindAnimationClipInfo(name);
		if (animationClipInfo != null)
		{
			animationClipInfo.Unload();
			AnimationManager.RemoveAnimationClipInfo(name);
		}
	}
	
	public static void UnloadAll()
	{
		for (int i = 0; i < AnimationManager.AnimationClipInfos.Count; i++)
		{
			AnimationClipInfo animationClipInfo = AnimationManager.AnimationClipInfos[i];
			if (animationClipInfo.IsLoaded)
			{
				AnimationManager.RemoveAnimationClipInfos.Add(animationClipInfo);
			}
		}
		AnimationManager.UnloadAnimationClipInfos();
	}
	
	private static void UnloadAnimationClipInfos()
	{
		if (AnimationManager.RemoveAnimationClipInfos.Count == 0)
		{
			return;
		}
		for (int i = 0; i < AnimationManager.RemoveAnimationClipInfos.Count; i++)
		{
			AnimationClipInfo animationClipInfo = AnimationManager.RemoveAnimationClipInfos[i];
			AnimationManager.Unload(animationClipInfo.Name);
		}
		AnimationManager.RemoveAnimationClipInfos.Clear();
	}
	
	public static void Update()
	{
		AnimationManager.UnloadTime += Time.deltaTime;
		if (AnimationManager.UnloadTime > 1f)
		{
			AnimationManager.UnloadTime = 0f;
			for (int i = 0; i < AnimationManager.AnimationClipInfos.Count; i++)
			{
				AnimationClipInfo animationClipInfo = AnimationManager.AnimationClipInfos[i];
				if (animationClipInfo.IsLoaded && animationClipInfo.CanUnload)
				{
					AnimationManager.RemoveAnimationClipInfos.Add(animationClipInfo);
				}
			}
			AnimationManager.UnloadAnimationClipInfos();
		}
	}
}


