
using System;

public class BedroomSceneState : CommonSceneState
{
	public BedroomSceneState(SceneStateManager sceneStateManager) : base(sceneStateManager)
	{
		base.BearSceneController = new BearBedroomController();
	}
}


