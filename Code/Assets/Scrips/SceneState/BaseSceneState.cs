
using System;
using Soulgame.StateManagement;

public abstract class BaseSceneState : StateManager<BaseSceneState, SceneAction>.State
{
	public SceneStateManager SceneStateManager
	{
		get;
		private set;
	}
	
	public BaseSceneState(SceneStateManager stateManager) : base(stateManager)
	{
		this.SceneStateManager = stateManager;
	}
}


