using UnityEngine;
using System.Collections;

public abstract class BearSceneController {

	public Level Level;

	private bool Paused;

	private int LookAtFingerId = -1;

	public Vector3 LookAtPosition {
		get;
		private set;
	}

	public bool LookAround {
		get {
			return LookAtFingerId > -1;
		}
	}

	public virtual void OnEnter() {
		Main.Instance.SceneTouchController.UnregisterTouchLayer (-1);
		Main.Instance.SceneTouchController.RegisterTouchLayer (-1, new System.Func<TouchRemapper.TouchData, bool> (this.HandleTouch));

	}

	public virtual void OnExit(bool unload) {
		if (unload) {
		}
	}

	public virtual void OnAppResume() {
		this.Paused = false;
	}

	public virtual void OnAppPause() {
		this.Paused = true;
	}

	public virtual void OnUpdate() {
	}

	public virtual bool HandleTouch(TouchRemapper.TouchData touch) {
		this.HandleTouchLookAt (touch);
		return false;
	}

	private void HandleTouchLookAt(TouchRemapper.TouchData touch) {
		switch (touch.Phase) {
		case TouchPhase.Began:
			this.LookAtFingerId = touch.FingerId;
			break;
		case TouchPhase.Ended:
			this.LookAtFingerId = -1;
			break;
		case TouchPhase.Moved:
		case TouchPhase.Stationary:
			if (LookAtFingerId >= 0) {
				this.LookAt (touch.Position);
			}
			break;
		default:
			break;
		}
	}

	public void LookAt(Vector3 screenPosition) {
		Vector2 v = screenPosition;
		Ray ray = Camera.main.ScreenPointToRay(v);
		Plane plane = new Plane(Vector3.forward, 0f);
		float d;
		if (plane.Raycast(ray, out d))
		{
			LookAtPosition = ray.GetPoint (d);
		}
	}
}
