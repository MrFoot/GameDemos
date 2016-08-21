using System;


public class LevelUpAction
{
	public GameAction Action
	{
		get;
		private set;
	}

	public object DataObject
	{
		get;
		private set;
	}

	public BaseGameState ReturnToState
	{
		get;
		private set;
	}

	public LevelUpAction(GameAction action, object dataObject, BaseGameState returnToState)
	{
		this.Action = action;
		this.DataObject = dataObject;
		this.ReturnToState = returnToState;
	}
}


