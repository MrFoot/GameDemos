
using System;

public class TerraceState : CommonRoomState
{
	public TerraceState(GameStateManager stateManager) : base(stateManager)
	{
		this.BearSceneController = new TerraceSceneController ();
	}

	public override Level LevelEnum
	{
		get
		{
			return Level.Terrace;
		}
	}

	public override CommonEditRoomState EditState()
	{
		return base.GameStateManager.EditTerraceState;
	}

	public override void OnAction(GameAction gameAction, object data)
	{
		switch (gameAction) {
		case GameAction.OpenFarmGame:
			this.ChangeState (base.GameStateManager.FarmState);
			break;
		default:
			base.OnAction (gameAction, data);
			break;
		}
	}
}


