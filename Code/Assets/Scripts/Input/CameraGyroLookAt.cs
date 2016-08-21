using UnityEngine;
using System.Collections;

public class CameraGyroLookAt : MonoBehaviour {

	private Transform LeftTarget;
	private Transform RightTarget;
	private Transform TopTarget;
	private Transform DownTarget;

	private Vector3 LeftDirction;
	private Vector3 RightDirction;
	private Vector3 TopDirction;
	private Vector3 DownDirction;

	private float HalfAngle;

	private Vector3 RotationRate;
	private float ChangeSpeed = 0.2f;
	private Vector3 LastAngle;
	private Vector3 CurrentAngle;

	void Awake() {
		RotationRate = Vector3.zero;
		LeftTarget = GameObject.Find ("Border/CubeLeft").transform;
		RightTarget = GameObject.Find ("Border/CubeRight").transform;
		TopTarget = GameObject.Find ("Border/CubeTop").transform;
		DownTarget = GameObject.Find ("Border/CubeDown").transform;
		if (LeftTarget && RightTarget && TopTarget && DownTarget) {
			Input.gyro.enabled = true;
		}
	}

	void Start () {

		HalfAngle = GetComponent<Camera> ().fieldOfView / 2;
	}

	void InitDirction() {
		LeftDirction = LeftTarget.position - transform.position;
		RightDirction = RightTarget.position - transform.position;
		TopDirction = TopTarget.position - transform.position;
		DownDirction =  DownTarget.position - transform.position;
	}
	
	// Update is called once per frame
	void LateUpdate () {
		InitDirction (); //暂时放这里，后面改到初始化的start方法中

		#if UNITY_EDITOR
			RotationRate.y = Input.GetAxis("Horizontal") * 0.5f;
			RotationRate.x = -Input.GetAxis("Vertical") * 0.5f;
			RotationRate.z = 0f;
		#else
			RotationRate = Input.gyro.rotationRate;
		#endif

		LastAngle = transform.rotation.eulerAngles;
		CurrentAngle = transform.rotation.eulerAngles;
		CurrentAngle.x += RotationRate.x * ChangeSpeed;
		CurrentAngle.y += RotationRate.y * ChangeSpeed;
		CurrentAngle.z = 0f;
		transform.rotation = Quaternion.Euler (CurrentAngle);
		//Debug.Log (Vector3.Angle (TopDirction, transform.forward));
		if (IsExBorderHorizontal()) {
			CurrentAngle.y = LastAngle.y;
		}
		if (IsExBorderVertical()) {
			CurrentAngle.x = LastAngle.x;
		}
	
		transform.rotation = Quaternion.Euler (CurrentAngle);
	}

	private bool IsExBorderHorizontal() {
		return (Vector3.Angle (LeftDirction, transform.forward) < HalfAngle && CurrentAngle.y < LastAngle.y) || (Vector3.Angle (RightDirction, transform.forward) < HalfAngle && CurrentAngle.y > LastAngle.y);
	}

	private bool IsExBorderVertical() {
		return (Vector3.Angle (TopDirction, transform.forward) < HalfAngle && CurrentAngle.x < LastAngle.x) || (Vector3.Angle (DownDirction, transform.forward) < HalfAngle && CurrentAngle.x > LastAngle.x);
	}
}
