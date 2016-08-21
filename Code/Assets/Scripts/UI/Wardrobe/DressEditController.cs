using UnityEngine;
using System.Collections;

public class DressEditController : GameActionButtonController {

	protected override GameAction Action {
		get {
			return GameAction.SelectCategory;
		}
	}

	public override object GetActionData ()
	{
        return null;
	}
}
