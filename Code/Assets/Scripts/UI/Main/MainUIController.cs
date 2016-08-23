using UnityEngine;
using System.Collections;
using System;
using Soulgame.Util;

public class MainUIController : MonoBehaviour {


	public static MainUIController Instance
	{
		get;
		private set;
	}

	void Awake() {
		DontDestroyOnLoad (this);
		MainUIController.Instance = this;

	}

	void Start() {

	}

    public void OnSceneStatePreEnter(BaseGameSceneState baseGameSceneState)
	{

	}

    public void OnSceneStatePreExit(BaseGameSceneState baseGameSceneState) {

    }

    public void ShowTips(string tips) {
        Debug.Log(tips);
    }

}
