using UnityEngine;
using System.Collections;
using Soulgame.Event;
using Soulgame.Util;
using System.Collections.Generic;
using Soulgame.StateManagement;

public class Main : MainBase {

	private HashSet<string> IgnoreLevelLoadedNames;

	public static Main Instance
	{
		get;
		private set;
	}

	public EventBus EventBus
	{
		get;
		private set;
	}

	public AppSession AppSession
	{
		get;
		private set;
	}

    public GameStateManager GameStateManager
	{
		get;
		private set;
	}

	public SceneTouchController SceneTouchController
	{
		get;
		private set;
	}

	public TableManager TableManager {
		get;
		private set;
	}

    public UserManager UserManager { 
        get; 
        private set; 
    }

	protected override void Awake() {
		base.Awake ();
		this.IgnoreLevelLoadedNames = new HashSet<string>();
		this.IgnoreLevelLoadedNames.Add("StartUp");
		this.IgnoreLevelLoadedNames.Add("Main");
		Main.Instance = this;
		this.InstantiateObjects();
		this.ApplyProperties();
		this.InitObjects();
	}

	private void InstantiateObjects()
	{
		this.TableManager = new TableManager ();
		this.EventBus = new EventBus ();
		this.AppSession = new AppSession ();
        this.GameStateManager = new GameStateManager();
		this.SceneTouchController = new SceneTouchController();
        this.UserManager = new UserManager();
        /*
         * 实例化各个 Controllers
         * */
	}

	private void ApplyProperties()
	{
		this.AppSession.EventBus = this.EventBus;

        /*
         * 创建 Controllers
         * */
	}

	private void InitObjects()
	{
		this.AppSession.Init ();
		this.SceneTouchController.Init();
		base.MainGameLogic.Init();
		this.GameStateManager.Init();

		//初始化顺序不能变
		this.TableManager.Init ();
        this.UserManager.Init(); 
	}

	protected override void OnAppStart ()
	{
		base.MainGameLogic.OnAppResume ();
	}

	protected override void OnAppResume ()
	{
		this.AppSession.OnAppResume ();
		this.GameStateManager.OnAppResume();
		base.MainGameLogic.OnAppResume ();
		this.EventBus.FireEvent (EventId.APP_RESUME);
	}

	protected override void OnAppPause (bool quitting)
	{
		this.GameStateManager.OnAppPause();
		this.AppSession.OnAppPause ();
		base.MainGameLogic.OnAppPause();
		this.EventBus.FireEvent (EventId.APP_PAUSE, quitting);
	}

	protected override void OnBackPress ()
	{
		this.GameStateManager.OnBackPress();
	}

	protected override void Update()
	{
		base.Update ();
		this.GameStateManager.OnUpdate();
	}

	public void ClearPrefs()
	{
		AppSession.ClearPrefs ();
		GameStateManager.ClearPrefs();
		MainGameLogic.ClearPrefs();
		UserManager.ClearPrefs();
		UserPrefs.Save ();
	}

	protected void LateUpdate()
	{
		StateManager.AfterUpdate();
	}
	
	protected void OnLevelWasLoaded(int lvl)
	{
		if (!this.IgnoreLevelLoadedNames.Contains(Application.loadedLevelName))
		{
			this.GameStateManager.OnLevelWasLoaded(lvl);
		}
	}

}
