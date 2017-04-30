using System;
using UnityEngine;

public class Game1State : BaseSceneState
{
    public Game1State(SceneStateManager stateManager)
        : base(stateManager)
    {
        this.BaseSceneController = new Game1SceneController();
    }

    public override Level LevelEnum
    {
        get
        {
            return Level.Game1;
        }
    }

    public override void OnEnter(FootStudio.Framework.BaseState<SceneAction> previousState, object data)
    {
        base.OnEnter(previousState, data);
    }

    public override void OnAction(SceneAction gameAction, object data)
    {
        switch (gameAction)
        {

            default:
                base.OnAction(gameAction, data);
                break;
        }
    }
}


