using System;
using UnityEngine;
using FootStudio.Framework;

public class InitialSceneController1 : BaseSceneController
{
    public InitialSceneController1()
	{

	}

    void Awake()
    {
        GameLog.Debug(Tag + " Awake");
        if (Main.Instance != null)
        {
            Main.Instance.SceneStateManager.InitialState.BaseSceneController = this;
        }
    }
}


