using UnityEngine;
using System.Collections;

public class FarmState : BaseGameState {

	public override Level LevelEnum
	{
		get
		{
			return Level.Farm;
		}
	}

	public FarmState(GameStateManager stateManager) : base(stateManager)
	{
	}

	public override BaseGameState OnPreEnter(BaseGameState previousState, object data)
	{
		return this;
	}

	public override void OnEnter(BaseGameState previousState, object data)
	{
	}

	public override void OnExit (BaseGameState nextState, object data)
	{
		
	}

	public override void OnAction (GameAction gameAction, object data)
	{

	}
}
