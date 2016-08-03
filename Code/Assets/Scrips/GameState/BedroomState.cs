using UnityEngine;
using System.Collections;

public class BedroomState : CommonRoomState {

	public BedroomState(MtaGameStateManager stateManager) : base(stateManager)
	{
	}

	public override BaseSceneState CorrespondingSceneState
	{
		get
		{
			return Main.Instance.SceneStateManager.BedroomSceneState;
		}
	}

	public override string LevelName
	{
		get
		{
			return Level.Bedroom.Name;
		}
	}
}
