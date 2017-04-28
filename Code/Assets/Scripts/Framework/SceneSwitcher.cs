using System;
using UnityEngine;
using System.Collections;

public static class SceneSwitcher
{
	static public string CurrentSceneName = null;
	static public bool IsLoading = false;
	static private int m_currentSceneID = -1;
	//网络是否加载完成
	public static bool IsNetLoaded = false;

	static public int CurrentSceneID
	{
		get
		{
			if(m_currentSceneID < 0)
			{
				int id = Application.loadedLevel;
				if(!IsLoading) m_currentSceneID = id;
				Debug.Log ("loadedLevel = " + id);
				return id;
			}
			else
			{
				return m_currentSceneID;
			}
		}
	}

	/// <summary>
	/// Switchs to next scene synchronously.
	/// </summary>
	static public void SwitchToNextScene()
	{
        Resources.UnloadUnusedAssets();
		m_currentSceneID = Application.loadedLevel;
		m_currentSceneID++;
		HttpLite.ClearHttpRequest();
		Application.LoadLevel(CurrentSceneID);
		CurrentSceneName = Application.loadedLevelName;
        Resources.UnloadUnusedAssets();
		GC.Collect();
	}

	/// <summary>
	/// Swtichs to previous scene synchronously.
	/// </summary>
	static public void SwtichToPreviousScene()
	{
        Resources.UnloadUnusedAssets();
		m_currentSceneID = Application.loadedLevel;
		m_currentSceneID--;
        HttpLite.ClearHttpRequest();
		Application.LoadLevel(CurrentSceneID);
		CurrentSceneName = Application.loadedLevelName;
        Resources.UnloadUnusedAssets();
		GC.Collect();
	}

	/// <summary>
	/// Switchs to specific scene synchronously by ID.
	/// </summary>
	/// <param name="sceneId">Scene identifier.</param>
	static public void SwitchToScene(int sceneID)
	{
        Resources.UnloadUnusedAssets();
		m_currentSceneID = sceneID;
        HttpLite.ClearHttpRequest();
		Application.LoadLevel(CurrentSceneID);
		CurrentSceneName = Application.loadedLevelName;
        Resources.UnloadUnusedAssets();
        GC.Collect();
	}

	/// <summary>
	/// Switchs to specific scene synchronously by name.
	/// </summary>
	/// <param name="scene">Scene Name.</param>
	static public void SwitchToScene(string sceneName)
	{
        Resources.UnloadUnusedAssets();
        HttpLite.ClearHttpRequest();
		CurrentSceneName = sceneName;
		Application.LoadLevel(sceneName);
		m_currentSceneID = Application.loadedLevel;
        Resources.UnloadUnusedAssets();
		GC.Collect();
	}
	
	/// <summary>
	/// Swich scene asynchronously while showing loading screen.
	/// </summary>
	/// <param name="scene">Scene Name.</param>
	static public void SwitchToSceneAsync(string sceneName)
	{
        //HttpLite.ClearHttpRequest();
		CurrentSceneName = sceneName;
		m_currentSceneID = -1;
		IsLoading = true;

        UnityEngine.Object LoadingWindowAsset = Resources.Load("UI/Loading/LoadingWindow");
        if(LoadingWindowAsset != null)
		{
			GameObject asyncLoadingWindow = GameObject.Instantiate(LoadingWindowAsset) as GameObject;
			asyncLoadingWindow.transform.position = new Vector3(-999, -999, -999);
		}
	}
}
