using UnityEngine;
using System.Collections;

public class KitchenState : CommonRoomState {

	public KitchenState(MtaGameStateManager stateManager) : base(stateManager)
	{
	}

	public override BaseSceneState CorrespondingSceneState
	{
		get
		{
			return Main.Instance.SceneStateManager.KitchenSceneState;
		}
	}

	public override string LevelName
	{
		get
		{
			return Level.Kitchen.Name;
		}
	}
}
