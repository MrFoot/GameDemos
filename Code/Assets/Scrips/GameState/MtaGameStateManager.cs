
using System;
using Soulgame.StateManagement;
using System.Collections.Generic;
using Soulgame.Util;

/// <summary>
/// 控制整个游戏的状态，不同状态下可执行的操作不一样
/// </summary>
using UnityEngine;


public class MtaGameStateManager : GameStateManager<BaseGameState, GameAction>
{

	protected override string Tag
	{
		get
		{
			return "MtaGameStateManager";
		}
	}

	protected const string LastStateKey = "GameStateManager.LastState";

	private Level.LevelEnum LastSavedState;

	private List<IAutoOpenState> AutoOpenedCommonRoomStates = new List<IAutoOpenState>();
	
	private List<IAutoOpenState> AutoOpenRestoreStates = new List<IAutoOpenState>();

	public readonly Func<bool> CheckAndOpenCommonRoomStatesFunct;
	
	public readonly Func<bool> CheckAndOpenRestoreStatesFunct;

	public static bool BlockUpdatesOnStart = true;

	public bool IgnoreLevelLoad;

//	public RestoreState RestoreState
//	{
//		get;
//		private set;
//	}
//	
//	public WhatsNewState WhatsNewState
//	{
//		get;
//		private set;
//	}

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

	public GameAction LastTriggeredAction
	{
		private get;
		set;
	}

	public BaseGameState PreviousSessionRoomState
	{
		get;
		private set;
	}

	public static MtaGameStateManager Instance
	{
		get
		{
			return Main.Instance.GameStateManager;
		}
	}

	public MtaGameStateManager()
	{
		this.TerraceState = new TerraceState(this);
		this.KitchenState = new KitchenState(this);
		this.BathroomState = new BathroomState(this);
		this.BedroomState = new BedroomState(this);
		this.CheckAndOpenCommonRoomStatesFunct = new Func<bool>(this.CheckAndOpenCommonRoomStates);
		this.CheckAndOpenRestoreStatesFunct = new Func<bool>(this.CheckAndOpenRestoreStates);
	}

	public override void Init()
	{
		//this.AddAutoOpenRestoreState(this.RestoreState);
		//this.AddAutoOpenCommonRoomState(this.WhatsNewState);
		base.Init ();
	}

	protected override Pair<BaseGameState, object> GetEntryStateAndData()
	{
		BaseGameState baseGameState = null; //this.RoomEditController.GetPendingUpdateItemGameState();
		Level.LevelEnum @int = (Level.LevelEnum)UserPrefs.GetInt(MtaGameStateManager.LastStateKey, 0);
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
//		if (this.TutorialFlowController.MainTutorialInProgress)
//		{
//			baseGameState = this.TutorialStep01StrollerState;
//		}
		object second = this;
		return new Pair<BaseGameState, object>(baseGameState, second);
	}

	public override void OnUpdate()
	{
		if (MtaGameStateManager.BlockUpdatesOnStart)
		{
			return;
		}
		base.OnUpdate();
	}
	
	public static void ClearPrefs()
	{
		UserPrefs.Remove(MtaGameStateManager.LastStateKey);
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

	public override bool CanAutoOpen()
	{
		return base.CanAutoOpen() && !Main.Instance.BearCharacter.NonInterruptible;
	}
	
	protected override bool CanLoadLevel()
	{
		return (base.NextState.LevelName != null && (base.CurrentState == null || base.NextState.LevelName != base.CurrentState.LevelName)) || base.ForceStateReload;
	}
	
	protected override string LoadLevelName()
	{
		return base.NextState.LevelName;
	}
	
	protected override bool BlockStateChange(BaseGameState newState)
	{
		return !this.IgnoreLevelLoad && base.BlockStateChange(newState);
	}
	
	protected override void ToNullState()
	{
		Main.Instance.QuitApp();
	}
	
	public override void OnAppPause()
	{
		base.OnAppPause();
		UserPrefs.SetInt(MtaGameStateManager.LastStateKey, (int)this.LastSavedState);
	}
	
	public override bool OnBackPress()
	{
		return base.FireAction(GameAction.BackButton);
	}
	
	public override bool FireAction(GameAction gameAction, object data)
	{
		bool flag = base.FireAction(gameAction, data);
		if (flag)
		{
			this.LastTriggeredAction = gameAction;
		}
		return flag;
	}
	
	public override void OnLevelWasLoaded(int lvl)
	{
		if (this.IgnoreLevelLoad)
		{
			//Main.Instance.SharedGameStateManager.OnLevelWasLoaded(lvl);
		}
		else
		{
			base.OnLevelWasLoaded(lvl);
		}
	}

	private bool CheckAndOpenCommonRoomStates()
	{
		return base.CheckAndOpenStates(this.AutoOpenedCommonRoomStates);
	}
	
	private bool CheckAndOpenRestoreStates()
	{
		return base.CheckAndOpenStates(this.AutoOpenRestoreStates);
	}

	public void AddAutoOpenCommonRoomState(IAutoOpenState autoOpenState)
	{
		this.AutoOpenedCommonRoomStates.Add(autoOpenState);
	}
	
	public void AddAutoOpenRestoreState(IAutoOpenState autoOpenState)
	{
		this.AutoOpenRestoreStates.Add(autoOpenState);
	}
}


