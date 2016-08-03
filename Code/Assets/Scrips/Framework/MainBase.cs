using UnityEngine;
using System.Collections;
using Soulgame.Threading;
using Soulgame.Asset;

public abstract class MainBase : MonoBehaviour {

	protected const string Tag = "Main";

	private bool Started;

	public MainThread MainThread {
		get;
		private set;
	}

	public MainGameLogic MainGameLogic
	{
		get;
		private set;
	}

	protected virtual void Awake() {
		DontDestroyOnLoad (this);
		this.MainThread = gameObject.AddComponent<MainThread> ();
		this.MainGameLogic = base.gameObject.AddComponent<MainGameLogic>();
		AssetManager.Initialize();
		Application.targetFrameRate = 60;
	}

	// Use this for initialization
	void Start () {
		this.Started = true;
		this.OnAppStart ();
	}
	
	// Update is called once per frame
	protected virtual void Update () {
		bool flag = true;
		if (flag && Input.GetKeyUp(KeyCode.Escape))
		{
			this.OnBackPress();
		}
	}

	private void OnApplicationPause(bool gamePaused)
	{
		if (!this.Started)
		{
			return;
		}
		if (gamePaused)
		{
			this.OnAppPause(false);
		}
		else
		{
			this.OnAppResume();
		}
	}

	public void QuitApp() {
		this.OnAppPause (true);
	}

	protected abstract void OnBackPress();
	
	protected abstract void OnAppStart();
	
	protected abstract void OnAppPause(bool quitting);
	
	protected abstract void OnAppResume();
}
