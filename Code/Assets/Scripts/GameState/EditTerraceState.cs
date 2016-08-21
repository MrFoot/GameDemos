using System;


public class EditTerraceState : CommonEditRoomState
{
	public override Level LevelEnum
	{
		get
		{
			return Level.EditTerrace;
		}
	}

	protected override Room Room
	{
		get
		{
			return Room.Terrace;
		}
	}

	public EditTerraceState(GameStateManager stateManager) : base(stateManager)
	{
	}

	public override void OnEnter(BaseGameState previousState, object data)
	{
		base.OnEnter(previousState, data);
	}

	public override void OnPreExit(BaseGameState nextState, object data)
	{
		base.OnPreExit(nextState, data);
	}

	public override void OnExit(BaseGameState nextState, object data)
	{
		base.OnExit(nextState, data);
	}
}


