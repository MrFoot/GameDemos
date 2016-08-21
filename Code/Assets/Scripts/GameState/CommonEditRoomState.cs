using System;


public abstract class CommonEditRoomState : BaseGameState
{
	protected abstract Room Room
	{
		get;
	}

	public CommonEditRoomState(GameStateManager stateManager) : base(stateManager)
	{
	}

	public override void OnAction(GameAction gameAction, object data)
	{

	}

	public override void OnEnter(BaseGameState previousState, object data)
	{

	}

	public override void OnExit(BaseGameState nextState, object data)
	{

	}
}


