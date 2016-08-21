using UnityEngine;
using System.Collections;

public abstract class ButtonActionController : MonoBehaviour {

	protected enum HandleTouch {
		Down,
		Up
	}

	public static string LastButtonTriggered;

	[SerializeField]
	protected ButtonActionController.HandleTouch HandleTouchEvent;

	[SerializeField]
	protected bool DelayedTouch;

	private float AwakeTime;

	protected virtual void Awake()
	{
		this.AwakeTime = Time.unscaledTime;
	}

	/// <summary>
	/// 是否延时一段时间再可以点击
	/// </summary>
	/// <returns><c>true</c>, if touch was delayed, <c>false</c> otherwise.</returns>
	protected virtual bool DelayTouch()
	{
        return this.DelayedTouch && Time.unscaledTime - this.AwakeTime < 0.5f;
	}

	protected virtual void OnPress(bool pressed)
	{
		if (this.DelayTouch())
		{
			return;
		}
		if (pressed && this.HandleTouchEvent == ButtonActionController.HandleTouch.Down)
		{
			this.HandleOnTouchDown();
		}
	}
		
	protected virtual void OnClick()
	{
		if (this.DelayTouch())
		{
			return;
		}
		if (this.HandleTouchEvent == ButtonActionController.HandleTouch.Up)
		{
			this.HandleOnTouchUp();
		}
	}

	protected abstract void HandleOnTouchDown();

	protected abstract void HandleOnTouchUp();
}
