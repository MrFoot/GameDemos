using System;
using UnityEngine;

public class StoryState : BaseSceneState
{
	public StoryState(SceneStateManager stateManager) : base(stateManager)
	{
		this.BaseSceneController = new StorySceneController ();
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


