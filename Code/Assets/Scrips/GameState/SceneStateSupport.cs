
using System;

public abstract class SceneStateSupport : BaseGameState
{
	/// <summary>
	/// 对应的场景状态
	/// </summary>
	/// <value>The state of the corresponding scene.</value>
	public abstract BaseSceneState CorrespondingSceneState
	{
		get;
	}
	
	public SceneStateSupport(MtaGameStateManager stateManager) : base(stateManager)
	{
	}

	/// <summary>
	/// 进入状态前改变场景状态
	/// </summary>
	/// <param name="previousState">Previous state.</param>
	/// <param name="data">Data.</param>
	public override BaseGameState OnPreEnter(BaseGameState previousState, object data)
	{
		Main.Instance.SceneStateManager.ChangeStateInSyncWithMainStateManager(this.CorrespondingSceneState);
		return this;
	}
	
	public override void OnEnter(BaseGameState previousState, object data)
	{
		base.OnEnter(previousState, data);
	}
	
	public override void OnPreExit(BaseGameState nextState, object data)
	{
		SceneStateSupport sceneStateSupport = nextState as SceneStateSupport;
		Main.Instance.SceneStateManager.ChangeStateInSyncWithMainStateManager((sceneStateSupport != null) ? sceneStateSupport.CorrespondingSceneState : null);
	}
	
	public override void OnExit(BaseGameState nextState, object data)
	{
	}
}


