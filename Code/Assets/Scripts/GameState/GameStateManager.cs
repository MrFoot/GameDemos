using System;
using System.Collections.Generic;
using Soulgame.Util;
using UnityEngine;
using Soulgame.StateManagement;
using UnityEngine.SceneManagement;

public class GameStateManager : StateManager<BaseGameState, GameAction>
{

	/// <summary>
	/// 游戏进入时加载的场景
	/// </summary>
	/// <value>The state of the entry.</value>
	public BaseGameState EntryState
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

	public WardrobeState WardrobeState
	{
		get;
		private set;
	}

	public EditKitchenState EditKitchenState
	{
		get;
		private set;
	}

	public EditBathroomState EditBathroomState
	{
		get;
		private set;
	}

	public EditBedroomState EditBedroomState
	{
		get;
		private set;
	}

	public EditTerraceState EditTerraceState
	{
		get;
		private set;
	}

	public FarmState FarmState
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

    //public RoomEditController RoomEditController
    //{
    //    private get;
    //    set;
    //}

	public BaseGameState PreviousSessionRoomState
	{
		get;
		private set;
	}

	public static GameStateManager Instance
	{
		get
		{
			return Main.Instance.GameStateManager;
		}
	}

	public GameStateManager()
	{
		this.TerraceState = new TerraceState(this);
		this.KitchenState = new KitchenState(this);
		this.BathroomState = new BathroomState(this);
		this.BedroomState = new BedroomState(this);
		this.WardrobeState = new WardrobeState(this);
		this.EditKitchenState = new EditKitchenState(this);
		this.EditBathroomState = new EditBathroomState(this);
		this.EditBedroomState = new EditBedroomState(this);
		this.EditTerraceState = new EditTerraceState(this);
		this.FarmState = new FarmState (this);
	}
	
	public void Init()
	{
		this.EnterInitialState();
	}
	
	public override void OnUpdate()
	{
		if (GameStateManager.BlockUpdatesOnStart)
		{
			return;
		}
		base.OnUpdate();
	}

	protected Pair<BaseGameState, object> GetEntryStateAndData()
	{
		BaseGameState baseGameState = null; //this.RoomEditController.GetPendingUpdateItemGameState();
		Level.LevelEnum @int = (Level.LevelEnum)UserPrefs.GetInt(GameStateManager.LastStateKey, 0);
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
		return new Pair<BaseGameState, object>(baseGameState, second);
	}
		
	protected override bool BlockStateChange(BaseGameState newState)
	{
		return newState == null || base.BlockStateChange(newState);
	}

	public static void ClearPrefs()
	{
		UserPrefs.Remove(GameStateManager.LastStateKey);
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
		UserPrefs.SetInt(GameStateManager.LastStateKey, (int)this.LastSavedState);
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
		Pair<BaseGameState, object> entryStateAndData = this.GetEntryStateAndData();
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


