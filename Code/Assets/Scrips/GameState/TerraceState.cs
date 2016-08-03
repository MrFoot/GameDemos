
using System;

public class TerraceState : CommonRoomState
{
	public TerraceState(MtaGameStateManager stateManager) : base(stateManager)
	{
	}

	public override BaseSceneState CorrespondingSceneState
	{
		get
		{
			return Main.Instance.SceneStateManager.TerraceSceneState;
		}
	}

	public override string LevelName
	{
		get
		{
			return Level.Terrace.Name;
		}
	}
}


