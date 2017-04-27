using UnityEngine;
using System.Collections;

/// <summary>
/// 该基类是视图的基类，一个scene维护一个SceneController用于处理视图
/// </summary>
public abstract class BaseSceneController : MonoBehaviour{

	public virtual void OnEnter() {
	}

	public virtual void OnExit(bool unload) {

	}

	public virtual void OnAppResume() {
	}

	public virtual void OnAppPause() {
	}

	public virtual void OnUpdate() {
	}

}
