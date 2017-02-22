using UnityEngine;
using System.Collections;

public class ActorUnit : MonoBehaviour {

    public Unit unit{get; private set;}
    public float Speed;

	// Use this for initialization
	void Start () {
        unit = new Unit(transform, Speed);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
