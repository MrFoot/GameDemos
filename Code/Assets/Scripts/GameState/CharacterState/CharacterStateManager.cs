using System;
using System.Collections.Generic;
using Soulgame.Util;
using UnityEngine;
using Soulgame.StateManagement;
using UnityEngine.SceneManagement;

public class CharacterStateManager : StateManager<BaseCharacterState, CharacterAction>
{

	/// <summary>
	/// 游戏进入时加载的场景
	/// </summary>
	/// <value>The state of the entry.</value>
	public BaseGameSceneState EntryState
	{
		get;
		protected set;
	}

	protected override string Tag
	{
		get
		{
			return "GameStateManager";
		}
	}

	protected const string LastStateKey = "GameStateManager.LastState";

	private Level.LevelEnum LastSavedState;

	public static bool BlockUpdatesOnStart = true;

	public bool IgnoreLevelLoad;

	#region State


	#endregion
    public CharacterAction LastTriggeredAction
	{
		private get;
		set;
	}

	public BaseGameSceneState PreviousSessionRoomState
	{
		get;
		private set;
	}

	public static GameSceneStateManager Instance
	{
		get
		{
			return Main.Instance.GameStateManager;
		}
	}

    public CharacterStateManager()
	{

	}
	
	public void Init()
	{
		this.EnterInitialState();
	}
	
	public override void OnUpdate()
	{
		if (GameSceneStateManager.BlockUpdatesOnStart)
		{
			return;
		}
		base.OnUpdate();
	}

	protected override bool BlockStateChange(BaseCharacterState newState)
	{
		return newState == null || base.BlockStateChange(newState);
	}

	protected override void OnStateChanged()
	{
		base.OnStateChanged();
        /*
		if (base.CurrentState == this.AquariumState)
		{
            this.LastSavedState = Level.LevelEnum.Aquarium;
		}
		else if (base.CurrentState == this.FisheriesState)
		{
            this.LastSavedState = Level.LevelEnum.Fisheries;
		}
		else if (base.CurrentState == this.ShopState)
		{
			this.LastSavedState = Level.LevelEnum.Shop;
		}
		else if (base.CurrentState == this.StoryState)
		{
			this.LastSavedState = Level.LevelEnum.Story;
		}
         * */
	}
	
	protected override void ToNullState()
	{
		Main.Instance.QuitApp();
	}

	public override void OnAppPause()
	{
		base.OnAppPause();
	}

	public override bool FireAction(CharacterAction characterAction, object data)
	{
        bool flag = !StateManager.ActionTriggeredInUpdate && base.FireAction(characterAction, data); ;
		if (flag)
		{
            this.LastTriggeredAction = characterAction;
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

		base.StartStateChange();
	}
	
	public void EnterInitialState()
	{
        /*
		this.Data = entryStateAndData.Second;
		this.EntryState = entryStateAndData.First;
		Assert.IsTrue(this.EntryState != null, "Entry state must never be null", new object[0]);
		base.NextState = this.EntryState;
		if (base.PreviousState == null)
		{
			base.OnStatePreEnterEvent(base.NextState, base.PreviousState, this.Data);
		}
         * */
	}
}


