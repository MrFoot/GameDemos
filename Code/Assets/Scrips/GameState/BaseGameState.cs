
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
	
	public abstract string LevelName
	{
		get;
	}
	
	public MtaGameStateManager GameStateManager
	{
		get;
		private set;
	}
	
	protected BaseGameState(MtaGameStateManager stateManager) : base(stateManager)
	{
		this.GameStateManager = stateManager;
	}

	public override void OnEnter(BaseGameState previousState, object data)
	{

	}
	
	public override void OnAppResume()
	{
		base.OnAppResume();
	}
}


