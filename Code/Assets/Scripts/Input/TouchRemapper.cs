using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TouchRemapper : MonoBehaviour {

	public class TouchData
	{
		private Vector3 position;
		
		public float DeltaTime
		{
			get;
			private set;
		}
		
		public int Priority
		{
			get;
			private set;
		}
		
		public int FingerId
		{
			get;
			private set;
		}
		
		public TouchPhase Phase
		{
			get;
			private set;
		}
		
		public Vector3 Position
		{
			get
			{
				return this.position;
			}
			protected internal set
			{
				Vector3 vector = value;
				if (vector.x == this.position.x && vector.y == this.position.y)
				{
					this.Phase = TouchPhase.Stationary;
				}
				else
				{
					this.Phase = TouchPhase.Moved;
				}
				this.position = vector;
			}
		}
		
		public void SetData(Vector3 position, int fingerId, TouchPhase phase, int priority, float deltaTime)
		{
			this.Position = position;
			this.FingerId = fingerId;
			this.Phase = phase;
			this.DeltaTime = deltaTime;
			this.Priority = priority;
		}

		public void Hold(Vector3 position, float deltaTime)
		{
			this.Position = position;
			this.DeltaTime = deltaTime;
		}

		public void Up(Vector3 position, float deltaTime)
		{
			this.Position = position;
			this.DeltaTime = deltaTime;
			this.Phase = TouchPhase.Ended;
			this.FingerId = -1;
		}
		
		public override string ToString()
		{
			return string.Format("[TouchWrapper: DeltaTime={0}, Priority={1}, FingerId={2}, Phase={3}, Position={4}]", new object[] {
				this.DeltaTime,
				this.Priority,
				this.FingerId,
				this.Phase,
				this.Position
			});
		}
	}
		
	private bool InitDone;

	private SceneTouchController SceneTouchController;

	private TouchData CurrentTouchData = new TouchData();

	private const int ScenePriority = 1000;

	private int FingerId = -1;

	private Touch CurrentTouch;

	private bool HasTouch = false;

	void Awake() {
		Init ();
	}

	private void Init() {
		if (this.InitDone)
		{
			return;
		}
		this.SceneTouchController = Main.Instance.SceneTouchController;
		this.InitDone = true;
	}

	void Update() {
		#if UNITY_EDITOR
		if (Input.GetMouseButtonDown(0)) {
			CurrentTouchData.SetData(Input.mousePosition, 100, TouchPhase.Began, ScenePriority, 0f);
			FingerId = 100;
			HasTouch = true;
		} else if (Input.GetMouseButton(0)) {
			CurrentTouchData.Hold(Input.mousePosition, Time.deltaTime);
			HasTouch = true;
		} else if (Input.GetMouseButtonUp(0)) {
			CurrentTouchData.Up(Input.mousePosition, Time.deltaTime);
			FingerId = -1;
			HasTouch = true;
		}
		#endif
		if (Input.touchCount > 0) {
			if (FingerId == -1) {
				for(int i=0; i<Input.touchCount; i++) {
					if (Input.touches [i].phase == TouchPhase.Began) {
						FingerId = Input.touches [i].fingerId;
						HasTouch = HandleTouch (Input.touches [i]);
						break;
					}
				}
			} else {
				for(int i=0; i<Input.touchCount; i++) {
					if (FingerId == Input.touches [i].fingerId) {
						HasTouch = HandleTouch (Input.touches [i]);
						break;
					}
				}
			}
		}
		if (HasTouch)
			this.SceneTouchController.HandleTouch (CurrentTouchData);
		
		HasTouch = false;
	}

	private bool HandleTouch(Touch touch) {
		switch (touch.phase) {
		case TouchPhase.Began:
			CurrentTouchData.SetData (touch.position, touch.fingerId, TouchPhase.Began, ScenePriority, 0f);
			return true;
		case TouchPhase.Moved:
		case TouchPhase.Stationary:
			CurrentTouchData.Hold (touch.position, Time.deltaTime);
			return true;
		case TouchPhase.Canceled:
			FingerId = -1;
			return false;
		case TouchPhase.Ended:
			CurrentTouchData.Up (touch.position, Time.deltaTime);
			FingerId = -1;
			return true;
		}
		return false;
	}
}
