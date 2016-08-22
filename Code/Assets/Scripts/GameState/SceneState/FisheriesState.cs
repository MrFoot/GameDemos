using System;

public class FisheriesState : BaseGameSceneState
{
	public FisheriesState(GameSceneStateManager stateManager) : base(stateManager)
	{
		this.BaseSceneController = new BathroomSceneController ();
	}

	public override Level LevelEnum
	{
		get
		{
			return Level.Fisheries;
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


