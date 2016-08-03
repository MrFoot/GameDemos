
using System;


public class BathroomSceneState : CommonSceneState
{
	public BathroomSceneState(SceneStateManager sceneStateManager) : base(sceneStateManager)
	{
		base.BearSceneController = new BearBathroomController();
	}
}


