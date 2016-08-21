using System;

public class EditBathroomState : CommonEditRoomState
{
	public override Level LevelEnum
	{
		get
		{
			return Level.EditBathroom;
		}
	}

	protected override Room Room
	{
		get
		{
			return Room.Bathroom;
		}
	}

	public EditBathroomState(GameStateManager stateManager) : base(stateManager)
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


