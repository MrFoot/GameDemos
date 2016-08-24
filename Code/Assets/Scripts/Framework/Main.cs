using UnityEngine;
using System.Collections;
using Soulgame.Event;
using Soulgame.Util;
using System.Collections.Generic;
using Soulgame.StateManagement;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

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

    public GameSceneStateManager GameStateManager
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
        this.GameStateManager = new GameSceneStateManager();
		this.SceneTouchController = new SceneTouchController();
        this.UserManager = new UserManager();
        /*
         * 实例化各个 Controllers
         * */
	}

	private void ApplyProperties()
	{
		this.AppSession.EventBus = this.EventBus;

        SceneManager.sceneLoaded += OnLevelLoaded;
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
        CharacterFactory.Init();
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
		GameSceneStateManager.ClearPrefs();
		MainGameLogic.ClearPrefs();
		UserManager.ClearPrefs();
		UserPrefs.Save ();
	}

	protected void LateUpdate()
	{
		StateManager.AfterUpdate();
	}

    protected void  OnLevelLoaded(Scene s, LoadSceneMode m) {
        Debug.Log("Scene Name = " + s.name + " | " + "LoadSceneMode = " + m);

        if (!this.IgnoreLevelLoadedNames.Contains(s.name))
        {
            this.GameStateManager.OnLevelWasLoaded(s.buildIndex);
        }
    }

}
