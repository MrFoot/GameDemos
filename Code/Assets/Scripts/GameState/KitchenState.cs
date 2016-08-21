using UnityEngine;
using System.Collections;

public class KitchenState : CommonRoomState {

	public KitchenState(GameStateManager stateManager) : base(stateManager)
	{
		this.BearSceneController = new KitchenSceneController ();
	}

	public override Level LevelEnum
	{
		get
		{
			return Level.Kitchen;
		}
	}

	public override CommonEditRoomState EditState()
	{
		return base.GameStateManager.EditTerraceState;
	}
}
