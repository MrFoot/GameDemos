using System;


public class PreviousButtonController : GameActionButtonController
{
	protected override GameAction Action
	{
		get
		{
			return GameAction.Previous;
		}
	}
}


