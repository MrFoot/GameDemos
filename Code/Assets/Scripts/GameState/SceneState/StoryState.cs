using System;

public class StoryState : BaseGameSceneState
{
	public StoryState(GameSceneStateManager stateManager) : base(stateManager)
	{
		this.BaseSceneController = new TerraceSceneController ();
	}

	public override Level LevelEnum
	{
		get
		{
			return Level.Story;
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


