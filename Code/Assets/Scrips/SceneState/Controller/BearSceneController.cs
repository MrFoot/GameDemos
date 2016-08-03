
using System;
using Soulgame.Util;


public abstract class BearSceneController
{
	private const string TAG = "BearSceneController";

	private int MainLayerIndex;

	public Level.LevelEnum Level;

	public string MainLayerName = string.Empty;

	public bool Dirt = true;

	public virtual void OnEnter()
	{
		this.UpdateAngelaOnEnter();
		this.ResetLookAtTouch();
	}

	public void ResetLookAtTouch()
	{

	}

	protected void UpdateAngelaOnEnter()
	{
		Main.Instance.BearCharacter.OnGameStateEnter(this.Level);
		Main.Instance.BearCharacter.CharacterAnimationEvents.OnEnter();
		this.ResetLookAt();
		this.ResetHeadPoke();
		this.ResetTimers();
		Main.Instance.SceneTouchController.UnregisterTouchLayer(-1);
		Main.Instance.SceneTouchController.RegisterTouchLayer(-1, new Func<TouchRemapper.TouchData, bool>(this.HandleTouch));
		Main.Instance.BearCharacter.BearSlotController.Dirt = this.Dirt;

		Main.Instance.BearCharacter.ForceUpdate();
	}

	public virtual void OnExit(bool unload)
	{

	}

	public virtual void OnAppPause()
	{

	}
	
	public virtual void OnAppResume()
	{

	}

	public virtual void OnUpdate()
	{
		Main.Instance.BearCharacter.OnUpdate();
	}

	private void UpdateMood()
	{
	}

	public void ResetLookAt()
	{

	}

	public void ResetHeadPoke()
	{

	}

	private void ResetTimers()
	{

	}
		

	public virtual bool HandleTouch(TouchRemapper.TouchData touch)
	{
		//this.HandleTouchLookAt(touch);
		return false;
	}

}


