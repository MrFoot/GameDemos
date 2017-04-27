using System;

public class StoryState : BaseSceneState
{
	public StoryState(SceneStateManager stateManager) : base(stateManager)
	{
		this.BaseSceneController = new StorySceneController ();
	}

	public override Level LevelEnum
	{
		get
		{
			return Level.Test;
		}
	}

	public override void OnAction(SceneAction gameAction, object data)
	{
		switch (gameAction) {

		    default:
			    base.OnAction (gameAction, data);
			    break;
		}
	}
}


