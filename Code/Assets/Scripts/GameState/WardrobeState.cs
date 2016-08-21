using System;
using Soulgame.StateManagement;


public class WardrobeState : BaseGameState
{
	private BaseGameState ExitToState;

	public override Level LevelEnum
	{
		get
		{
			return Level.Wardrobe;
		}
	}

	public WardrobeState(GameStateManager stateManager) : base(stateManager)
	{
	}

	public override void OnEnter(BaseGameState previousState, object data)
	{
		MainUIController.Instance.OnWardrobeEnter(previousState);
		this.CheckAndSetPreviousState(previousState, data);
	}

	public override void OnExit (BaseGameState nextState, object data)
	{
		MainUIController.Instance.OnWardrobeExit(nextState);
	}

	public override void OnAction (GameAction gameAction, object data)
	{
		switch (gameAction) {
		case GameAction.SelectCategory:
			return;
		case GameAction.Next:
			return;
		case GameAction.Previous:
			return;
		}
	}

	public void CheckAndSetPreviousState(BaseGameState previousState, object data)
	{
		if (previousState is CommonRoomState)
		{
			this.ExitToState = previousState;
			return;
		}
		if (data is LevelUpAction)
		{
			this.ExitToState = (data as LevelUpAction).ReturnToState;
			return;
		}
		this.ExitToState = base.GameStateManager.TerraceState;
	}

	public override void OnAppResume()
	{
		base.OnAppResume();
	}

	public override void OnAppPause()
	{
		base.OnAppPause();
	}
}


