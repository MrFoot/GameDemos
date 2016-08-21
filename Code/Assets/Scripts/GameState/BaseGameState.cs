
using System;
using Soulgame.StateManagement;

public abstract class BaseGameState : StateManager<BaseGameState, GameAction>.State
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
	
	public GameStateManager GameStateManager
	{
		get;
		private set;
	}
	
	protected BaseGameState(GameStateManager stateManager) : base(stateManager)
	{
		this.GameStateManager = stateManager;
	}

	public BearSceneController BearSceneController {
		get;
		protected set;
	}

	public override void OnEnter(BaseGameState previousState, object data)
	{
		this.BearSceneController.OnEnter ();
	}

	public override void OnExit(BaseGameState nextState, object data)
	{
		this.BearSceneController.OnExit (nextState == null);
	}
		
	public override void OnAction (GameAction gameAction, object data)
	{
		
	}

	public override void OnAppResume ()
	{
		base.OnAppResume ();
		this.BearSceneController.OnAppResume ();
	}

	public override void OnAppPause ()
	{
		base.OnAppPause ();
		this.BearSceneController.OnAppPause ();
	}

	public override void OnUpdate ()
	{
		base.OnUpdate ();
		this.BearSceneController.OnUpdate ();
	}
}


