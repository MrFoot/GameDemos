using UnityEngine;
using System.Collections;

public class ShopState : BaseGameSceneState
{

	public ShopState(SceneStateManager stateManager) : base(stateManager)
	{
		this.BaseSceneController = new ShopSceneController ();
	}

	public override Level LevelEnum
	{
		get
		{
			return Level.Shop;
		}
	}

}
