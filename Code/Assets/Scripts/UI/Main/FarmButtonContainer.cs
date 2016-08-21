using UnityEngine;
using System.Collections;

public class FarmButtonContainer : GameActionButtonController {

	protected override GameAction Action
	{
		get
		{
			return GameAction.OpenFarmGame;
		}
	}
}
