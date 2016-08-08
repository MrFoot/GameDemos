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


	public SceneTouchController SceneTouchController
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
		this.SceneTouchController = new SceneTouchController();
	}

	private void ApplyProperties()
	{
		this.AppSession.EventBus = this.EventBus;
	}

	private void InitObjects()
	{
		this.TableManager.Init ();
		this.AppSession.Init ();
		this.SceneTouchController.Init();
		base.MainGameLogic.Init();
	}

	protected override void OnAppStart ()
	{
		base.MainGameLogic.OnAppResume ();
	}

	protected override void OnAppResume ()
	{
		this.AppSession.OnAppResume ();
		base.MainGameLogic.OnAppResume ();
		this.EventBus.FireEvent (EventId.APP_RESUME);
	}

	protected override void OnAppPause (bool quitting)
	{
		this.AppSession.OnAppPause ();
		base.MainGameLogic.OnAppPause();
		this.EventBus.FireEvent (EventId.APP_PAUSE, quitting);
	}

	protected override void OnBackPress ()
	{
	}

	protected override void Update()
	{
		base.Update ();
	}

	public void ClearPrefs()
	{
		AppSession.ClearPrefs ();
		MainGameLogic.ClearPrefs();
		UserPrefs.Save ();
	}

	protected void LateUpdate()
	{
		StateManager.AfterUpdate();
	}

	public void OnCameraPostRender(Camera camera)
	{
		if (StateManager.StateChanging)
		{
		}
	}
}
