using UnityEngine;
using System.Collections;

public class CharacterModel : MonoBehaviour {
    [HideInInspector]
    public Transform CatchedTrans;

    [HideInInspector]
    public GameObject CatchedGo;

    private const float Precision = 0.1f;

    [HideInInspector]
    public float Speed = 15;

    public Animator Animator {
        get;
        private set;
    }

	void Awake () {
        this.Animator = this.GetComponent<Animator>();
        CatchedTrans = transform;
        CatchedGo = gameObject;
	}
	
	void Update () {
	
	}

    public void Show() {
        gameObject.SetActive(true);
    }

    public void Hide() {
        gameObject.SetActive(false);
    }

    public void Reset() {
        CatchedTrans.position = Vector3.zero;
    }

    public void Move(Vector3 des, float delta) {
        Vector3 vec = des - CatchedTrans.position;
        float dis = vec.magnitude;

        if (dis > Precision)
        {
            Vector3 dir = vec.normalized;

            CatchedTrans.position += dir * delta * Speed;
        }
        
    }

    public bool IsAtPosition(Vector3 pos) {

        return (pos - CatchedTrans.position).magnitude < Precision;
    }

}
