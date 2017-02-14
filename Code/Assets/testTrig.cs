using UnityEngine;
using System.Collections;

public class testTrig : MonoBehaviour {

    private BoxCollider collider;
	// Use this for initialization
	void Start () {
        collider = GetComponent<BoxCollider>();
	}
	
    void OnTriggerEnter(Collider collider)
    {
        Debug.Log(collider.name);
    }

	// Update is called once per frame
	void Update () {
	
	}
}
