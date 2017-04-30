using System;
using UnityEngine;
using FootStudio.Framework;

public class Game1SceneController : BaseSceneController
{
    public Game1SceneController()
	{

	}

    void Awake()
    {
        GameLog.Debug(Tag + " Awake");
        if (Main.Instance != null)
        {
            Main.Instance.SceneStateManager.Game1State.BaseSceneController = this;
        }
    }
}


