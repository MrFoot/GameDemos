using System;
using UnityEngine;

public class StorySceneController : BaseSceneController
{
	public StorySceneController ()
	{

	}

    void Awake()
    {
        Debug.LogError("StorySceneController Awake");
        if (Main.Instance != null)
        {
            Main.Instance.SceneStateManager.StoryState.BaseSceneController = this;
        }
    }
}


