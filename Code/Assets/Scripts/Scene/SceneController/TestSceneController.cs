using System;
using UnityEngine;
using FootStudio.Framework;

public class TestSceneController : BaseSceneController
{
	public TestSceneController ()
	{

	}

    void Awake()
    {
        GameLog.Debug(Tag + " Awake");
        if (Main.Instance != null)
        {
            Main.Instance.SceneStateManager.TestState.BaseSceneController = this;
        }
    }
}


