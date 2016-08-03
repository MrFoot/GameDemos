
using System;
using UnityEngine;
using Soulgame.Asset;
using Soulgame.Util;

[Serializable]
public class AnimationClipInfo
{
	private static string AnimationClipEventExtenstion = ".animevent";
	
	private static string AnimationLocomotionClipExtenstion = ".animlocomotion";
	
	private float LastUsedTime;
	
	private float UsedTimeout;
	
	private int LoadingCounter;
	
	public AssetManager.Asset AnimationClipAsset
	{
		get;
		set;
	}
	
	public AssetManager.Asset AnimationClipEventAsset
	{
		get;
		set;
	}
	
	public AssetManager.Asset AnimationLocomotionClipAsset
	{
		get;
		set;
	}
	
	public string Name
	{
		get;
		private set;
	}
	
	public AnimationClip AnimationClip
	{
		get
		{
			return this.AnimationClipAsset.AssetObject as AnimationClip;
		}
	}
	
	public AnimationClipEvent AnimationClipEvent
	{
		get
		{
			return (this.AnimationClipEventAsset != null) ? (this.AnimationClipEventAsset.AssetObject as AnimationClipEvent) : null;
		}
	}
	
	public AnimationLocomotionClip AnimationLocomotionClip
	{
		get
		{
			return (this.AnimationLocomotionClipAsset != null) ? (this.AnimationLocomotionClipAsset.AssetObject as AnimationLocomotionClip) : null;
		}
	}
	
	public float ooLength
	{
		get;
		private set;
	}
	
	public bool IsLoaded
	{
		get
		{
			return this.AnimationClipAsset != null && this.LoadingCounter == 0;
		}
	}
	
	public bool CanUnload
	{
		get
		{
			return this.LoadingCounter == 0 && Time.time - this.LastUsedTime > this.UsedTimeout;
		}
	}
	
	public AnimationClipInfo(string name, float usedTimeout)
	{
		this.Name = name;
		this.UsedTimeout = usedTimeout;
	}
	
	private void OnAnimationClipAssetLoaded(AssetManager.Asset asset)
	{
		this.AnimationClipAsset = asset;
		this.LoadingCounter--;
		if (asset.AssetObject == null)
		{
			GameLog.Error("Animation clip {0} load failed!", new object[]{
				asset.Name
			});
			return;
		}
		AnimationClip animationClip = this.AnimationClip;
		this.ooLength = ((animationClip.length <= 0f) ? 0f : (1f / animationClip.length));
	}
	
	private void OnAnimationClipEventAssetLoaded(AssetManager.Asset asset)
	{
		this.AnimationClipEventAsset = asset;
		this.LoadingCounter--;
	}
	
	private void OnAnimationLocomotionClipAssetLoaded(AssetManager.Asset asset)
	{
		this.AnimationLocomotionClipAsset = asset;
		this.LoadingCounter--;
	}
	
	public void SetUsed()
	{
		this.LastUsedTime = Time.time;
	}
	
	public void LoadFromResources(string name)
	{
		this.LoadingCounter = 3;
		this.AnimationClipAsset = AssetManager.LoadAssetFromResources(name, typeof(AnimationClip), new AssetManager.OnAssetLoaded(this.OnAnimationClipAssetLoaded));
		this.AnimationClipEventAsset = AssetManager.LoadAssetFromResources(name + AnimationClipInfo.AnimationClipEventExtenstion, typeof(AnimationClipEvent), new AssetManager.OnAssetLoaded(this.OnAnimationClipEventAssetLoaded));
		this.AnimationLocomotionClipAsset = AssetManager.LoadAssetFromResources(name + AnimationClipInfo.AnimationLocomotionClipExtenstion, typeof(AnimationLocomotionClip), new AssetManager.OnAssetLoaded(this.OnAnimationLocomotionClipAssetLoaded));
		this.SetUsed();
	}
	
	public void LoadAsyncFromResources(string name)
	{
		this.LoadingCounter = 3;
		this.AnimationClipAsset = AssetManager.LoadAsyncAssetFromResources(name, typeof(AnimationClip), new AssetManager.OnAssetLoaded(this.OnAnimationClipAssetLoaded));
		this.AnimationClipEventAsset = AssetManager.LoadAsyncAssetFromResources(name + AnimationClipInfo.AnimationClipEventExtenstion, typeof(AnimationClipEvent), new AssetManager.OnAssetLoaded(this.OnAnimationClipEventAssetLoaded));
		this.AnimationLocomotionClipAsset = AssetManager.LoadAsyncAssetFromResources(name + AnimationClipInfo.AnimationLocomotionClipExtenstion, typeof(AnimationLocomotionClip), new AssetManager.OnAssetLoaded(this.OnAnimationLocomotionClipAssetLoaded));
		this.SetUsed();
	}
	
	public void LoadFromEditorResources(string name)
	{
		this.LoadingCounter = 3;
		this.AnimationClipAsset = AssetManager.LoadAssetFromEditorResources(name, typeof(AnimationClip), new AssetManager.OnAssetLoaded(this.OnAnimationClipAssetLoaded));
		this.AnimationClipEventAsset = AssetManager.LoadAssetFromEditorResources(name + AnimationClipInfo.AnimationClipEventExtenstion, typeof(AnimationClipEvent), new AssetManager.OnAssetLoaded(this.OnAnimationClipEventAssetLoaded));
		this.AnimationLocomotionClipAsset = AssetManager.LoadAssetFromEditorResources(name + AnimationClipInfo.AnimationLocomotionClipExtenstion, typeof(AnimationLocomotionClip), new AssetManager.OnAssetLoaded(this.OnAnimationLocomotionClipAssetLoaded));
		this.SetUsed();
	}
	
	public void LoadFromBundle(AssetManager.Bundle bundle, string name)
	{
		this.LoadingCounter = 3;
		this.AnimationClipAsset = AssetManager.LoadAssetFromBundle(bundle, name, typeof(AnimationClip), new AssetManager.OnAssetLoaded(this.OnAnimationClipAssetLoaded));
		this.AnimationClipEventAsset = AssetManager.LoadAssetFromBundle(bundle, name + AnimationClipInfo.AnimationClipEventExtenstion, typeof(AnimationClipEvent), new AssetManager.OnAssetLoaded(this.OnAnimationClipEventAssetLoaded));
		this.AnimationLocomotionClipAsset = AssetManager.LoadAssetFromBundle(bundle, name + AnimationClipInfo.AnimationLocomotionClipExtenstion, typeof(AnimationLocomotionClip), new AssetManager.OnAssetLoaded(this.OnAnimationLocomotionClipAssetLoaded));
		this.SetUsed();
	}
	
	public void LoadAsyncFromBundle(AssetManager.Bundle bundle, string name)
	{
		this.LoadingCounter = 3;
		this.AnimationClipAsset = AssetManager.LoadAsyncAssetFromBundle(bundle, name, typeof(AnimationClip), new AssetManager.OnAssetLoaded(this.OnAnimationClipAssetLoaded));
		this.AnimationClipEventAsset = AssetManager.LoadAsyncAssetFromBundle(bundle, name + AnimationClipInfo.AnimationClipEventExtenstion, typeof(AnimationClipEvent), new AssetManager.OnAssetLoaded(this.OnAnimationClipEventAssetLoaded));
		this.AnimationLocomotionClipAsset = AssetManager.LoadAsyncAssetFromBundle(bundle, name + AnimationClipInfo.AnimationLocomotionClipExtenstion, typeof(AnimationLocomotionClip), new AssetManager.OnAssetLoaded(this.OnAnimationLocomotionClipAssetLoaded));
		this.SetUsed();
	}
	
	public void Unload()
	{
		if (this.AnimationClipAsset != null)
		{
			AssetManager.UnloadAsset(this.AnimationClipAsset);
		}
		if (this.AnimationClipEventAsset != null)
		{
			AssetManager.UnloadAsset(this.AnimationClipEventAsset);
		}
		if (this.AnimationLocomotionClipAsset != null)
		{
			AssetManager.UnloadAsset(this.AnimationLocomotionClipAsset);
		}
		this.AnimationClipAsset = null;
		this.AnimationClipEventAsset = null;
		this.AnimationLocomotionClipAsset = null;
		this.LoadingCounter = 0;
	}
}

