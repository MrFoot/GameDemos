using UnityEngine;
using System.Collections;
using Soulgame.StateManagement;

/// <summary>
/// 场景中的状态，比如不同场景下可以执行的动作，比如摸头等
/// </summary>
public class SceneStateManager : StateManager<BaseSceneState, SceneAction> {

	protected override string Tag
	{
		get
		{
			return "SceneStateManager";
		}
	}

	public BathroomSceneState BathroomSceneState
	{
		get;
		private set;
	}
	
	public BedroomSceneState BedroomSceneState
	{
		get;
		private set;
	}
	
	public TerraceSceneState TerraceSceneState
	{
		get;
		private set;
	}
	
	public KitchenSceneState KitchenSceneState
	{
		get;
		private set;
	}

	public SceneStateManager()
	{
		this.BathroomSceneState = new BathroomSceneState(this);
		this.BedroomSceneState = new BedroomSceneState(this);
		this.TerraceSceneState = new TerraceSceneState(this);
		this.KitchenSceneState = new KitchenSceneState(this);

		MtaGameStateManager Mta = Main.Instance.GameStateManager;
		Mta.OnStateEnter += new StateManager<BaseGameState, GameAction>.StateChangeEvent(this.OnGameStateEnter);
		Mta.OnStateExit += new StateManager<BaseGameState, GameAction>.StateChangeEvent(this.OnGameStateExit);
		Mta.OnStatePreEnter += new StateManager<BaseGameState, GameAction>.StateChangeEvent(this.OnGameStatePreEnter);
		Mta.OnStatePreExit +=  new StateManager<BaseGameState, GameAction>.StateChangeEvent(this.OnGameStatePreExit);
	}

	public void Init()
	{
	}

	protected override void ToNullState()
	{
	}
	
	public override void OnUpdate()
	{
		if (base.CurrentState != null)
		{
			base.CurrentState.OnUpdate();
		}
	}

	/// <summary>
	/// 根据游戏状态改变场景状态
	/// </summary>
	/// <param name="state">State.</param>
	public void ChangeStateInSyncWithMainStateManager(BaseSceneState state)
	{
		base.NextState = state;
	}
	
	private void OnGameStatePreExit(BaseGameState gameState, object data)
	{
		if (base.CurrentState == null)
		{
			return;
		}
		base.CurrentState.OnPreExit(base.NextState, data);
	}
	
	private void OnGameStatePreEnter(BaseGameState gameState, object data)
	{
		if (base.NextState == null)
		{
			return;
		}
		base.NextState.OnPreEnter(base.CurrentState, data);
	}
	
	private void OnGameStateExit(BaseGameState gameState, object data)
	{
		if (base.CurrentState == null)
		{
			return;
		}
		base.CurrentState.OnExit(base.NextState, data);
	}
	
	private void OnGameStateEnter(BaseGameState gameState, object data)
	{
		if (base.NextState == null)
		{
			return;
		}
		base.PreviousState = base.CurrentState;
		base.CurrentState = base.NextState;
		base.CurrentState.OnEnter(base.PreviousState, data);
	}
}
