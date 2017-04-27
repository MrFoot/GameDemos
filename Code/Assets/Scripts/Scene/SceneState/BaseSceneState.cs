using System;
using FootStudio.StateManagement;
using FootStudio.Framework;
using FootStudio.Util;

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
		protected set;
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
        Assert.NotNull(this.BaseSceneController, "this.BaseSceneController");
		this.BaseSceneController.OnEnter ();
	}

    public override void OnExit(BaseState<SceneAction> nextState, object data)
	{
		this.BaseSceneController.OnExit (nextState == null);
	}
		
    //这里处理一些在各个场景都要用到的事件，如 弹出警告框，货币更新等
	public override void OnAction (SceneAction gameAction, object data)
	{
        switch (gameAction)
        {
            case SceneAction.Tips:
                return;
            default:
                
                break;
        }
	}

	public override void OnAppResume ()
	{
		base.OnAppResume ();
		this.BaseSceneController.OnAppResume ();
	}

	public override void OnAppPause ()
	{
		base.OnAppPause ();
		this.BaseSceneController.OnAppPause ();
	}

	public override void OnUpdate ()
	{
		base.OnUpdate ();
		this.BaseSceneController.OnUpdate ();
	}



}


