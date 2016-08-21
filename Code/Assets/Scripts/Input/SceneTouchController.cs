
using System;
using System.Collections.Generic;
using UnityEngine;
using Soulgame.StateManagement;


public class SceneTouchController
{
	private const string Tag = "SceneTouchController";

	public enum TouchGesture
	{
		Poke,
		HorizontalSwipe,
		VerticalSwipe,
		Swipe
	}

	public delegate void HandleTouchEventHandler(TouchRemapper.TouchData touch);

	private Dictionary<Collider, List<GameAction>> ColliderPokeActionMapping;

	private Dictionary<Collider, List<GameAction>> ColliderSwipeActionMapping;

	private Dictionary<Collider, List<GameAction>> ColliderVerticalSwipeActionMapping;

	private Dictionary<Collider, List<GameAction>> ColliderHorizontalSwipeActionMapping;

	private bool CouldBeAHorizonzalSwipe;

	private bool CouldBeAVerticalSwipe;

	private const int MaxNumberOfTouchControllers = 10;

	private SortedDictionary<int, Func<TouchRemapper.TouchData, bool>> TouchControllers = new SortedDictionary<int, Func<TouchRemapper.TouchData, bool>>();

	private readonly List<int> CurrentIndexList = new List<int>();

	private readonly float VerticalSwipeComfortZone = (float)Screen.width / 10f;

	private readonly float HorizontalSwipeComfortZone = (float)Screen.height / 10f;

	private readonly float MinSwipeDistance = (float)Screen.width * 0.1f;

	private readonly float MaxPokeDistance = (float)Screen.width * 0.1f;

	private readonly float MaxSwipeDistance = float.MaxValue;

	private readonly float MinPokeTime = 0.4f;

	private readonly int MinPokeUpdates = 20;

	private float MinSwipeTime = 0.2f;

	private float MaxSwipeTime = 2f;

	private int UpdateIndex = 0;

	private int LayerMask = 0;

	private float StartTime;

	private Vector2 StartPos;

	private int StartUpdateIndex;

	private float EventDetectedTime;

	/// <summary>
	/// 是否检测多次
	/// </summary>
	private bool DetectedMultiTimeEvent = true;

	private Collider GestureColider;

	/// <summary>
	/// 事件被检测到
	/// </summary>
	private bool EventDetected;

	private List<GameAction> TmpReusableList = new List<GameAction>();

	private List<Func<TouchRemapper.TouchData, bool>> TmpValueList = new List<Func<TouchRemapper.TouchData, bool>>();

	private readonly List<int> TmpReusableIntList = new List<int>();

	public float ResetTrackingTime = 0.05f;

	public GameStateManager GameStateManager
	{
		get;
		set;
	}

	public RaycastHit LastHitInfo
	{
		get;
		private set;
	}

	public void Init()
	{
		this.ColliderPokeActionMapping = new Dictionary<Collider, List<GameAction>>();
		this.ColliderSwipeActionMapping = new Dictionary<Collider, List<GameAction>>();
		this.ColliderVerticalSwipeActionMapping = new Dictionary<Collider, List<GameAction>>();
		this.ColliderHorizontalSwipeActionMapping = new Dictionary<Collider, List<GameAction>>();
		this.CouldBeAHorizonzalSwipe = true;
		this.CouldBeAVerticalSwipe = true;
		this.LayerMask = 1 << (UnityEngine.LayerMask.NameToLayer("Player"));
		Main.Instance.GameStateManager.OnStateEnter += new StateManager<BaseGameState, GameAction>.StateChangeEvent(this.OnStateEnter);
	}

	public void OnStateEnter(BaseGameState previousState, object data)
	{
//		if (Main.Instance.GameStateManager.CurrentState == null)
//		{
//			if (!this.TouchControllers.ContainsKey(0))
//			{
//				this.RegisterTouchLayer(0, new Func<TouchRemapper.TouchData, bool>(this.HandleSceneTouch));
//			}
//		}
//		else
//		{
//			this.UnregisterTouchLayer(0);
//		}
	}

	public void UnregisterTouchLayer(int priority)
	{
		this.TouchControllers.Remove(priority);
		this.CurrentIndexList.Remove(priority);
	}

	public void UnregisterTouchLayer(Func<TouchRemapper.TouchData, bool> touchCallback)
	{
		this.TmpReusableIntList.Clear();
		for (int i = 0; i < this.CurrentIndexList.Count; i++)
		{
			int num = this.CurrentIndexList[i];
			if (this.TouchControllers[num] == touchCallback)
			{
				this.TmpReusableIntList.Add(num);
			}
		}
		for (int j = 0; j < this.TmpReusableIntList.Count; j++)
		{
			int num2 = this.TmpReusableIntList[j];
			this.TouchControllers.Remove(num2);
			this.CurrentIndexList.Remove(num2);
		}
	}

	public void RegisterTouchLayer(int priority, Func<TouchRemapper.TouchData, bool> touchCallback)
	{
		if (this.TouchControllers.ContainsKey(priority))
		{
			throw new InvalidProgramException("A touch is already registered with the specified priority: " + priority);
		}
		if (this.TouchControllers.Values.Count > MaxNumberOfTouchControllers)
		{
			throw new InvalidProgramException("Too Many Touch Controllers are registered. Max value is: " + MaxNumberOfTouchControllers);
		}
		this.TouchControllers[priority] = touchCallback;
		this.CurrentIndexList.Add(priority);
		this.CurrentIndexList.Sort();
	}

	public void HandleTouch(TouchRemapper.TouchData touch) {
		this.TmpValueList.Clear();
		for (int i = 0; i < this.CurrentIndexList.Count; i++)
		{
			this.TmpValueList.Add(this.TouchControllers[this.CurrentIndexList[i]]);
		}
		for (int j = 0; j < this.TmpValueList.Count; j++)
		{
			bool flag = this.TmpValueList[j](touch);
			if (flag)
			{
				break;
			}
		}
	}

	public bool HandleSceneTouch(TouchRemapper.TouchData touch)
	{
		this.UpdateIndex++;
		switch (touch.Phase)
		{
		case TouchPhase.Began:
			this.ResetFingerTracking(touch.Position);
			break;
		case TouchPhase.Moved:
			if (this.EventDetected && this.DetectedMultiTimeEvent && Time.time - this.EventDetectedTime > this.ResetTrackingTime)
			{
				this.ResetFingerTrackingForMultipleEvents(touch.Position);
			}
			if (!this.EventDetected || this.DetectedMultiTimeEvent)
			{
				this.TestIfSwipe(touch);
			}
            return !this.EventDetected && Time.unscaledTime - this.StartTime > this.MinPokeTime && this.UpdateIndex - this.StartUpdateIndex > this.MinPokeUpdates && this.PokeDetected(touch.Position);
		case TouchPhase.Stationary:
            if (!this.EventDetected && Time.unscaledTime - this.StartTime > this.MinPokeTime && this.UpdateIndex - this.StartUpdateIndex > this.MinPokeUpdates)
			{
				return this.PokeDetected(touch.Position);
			}
			break;
		case TouchPhase.Ended:
			return !this.EventDetected && (!this.DetectedMultiTimeEvent || !this.TestIfSwipe(touch)) && this.PokeDetected(touch.Position);
		}
		return this.GestureColider != null;
	}

	private bool TestIfSwipe(TouchRemapper.TouchData touch)
	{
		this.GestureColider = this.TestAllRegisteredCollidersByPriority(touch.Position);
		if (Mathf.Abs(touch.Position.y - this.StartPos.y) > this.HorizontalSwipeComfortZone)
		{
			//如果y轴距离过大，那么久不是一个水平滑动
			this.CouldBeAHorizonzalSwipe = false;
		}
		else if (Mathf.Abs(touch.Position.x - this.StartPos.x) > this.VerticalSwipeComfortZone)
		{
			this.CouldBeAVerticalSwipe = false;
		}
		//滑动时间限制
        float num = Time.unscaledTime - this.StartTime;
		if (num < this.MinSwipeTime || num > this.MaxSwipeTime)
		{
			return false;
		}
		//滑动距离限制
		float num2 = this.StartPos.x - touch.Position.x;
		float num3 = this.StartPos.y - touch.Position.y;
		float num4 = Mathf.Sqrt(num2 * num2 + num3 * num3);
		if (num4 < this.MinSwipeDistance || num4 > this.MaxSwipeDistance)
		{
			return false;
		}

		bool result;
		if (this.CouldBeAVerticalSwipe)
		{
			result = this.VerticalSwipeDetected(num3 < 0f);
		}
		else if (this.CouldBeAHorizonzalSwipe)
		{
			result = this.HorizontalSwipeDetected(num2 < 0f);
		}
		else
		{
			result = this.SwipingDetected();
		}
		this.EventDetectedTime = Time.time;
		this.EventDetected = true;
		return result;
	}

	private void ResetFingerTracking(Vector2 currentPosition)
	{
        this.StartTime = Time.unscaledTime;
		this.StartPos = currentPosition;
		this.StartUpdateIndex = this.UpdateIndex;
		this.GestureColider = this.TestAllRegisteredCollidersByPriority(this.StartPos);
		this.EventDetectedTime = float.MaxValue;
		this.CouldBeAVerticalSwipe = true;
		this.CouldBeAHorizonzalSwipe = true;
		this.EventDetected = false;
	}

	private void ResetFingerTrackingForMultipleEvents(Vector2 currentPosition)
	{
        this.StartTime = Time.unscaledTime;
		this.StartUpdateIndex = this.UpdateIndex;
		this.StartPos = currentPosition;
		this.GestureColider = this.TestAllRegisteredCollidersByPriority(this.StartPos);
		this.EventDetectedTime = float.MaxValue;
		this.CouldBeAVerticalSwipe = true;
		this.CouldBeAHorizonzalSwipe = true;
		this.EventDetected = true; //已经被检测过
	}

	private Collider TestAllRegisteredCollidersByPriority(Vector3 screenPosition)
	{
		Ray ray = Camera.main.ScreenPointToRay(screenPosition);
		RaycastHit lastHitInfo = default(RaycastHit);
		Physics.Raycast(ray, out lastHitInfo, 1000f, this.LayerMask);
		this.LastHitInfo = lastHitInfo;
		return lastHitInfo.collider;
	}

	private bool HorizontalSwipeDetected(bool left)
	{
		if (this.GestureColider == null)
		{
			return false;
		}
		if (!this.ColliderHorizontalSwipeActionMapping.ContainsKey(this.GestureColider))
		{
			return false;
		}
		List<GameAction> collection = this.ColliderHorizontalSwipeActionMapping[this.GestureColider];
		this.TmpReusableList.Clear();
		this.TmpReusableList.AddRange(collection);
		for (int i = 0; i < this.TmpReusableList.Count; i++)
		{
			this.GameStateManager.FireAction(this.TmpReusableList[i], this.GestureColider);
		}
		return true;
	}

	private bool VerticalSwipeDetected(bool up)
	{
		if (this.GestureColider == null)
		{
			return false;
		}
		if (!this.ColliderVerticalSwipeActionMapping.ContainsKey(this.GestureColider))
		{
			return false;
		}
		List<GameAction> collection = this.ColliderVerticalSwipeActionMapping[this.GestureColider];
		this.TmpReusableList.Clear();
		this.TmpReusableList.AddRange(collection);
		for (int i = 0; i < this.TmpReusableList.Count; i++)
		{
			this.GameStateManager.FireAction(this.TmpReusableList[i], this.GestureColider);
		}
		return true;
	}

	private bool SwipingDetected()
	{
		if (this.GestureColider == null)
		{
			return false;
		}
		if (!this.ColliderSwipeActionMapping.ContainsKey(this.GestureColider))
		{
			return false;
		}
		List<GameAction> collection = this.ColliderSwipeActionMapping[this.GestureColider];
		this.TmpReusableList.Clear();
		this.TmpReusableList.AddRange(collection);
		for (int i = 0; i < this.TmpReusableList.Count; i++)
		{
			this.GameStateManager.FireAction(this.TmpReusableList[i], this.GestureColider);
		}
		return true;
	}

	private bool PokeDetected(Vector2 currentPosition)
	{
		if ((this.StartPos - currentPosition).magnitude > this.MaxPokeDistance)
		{
			return false;
		}
		if (this.GestureColider == null)
		{
			return false;
		}
		if (!this.ColliderPokeActionMapping.ContainsKey(this.GestureColider))
		{
			return false;
		}
		this.EventDetected = true;
		List<GameAction> collection = this.ColliderPokeActionMapping[this.GestureColider];
		this.TmpReusableList.Clear();
		this.TmpReusableList.AddRange(collection);
		for (int i = 0; i < this.TmpReusableList.Count; i++)
		{
			this.GameStateManager.FireAction(this.TmpReusableList[i], this.GestureColider);
		}
		return true;
	}

	public void RegisterAction(GameAction action, SceneTouchController.TouchGesture gesture, Collider collider)
	{
		switch (gesture)
		{
		case SceneTouchController.TouchGesture.Poke:
			this.AddEventOnCollider(this.ColliderPokeActionMapping, collider, action);
			break;
		case SceneTouchController.TouchGesture.HorizontalSwipe:
			this.AddEventOnCollider(this.ColliderHorizontalSwipeActionMapping, collider, action);
			break;
		case SceneTouchController.TouchGesture.VerticalSwipe:
			this.AddEventOnCollider(this.ColliderVerticalSwipeActionMapping, collider, action);
			break;
		case SceneTouchController.TouchGesture.Swipe:
			this.AddEventOnCollider(this.ColliderSwipeActionMapping, collider, action);
			this.AddEventOnCollider(this.ColliderHorizontalSwipeActionMapping, collider, action);
			this.AddEventOnCollider(this.ColliderVerticalSwipeActionMapping, collider, action);
			break;
		}
	}

	public void UnregisterAction(GameAction action, SceneTouchController.TouchGesture gesture, Collider collider)
	{
		switch (gesture)
		{
		case SceneTouchController.TouchGesture.Poke:
			this.RemoveEventOnCollider(this.ColliderPokeActionMapping, collider, action);
			break;
		case SceneTouchController.TouchGesture.HorizontalSwipe:
			this.RemoveEventOnCollider(this.ColliderHorizontalSwipeActionMapping, collider, action);
			break;
		case SceneTouchController.TouchGesture.VerticalSwipe:
			this.RemoveEventOnCollider(this.ColliderVerticalSwipeActionMapping, collider, action);
			break;
		case SceneTouchController.TouchGesture.Swipe:
			this.RemoveEventOnCollider(this.ColliderSwipeActionMapping, collider, action);
			this.RemoveEventOnCollider(this.ColliderHorizontalSwipeActionMapping, collider, action);
			this.RemoveEventOnCollider(this.ColliderVerticalSwipeActionMapping, collider, action);
			break;
		}
	}

	private void AddEventOnCollider(Dictionary<Collider, List<GameAction>> mapping, Collider collider, GameAction action)
	{
		if (!mapping.ContainsKey(collider))
		{
			mapping[collider] = new List<GameAction>();
		}
		mapping[collider].Add(action);
	}

	private void RemoveEventOnCollider(Dictionary<Collider, List<GameAction>> mapping, Collider collider, GameAction action)
	{
		mapping[collider].Remove(action);
		if (mapping[collider].Count == 0)
		{
			mapping.Remove(collider);
		}
	}

	public void UnregisterAllActionsOnColider(Collider collider)
	{
		this.ColliderPokeActionMapping.Remove(collider);
		this.ColliderHorizontalSwipeActionMapping.Remove(collider);
		this.ColliderVerticalSwipeActionMapping.Remove(collider);
		this.ColliderSwipeActionMapping.Remove(collider);
	}

	public void UnregisterAllActions()
	{
		this.ColliderPokeActionMapping.Clear();
		this.ColliderHorizontalSwipeActionMapping.Clear();
		this.ColliderVerticalSwipeActionMapping.Clear();
		this.ColliderSwipeActionMapping.Clear();
	}

}


