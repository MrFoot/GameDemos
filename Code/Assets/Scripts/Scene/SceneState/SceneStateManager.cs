using System;
using System.Collections.Generic;
using FootStudio.Util;
using UnityEngine;
using FootStudio.StateManagement;
using UnityEngine.SceneManagement;
using FootStudio.Framework;

public class SceneStateManager : BaseStateManager<SceneAction>
{

    protected override string Tag
    {
        get
        {
            return "SceneStateManager";
        }
    }

	public bool blockUpdatesOnStart = false;

	#region State

    //剧情
    public StoryState StoryState
    {
        get;
        private set;
    }

	#endregion

	public SceneStateManager()
	{
		this.StoryState = new StoryState(this);

	}
	
	public override void Init()
	{
        this.EnterInitialState(this.StoryState);
	}
	
	public override void OnUpdate()
	{
        if (blockUpdatesOnStart)
		{
			return;
		}
		base.OnUpdate();
	}

    protected override bool BlockStateChange(BaseState<SceneAction> newState)
	{
		return newState == null || base.BlockStateChange(newState);
	}

	protected override void OnStateChanged()
	{
		base.OnStateChanged();
	}
	
	protected bool CanLoadLevel()
	{
        BaseSceneState Next = base.NextState as BaseSceneState;
        BaseSceneState Current = base.CurrentState as BaseSceneState;
        return (Next.LevelEnum != null && (Current == null || Next.LevelEnum != Current.LevelEnum)) || base.ForceStateReload;
	}

	protected string LoadLevelName()
	{
        BaseSceneState Next = base.NextState as BaseSceneState;
        return Next.LevelEnum.Name;
	}

	protected override void ToNullState()
	{
		Main.Instance.QuitApp();
	}

	public override void OnAppPause()
	{
		base.OnAppPause();
	}

	public bool OnBackPress()
	{
		return base.FireAction(SceneAction.BackButton);
	}

	public override bool FireAction(SceneAction gameAction, object data)
	{
		bool flag = !StateManager.ActionTriggeredInUpdate && base.FireAction(gameAction, data);;
		if (flag)
		{
			this.LastTriggeredAction = gameAction;
		}
		return flag;
	}

	public void OnLevelWasLoaded(int lvl)
	{
		this.OnStateChanged();
	}
	
	protected override void StartStateChange()
	{
		StateManager.StateChanging = true;
		StateManager.StateChangedInternal = true;
		if (this.CanLoadLevel())
		{
            SceneManager.LoadScene(this.LoadLevelName());
		}
		else
		{
			base.StartStateChange();
		}
	}
	
	public void EnterInitialState(BaseState<SceneAction> initialState)
	{
        this.EntryState = initialState;
		Assert.IsTrue(this.EntryState != null, "Entry state must never be null");
		base.NextState = this.EntryState;
		if (base.PreviousState == null)
		{
			base.OnStatePreEnterEvent(base.NextState, base.PreviousState, this.Data);
		}
        StartStateChange();
	}
}


