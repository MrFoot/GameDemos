using UnityEngine;
using System.Collections;

public class BedroomState : BaseGameSceneState
{

	public BedroomState(GameSceneStateManager stateManager) : base(stateManager)
	{
		this.BaseSceneController = new BedroomSceneController ();
	}

	public override Level LevelEnum
	{
		get
		{
			return Level.Bedroom;
		}
	}

}
