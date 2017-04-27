using System;
using UnityEngine;

public class StorySceneController : BaseSceneController
{
	public StorySceneController ()
	{

	}

    void Awake()
    {
        if (Main.Instance != null)
        {
            Main.Instance.SceneStateManager.StoryState.BaseSceneController = this;
        }
    }
}


