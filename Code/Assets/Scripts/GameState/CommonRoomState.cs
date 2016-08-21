
using System;

public abstract class CommonRoomState : BaseGameState
{
	public CommonRoomState(GameStateManager stateManager) : base(stateManager)
	{
	}

	public abstract CommonEditRoomState EditState();

	public override void OnPreExit(BaseGameState nextState, object data)
	{
		base.OnPreExit(nextState, data);
	}

	public override BaseGameState OnPreEnter(BaseGameState previousState, object data)
	{
		base.OnPreEnter(previousState, data);
		if (previousState != null)
		{
			MainUIController.Instance.OnCommonRoomStatePreEnter(this);
		}
		return this;
	}

	public override void OnEnter(BaseGameState previousState, object data)
	{
		base.OnEnter (previousState, data);
		//Main.Instance.BearCharacter.OnGameStateEnter (this.LevelEnum);
		if (previousState == null)
		{
			MainUIController.Instance.OnCommonRoomStatePreEnter(this);
		}
		MainUIController.Instance.OnCommonRoomStateEnter();
	}

	public override void OnExit(BaseGameState nextState, object data)
	{
		base.OnExit (nextState, data);
		if (!(nextState is CommonRoomState))
		{
			MainUIController.Instance.OnCommonRoomStateExit();
		}
	}

	public override void OnAction(GameAction gameAction, object data)
	{
		switch (gameAction) {
		case GameAction.BackButton:
			this.ChangeState (null);
			return;
		case GameAction.OpenWardrobe:
			this.ChangeState(base.GameStateManager.WardrobeState);
			return;
		case GameAction.OpenKitchen:
			this.ChangeState (base.GameStateManager.KitchenState);
			return;
		case GameAction.OpenBathroom:
			this.ChangeState (base.GameStateManager.BathroomState);
			return;
		case GameAction.OpenBedroom:
			this.ChangeState (base.GameStateManager.BedroomState);
			return;
		case GameAction.OpenTerrace:
			this.ChangeState (base.GameStateManager.TerraceState);
			return;
		default:
			base.OnAction (gameAction, data);
			break;
		}
	}

	public override void OnUpdate()
	{
		base.OnUpdate ();
	}
		
}


