using UnityEngine;
using System.Collections;

public abstract class GameActionButtonController : ButtonActionController {

	public UnityEngine.Object ActionData;

	protected abstract GameAction Action
	{
		get;
	}

	public virtual object GetActionData()
	{
		if (this.ActionData == null)
		{
			return null;
		}
		return this.ActionData;
	}

	protected override void HandleOnTouchDown()
	{
		string lastButtonTriggered = ButtonActionController.LastButtonTriggered;
		ButtonActionController.LastButtonTriggered = base.gameObject.name;
		if (!Main.Instance.GameStateManager.FireAction(this.Action, this.GetActionData()))
		{
			ButtonActionController.LastButtonTriggered = lastButtonTriggered;
		}
	}

	protected override void HandleOnTouchUp()
	{
		string lastButtonTriggered = ButtonActionController.LastButtonTriggered;
		ButtonActionController.LastButtonTriggered = base.gameObject.name;
		if (!Main.Instance.GameStateManager.FireAction(this.Action, this.GetActionData()))
		{
			ButtonActionController.LastButtonTriggered = lastButtonTriggered;
		}
	}
}
