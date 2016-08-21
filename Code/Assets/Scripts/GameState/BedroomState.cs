using UnityEngine;
using System.Collections;

public class BedroomState : CommonRoomState {

	public BedroomState(GameStateManager stateManager) : base(stateManager)
	{
		this.BearSceneController = new BedroomSceneController ();
	}

	public override Level LevelEnum
	{
		get
		{
			return Level.Bedroom;
		}
	}

	public override CommonEditRoomState EditState()
	{
		return base.GameStateManager.EditBedroomState;
	}
}
