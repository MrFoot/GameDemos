using UnityEngine;
using System.Collections;

/// <summary>
/// 该基类是视图的基类，一个scene维护一个SceneController用于处理视图
/// </summary>
public abstract class BaseSceneController : MonoBehaviour{

	public Level Level;

	private bool Paused;

	public virtual void OnEnter() {
        //保证一个场景只有一个有效的触碰检测
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
        switch (touch.Phase)
        {
            case TouchPhase.Began:
                break;
            case TouchPhase.Ended:
                break;
            case TouchPhase.Moved:
            case TouchPhase.Stationary:

                break;
            default:
                break;
        }
		return false;
	}

}
