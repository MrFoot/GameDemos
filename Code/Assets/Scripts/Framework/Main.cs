using UnityEngine;
using System.Collections;
using FootStudio.Framework;
using FootStudio.Util;
using System.Collections.Generic;
using FootStudio.StateManagement;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class Main : MainBase {

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

    public SceneStateManager SceneStateManager
	{
		get;
		private set;
	}

	public TableManager TableManager {
		get;
		private set;
	}

	protected override void Awake() {
		base.Awake ();

		Main.Instance = this;
		this.InstantiateObjects();
		this.ApplyProperties();
		this.InitObjects();

        GameLog.Level = GameLog.LogLevel.ERROR;
	}

	private void InstantiateObjects()
	{
        AssetManager.Initialize();

		this.TableManager = new TableManager ();
		this.EventBus = new EventBus ();
		this.AppSession = new AppSession ();
        this.SceneStateManager = new SceneStateManager();

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
		this.SceneStateManager.Init();

		//初始化顺序不能变
		this.TableManager.Init ();
	}

	protected override void OnAppStart ()
	{
	}

	protected override void OnAppResume ()
	{
		this.AppSession.OnAppResume ();
		this.SceneStateManager.OnAppResume();
		this.EventBus.FireEvent (EventId.APP_RESUME);
	}

	protected override void OnAppPause (bool quitting)
	{
		this.SceneStateManager.OnAppPause();
		this.AppSession.OnAppPause ();
		this.EventBus.FireEvent (EventId.APP_PAUSE, quitting);
	}

	protected override void OnBackPress ()
	{
		this.SceneStateManager.OnBackPress();
	}

	protected override void Update()
	{
		base.Update ();
		this.SceneStateManager.OnUpdate();
	}

	public void ClearPrefs()
	{
		AppSession.ClearPrefs ();
		UserPrefs.Save ();
	}

	protected void LateUpdate()
	{
		StateManager.AfterUpdate();
	}

    protected void  OnLevelLoaded(Scene s, LoadSceneMode m) {
        Debug.Log("Scene Name = " + s.name + " | " + "LoadSceneMode = " + m);

        this.SceneStateManager.OnLevelWasLoaded(s.buildIndex);
    }

}
