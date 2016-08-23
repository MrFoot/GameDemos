using System;

public class FisheriesState : BaseGameSceneState
{
	public FisheriesState(GameSceneStateManager stateManager) : base(stateManager)
	{
		this.BaseSceneController = new FisheriesSceneController ();
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

	}

}


