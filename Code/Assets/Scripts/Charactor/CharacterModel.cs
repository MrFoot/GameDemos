using UnityEngine;
using System.Collections;

public class CharacterModel : MonoBehaviour {
    [HideInInspector]
    public Transform CatchedTrans;

    [HideInInspector]
    public GameObject CatchedGo;

    //[HideInInspector]
    public float MoveSpeed = 5;
    public float TurnSpeed = 2;

    private const float Precision = 0.05f;  //精度

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

            CatchedTrans.position += dir * delta * MoveSpeed;
            //CatchedTrans.Translate(Vector3.left * Random.Range(-delta * MoveSpeed, delta * MoveSpeed));
            LockAt(des, delta);
        }
        
    }

    void LockAt(Vector3 target,float delta)
    {
        Vector3 dir = target - CatchedTrans.position;

        Quaternion lookRot = Quaternion.LookRotation(dir);
        CatchedTrans.rotation = Quaternion.Slerp(CatchedTrans.rotation, lookRot, Mathf.Clamp01(TurnSpeed * delta));
    }


    public bool IsAtPosition(Vector3 pos) {
        return (pos - CatchedTrans.position).magnitude < Precision;
    }

}
