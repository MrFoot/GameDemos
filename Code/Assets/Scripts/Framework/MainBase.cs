using UnityEngine;
using System.Collections;
using FootStudio.Threading;

public abstract class MainBase : MonoBehaviour {

	protected const string Tag = "Main";

	private bool Started;

	protected virtual void Awake() {
		DontDestroyOnLoad (this);
		
		Application.targetFrameRate = 60;
        DeclearPlatform();
	}

    void DeclearPlatform()
    {
        string platform = "111";
#if UNITY_EDITOR
        platform = "unity编辑模式";
#elif UNITY_XBOX360  
       platform="XBOX360平台";  
#elif UNITY_IPHONE  
       platform="IPHONE平台";  
#elif UNITY_ANDROID  
       platform="ANDROID平台";  
#elif UNITY_STANDALONE_OSX  
       platform="OSX平台";  
#elif UNITY_STANDALONE_WIN  
       platform="Windows平台";  
#else
        platform="其他平台";  
#endif
        Debug.LogError("当前平台 ： " + platform);
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
