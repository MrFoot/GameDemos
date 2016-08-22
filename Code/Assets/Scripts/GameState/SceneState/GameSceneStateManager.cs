using System;
using System.Collections.Generic;
using Soulgame.Util;
using UnityEngine;
using Soulgame.StateManagement;
using UnityEngine.SceneManagement;

public class GameSceneStateManager : StateManager<BaseGameSceneState, GameAction>
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
	public TerraceState TerraceState
	{
		get;
		private set;
	}

	public KitchenState KitchenState
	{
		get;
		private set;
	}

	public BathroomState BathroomState
	{
		get;
		private set;
	}

	public BedroomState BedroomState
	{
		get;
		private set;
	}


	#endregion
	public GameAction LastTriggeredAction
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

	public GameSceneStateManager()
	{
		this.TerraceState = new TerraceState(this);
		this.KitchenState = new KitchenState(this);
		this.BathroomState = new BathroomState(this);
		this.BedroomState = new BedroomState(this);
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

	protected Pair<BaseGameSceneState, object> GetEntryStateAndData()
	{
		BaseGameSceneState baseGameState = null;
		Level.LevelEnum @int = (Level.LevelEnum)UserPrefs.GetInt(GameSceneStateManager.LastStateKey, 0);
		switch (@int)
		{
		case Level.LevelEnum.Terrace:
			this.PreviousSessionRoomState = this.TerraceState;
			break;
		case Level.LevelEnum.Kitchen:
			this.PreviousSessionRoomState = this.KitchenState;
			break;
		case Level.LevelEnum.Bathroom:
			this.PreviousSessionRoomState = this.BathroomState;
			break;
		case Level.LevelEnum.Bedroom:
			this.PreviousSessionRoomState = this.BedroomState;
			break;
		default:
			throw new InvalidOperationException("Unknown lastLoadedLevel: " + @int);
		}
		baseGameState = (baseGameState ?? this.PreviousSessionRoomState);
		if (!Main.Instance.MainGameLogic.IsBedroomLightOn)
		{
			baseGameState = this.BedroomState;
		}
		object second = this;
		return new Pair<BaseGameSceneState, object>(baseGameState, second);
	}
		
	protected override bool BlockStateChange(BaseGameSceneState newState)
	{
		return newState == null || base.BlockStateChange(newState);
	}

	public static void ClearPrefs()
	{
		UserPrefs.Remove(GameSceneStateManager.LastStateKey);
	}

	protected override void OnStateChanged()
	{
		base.OnStateChanged();
		if (base.CurrentState == this.KitchenState)
		{
			this.LastSavedState = Level.LevelEnum.Kitchen;
		}
		else if (base.CurrentState == this.BathroomState)
		{
			this.LastSavedState = Level.LevelEnum.Bathroom;
		}
		else if (base.CurrentState == this.BedroomState)
		{
			this.LastSavedState = Level.LevelEnum.Bedroom;
		}
		else if (base.CurrentState == this.TerraceState)
		{
			this.LastSavedState = Level.LevelEnum.Terrace;
		}
	}
	
	protected bool CanLoadLevel()
	{
		return (base.NextState.LevelEnum != null && (base.CurrentState == null || base.NextState.LevelEnum != base.CurrentState.LevelEnum)) || base.ForceStateReload;
	}

	protected string LoadLevelName()
	{
		return base.NextState.LevelEnum.Name;
	}

	protected override void ToNullState()
	{
		Main.Instance.QuitApp();
	}

	public override void OnAppPause()
	{
		base.OnAppPause();
		UserPrefs.SetInt(GameSceneStateManager.LastStateKey, (int)this.LastSavedState);
	}

	public bool OnBackPress()
	{
		return base.FireAction(GameAction.BackButton);
	}

	public override bool FireAction(GameAction gameAction, object data)
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
	
	public void EnterInitialState()
	{
		Pair<BaseGameSceneState, object> entryStateAndData = this.GetEntryStateAndData();
		this.Data = entryStateAndData.Second;
		this.EntryState = entryStateAndData.First;
		Assert.IsTrue(this.EntryState != null, "Entry state must never be null", new object[0]);
		base.NextState = this.EntryState;
		if (base.PreviousState == null)
		{
			base.OnStatePreEnterEvent(base.NextState, base.PreviousState, this.Data);
		}
	}
}


