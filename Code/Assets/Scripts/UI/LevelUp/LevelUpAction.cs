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

	public BaseGameSceneState ReturnToState
	{
		get;
		private set;
	}

	public LevelUpAction(GameAction action, object dataObject, BaseGameSceneState returnToState)
	{
		this.Action = action;
		this.DataObject = dataObject;
		this.ReturnToState = returnToState;
	}
}


