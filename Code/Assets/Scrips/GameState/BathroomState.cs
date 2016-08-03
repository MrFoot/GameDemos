
using System;

public class BathroomState : CommonRoomState
{
	public BathroomState(MtaGameStateManager stateManager) : base(stateManager)
	{
	}

	public override BaseSceneState CorrespondingSceneState
	{
		get
		{
			return Main.Instance.SceneStateManager.BathroomSceneState;
		}
	}

	public override string LevelName
	{
		get
		{
			return Level.Bathroom.Name;
		}
	}

	public override void OnAction(GameAction gameAction, object data)
	{
		if (gameAction == GameAction.OpenTeethCleaning) {
			//this.ChangeState (base.GameStateManager.TeethCleaningState);
		} else if (gameAction == GameAction.OpenShowerScene) {
			//this.ChangeState (base.GameStateManager.BathingState);
		} else {
			base.OnAction (gameAction, data);
		}
	}
}


