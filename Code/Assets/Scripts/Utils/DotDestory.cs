using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using FootStudio.Util;
using UnityEngine.SceneManagement;
using System;

public class DotDestory : MonoBehaviour {

	// Use this for initialization
	void Start () {
		DontDestroyOnLoad (this.gameObject);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
