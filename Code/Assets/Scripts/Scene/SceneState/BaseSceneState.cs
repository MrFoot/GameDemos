using System;
using FootStudio.StateManagement;
using FootStudio.Framework;
using FootStudio.Util;
using UnityEngine;

public abstract class BaseSceneState : BaseState<SceneAction>
{
	public abstract Level LevelEnum
	{
		get;
	}

    public SceneStateManager SceneStateManager
	{
		get;
		private set;
	}
	
	protected BaseSceneState(SceneStateManager stateManager) : base(stateManager)
	{
        this.SceneStateManager = stateManager;
	}

	public BaseSceneController BaseSceneController {
		get;
		set;
	}

    public override void OnPreExit(BaseState<SceneAction> nextState, object data)
    {
        base.OnPreExit(nextState, data);
    }

    public override BaseState<SceneAction> OnPreEnter(BaseState<SceneAction> previousState, object data)
    {
        base.OnPreEnter(previousState, data);
        return this;
    }

    public override void OnEnter(BaseState<SceneAction> previousState, object data)
	{
        GameLog.Debug(Tag + " OnEnter");
        if (this.BaseSceneController != null)
        {
            this.BaseSceneController.OnEnter();
        }
	}

    public override void OnExit(BaseState<SceneAction> nextState, object data)
	{
        GameLog.Debug(Tag + " OnExit");
        if (this.BaseSceneController != null)
        {
            this.BaseSceneController.OnExit(nextState == null);
        }
	}
		
    //这里处理一些在各个场景都要用到的事件，如 弹出警告框，货币更新等
	public override void OnAction (SceneAction gameAction, object data)
	{
        //GameLog.Debug("current State : " + Tag + " OnAction " + gameAction);
        SceneStateManager mgr = this.SceneStateManager as SceneStateManager;
        switch (gameAction)
        {
            case SceneAction.Tips:
                break;
            case SceneAction.BackButton:
                this.ChangeState(mgr.InitialState);
                break;
            default:
                GameLog.Debug(Tag + ":" + gameAction + " No Match");
                break;
        }
	}

	public override void OnAppResume ()
	{
		base.OnAppResume ();
        if (this.BaseSceneController != null)
        {
            this.BaseSceneController.OnAppResume();
        }
	}

	public override void OnAppPause ()
	{
		base.OnAppPause ();
        if (this.BaseSceneController != null)
        {
            this.BaseSceneController.OnAppPause();
        }
	}

	public override void OnUpdate ()
	{
		base.OnUpdate ();
        if (this.BaseSceneController != null)
        {
            this.BaseSceneController.OnUpdate();
        }
	}



}


