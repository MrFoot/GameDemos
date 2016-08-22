using System;

public class TerraceState : BaseGameSceneState
{
	public TerraceState(GameSceneStateManager stateManager) : base(stateManager)
	{
		this.BaseSceneController = new TerraceSceneController ();
	}

	public override Level LevelEnum
	{
		get
		{
			return Level.Terrace;
		}
	}

	public override void OnAction(GameAction gameAction, object data)
	{
		switch (gameAction) {

		    default:
			    base.OnAction (gameAction, data);
			    break;
		}
	}
}


