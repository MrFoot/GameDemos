using System;
using UnityEngine;

public class InitialState : BaseSceneState
{
    public InitialState(SceneStateManager stateManager)
        : base(stateManager)
	{
		this.BaseSceneController = new TestSceneController ();
	}

	public override Level LevelEnum
	{
		get
		{
			return Level.Initial;
		}
	}

    public override void OnEnter(FootStudio.Framework.BaseState<SceneAction> previousState, object data)
    {
        base.OnEnter(previousState, data);
    }

	public override void OnAction(SceneAction gameAction, object data)
	{
        SceneStateManager mgr = this.SceneStateManager as SceneStateManager;
		switch (gameAction) {
            case SceneAction.ToTest:
                this.ChangeState(mgr.TestState);
                break;
            case SceneAction.ToGame_1:
                this.ChangeState(mgr.Game1State);
                break;
            case SceneAction.BackButton:
                this.ChangeState(null);
                break;
		    default:
			    base.OnAction (gameAction, data);
			    break;
		}
	}
}


