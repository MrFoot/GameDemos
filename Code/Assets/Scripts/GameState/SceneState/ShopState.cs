using UnityEngine;
using System.Collections;

public class ShopState : BaseGameSceneState
{

	public ShopState(GameSceneStateManager stateManager) : base(stateManager)
	{
		this.BaseSceneController = new BedroomSceneController ();
	}

	public override Level LevelEnum
	{
		get
		{
			return Level.Shop;
		}
	}

}
