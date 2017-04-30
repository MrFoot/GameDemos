using System;
using UnityEngine;

public class TestState : BaseSceneState
{
	public TestState(SceneStateManager stateManager) : base(stateManager)
	{
		this.BaseSceneController = new TestSceneController();
	}

	public override Level LevelEnum
	{
		get
		{
			return Level.Test;
		}
	}

    public override void OnEnter(FootStudio.Framework.BaseState<SceneAction> previousState, object data)
    {
        base.OnEnter(previousState, data);
    }

	public override void OnAction(SceneAction gameAction, object data)
	{
		switch (gameAction) {

		    default:
			    base.OnAction (gameAction, data);
			    break;
		}
	}
}


