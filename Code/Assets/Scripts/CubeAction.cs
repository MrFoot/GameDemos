using UnityEngine;
using System.Collections;

public class CubeAction : MonoBehaviour {
    private Transform Trans;
	// Use this for initialization
	void Start () {
        Trans = this.transform;
	}
	
	// Update is called once per frame
	void Update () {
        Trans.Rotate(Vector3.up,Time.deltaTime * 200);
	}
}
