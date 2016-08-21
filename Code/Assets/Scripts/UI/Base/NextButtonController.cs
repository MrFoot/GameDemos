using System;


public class NextButtonController : GameActionButtonController
{
	protected override GameAction Action
	{
		get
		{
			return GameAction.Next;
		}
	}
}


