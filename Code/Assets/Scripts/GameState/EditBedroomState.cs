using System;


public class EditBedroomState : CommonEditRoomState
{
	public override Level LevelEnum
	{
		get
		{
			return Level.EditBedroom;
		}
	}

	protected override Room Room
	{
		get
		{
			return Room.Bedroom;
		}
	}

	public EditBedroomState(GameStateManager stateManager) : base(stateManager)
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


