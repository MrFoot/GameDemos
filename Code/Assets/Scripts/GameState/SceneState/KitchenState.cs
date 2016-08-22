using UnityEngine;
using System.Collections;

public class KitchenState : BaseGameSceneState
{

	public KitchenState(GameSceneStateManager stateManager) : base(stateManager)
	{
		this.BaseSceneController = new KitchenSceneController ();
	}

	public override Level LevelEnum
	{
		get
		{
			return Level.Kitchen;
		}
	}

}
