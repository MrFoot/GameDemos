
using System;

public class TerraceSceneState : CommonSceneState
{
	public TerraceSceneState(SceneStateManager sceneStateManager) : base(sceneStateManager)
	{
		base.BearSceneController = new BearBathroomController();
	}
}


