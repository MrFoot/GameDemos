using System;
using System.Collections.Generic;
using FootStudio.Util;
using UnityEngine;
using FootStudio.StateManagement;
using UnityEngine.SceneManagement;

public class SceneStateManager : StateManager<BaseGameSceneState, GameAction>
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

	public static bool BlockUpdatesOnStart = false;

	public bool IgnoreLevelLoad;

	#region State

    //剧情
	public StoryState StoryState
	{
		get;
		private set;
	}

    //水族馆
	public AquariumState AquariumState
	{
		get;
		private set;
	}

    //渔场
	public FisheriesState FisheriesState
	{
		get;
		private set;
	}

    //商城
	public ShopState ShopState
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

	public static SceneStateManager Instance
	{
		get
		{
			return Main.Instance.SceneStateManager;
		}
	}

	public SceneStateManager()
	{
		this.StoryState = new StoryState(this);
		this.AquariumState = new AquariumState(this);
		this.FisheriesState = new FisheriesState(this);
		this.ShopState = new ShopState(this);
	}
	
	public void Init()
	{
		this.EnterInitialState();
	}
	
	public override void OnUpdate()
	{
		if (SceneStateManager.BlockUpdatesOnStart)
		{
			return;
		}
		base.OnUpdate();
	}

	protected Pair<BaseGameSceneState, object> GetEntryStateAndData()
	{
		BaseGameSceneState baseGameState = null;
		Level.LevelEnum @int = (Level.LevelEnum)UserPrefs.GetInt(SceneStateManager.LastStateKey, 0);
		switch (@int)
		{
		case Level.LevelEnum.Story:
			this.PreviousSessionRoomState = this.StoryState;
			break;
        case Level.LevelEnum.Aquarium:
			this.PreviousSessionRoomState = this.AquariumState;
			break;
        case Level.LevelEnum.Fisheries:
			this.PreviousSessionRoomState = this.FisheriesState;
			break;
		case Level.LevelEnum.Shop:
			this.PreviousSessionRoomState = this.ShopState;
			break;
		default:
			throw new InvalidOperationException("Unknown lastLoadedLevel: " + @int);
		}
		baseGameState = (baseGameState ?? this.PreviousSessionRoomState);
		if (!Main.Instance.MainGameLogic.IsBedroomLightOn)
		{
			baseGameState = this.ShopState;
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
		UserPrefs.Remove(SceneStateManager.LastStateKey);
	}

	protected override void OnStateChanged()
	{
		base.OnStateChanged();
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
		UserPrefs.SetInt(SceneStateManager.LastStateKey, (int)this.LastSavedState);
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
			//base.StartStateChange();
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


