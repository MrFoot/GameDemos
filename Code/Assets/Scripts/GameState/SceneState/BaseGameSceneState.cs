using System;
using FootStudio.StateManagement;

public abstract class BaseGameSceneState : StateManager<BaseGameSceneState, GameAction>.State
{
	public virtual string Tag
	{
		get
		{
			return base.GetType().Name;
		}
	}
	
	public abstract Level LevelEnum
	{
		get;
	}
	
	public SceneStateManager GameStateManager
	{
		get;
		private set;
	}
	
	protected BaseGameSceneState(SceneStateManager stateManager) : base(stateManager)
	{
		this.GameStateManager = stateManager;
	}

	public BaseSceneController BaseSceneController {
		get;
		protected set;
	}

    public override void OnPreExit(BaseGameSceneState nextState, object data) {
        base.OnPreExit(nextState, data);
        if (nextState != null)
        {
            MainUIController.Instance.OnSceneStatePreExit(this);
        }

    }

    public override BaseGameSceneState OnPreEnter(BaseGameSceneState previousState, object data) {
        base.OnPreEnter(previousState, data);
        if (previousState != null)
        {
            MainUIController.Instance.OnSceneStatePreEnter(this);
        }
        return this;
    }

	public override void OnEnter(BaseGameSceneState previousState, object data)
	{
		this.BaseSceneController.OnEnter ();
	}

	public override void OnExit(BaseGameSceneState nextState, object data)
	{
		this.BaseSceneController.OnExit (nextState == null);
	}
		
    //这里处理一些在各个场景都要用到的事件，如 弹出警告框，货币更新等
	public override void OnAction (GameAction gameAction, object data)
	{
        switch (gameAction)
        {
            case GameAction.Tips:
                MainUIController.Instance.ShowTips(data as string);
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


