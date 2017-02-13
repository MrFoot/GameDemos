using UnityEngine;
using System.Collections;
using System;

/**
 *  Transform 的 旋转API
 * */
public class TestCube : MonoBehaviour {

    public enum RotateType
    {
        None,
        LocalEularAngles,
        EularAngles,
        LocalRotation,
        rotation,
        RotateAround,
        Rotate,
        LookAt,
    }

    private Transform mCatchedTrans;
    private GameObject mCatchedGo;
    private RotateType mRotateType;
    private int counter;

    private Action del_playRotate;

	void Start () {
        Debug.Log("Hello Cube");

        mCatchedGo = gameObject;
        mCatchedTrans = transform;
	}

	void Update () {
        if(mRotateType == RotateType.None) return;

        if (del_playRotate == null) return;

        del_playRotate();
        //Debug.Log("counter = " + counter);
	}

    public void StartRotate(int type)
    {
        RotateType rotateType = RotateType.None;
        

        try
        {
            rotateType = (RotateType)type;

            mRotateType = rotateType;

            Set(rotateType);
        }
        catch (Exception)
        {
            Debug.LogError("Error int type : " + type);
            del_playRotate = null;
        }
    }

    void Set(RotateType type)
    {
        counter = 0;
        switch (type)
        {
            case RotateType.LocalEularAngles:
                del_playRotate = Rotate_LocalEularAngles;
                break;
            case RotateType.EularAngles:
                del_playRotate = Rotate_EularAngles;
                break;
            case RotateType.LocalRotation:
                del_playRotate = Rotate_LocalRotation;
                break;
            case RotateType.rotation:
                del_playRotate = Rotate_rotation;
                break;
            case RotateType.RotateAround:
                del_playRotate = Rotate_RotateAround;
                break;
            case RotateType.Rotate:
                del_playRotate = Rotate_Rotate;
                break;
            case RotateType.LookAt:
                del_playRotate = Rotate_LookAt;
                break;
        }
    }

    void Rotate_LocalEularAngles()
    {
        mCatchedTrans.localEulerAngles = new Vector3(0, counter++, 0);
    }

    void Rotate_EularAngles()
    {
        mCatchedTrans.eulerAngles = new Vector3(0, counter++, 0);
    }

    void Rotate_LocalRotation()
    {
        Quaternion target = Quaternion.Euler(0,counter++,0);
        mCatchedTrans.localRotation = target;
    }

    void Rotate_rotation()
    {
        Quaternion target = Quaternion.Euler(0, counter++, 0);
        mCatchedTrans.rotation = target;
    }

    void Rotate_RotateAround()
    {
        mCatchedTrans.RotateAround(Vector3.zero,Vector3.up, Time.deltaTime * 50f);
    }

    void Rotate_Rotate()
    {
        mCatchedTrans.Rotate(Vector3.up, Time.deltaTime * 50f, Space.Self);
    }

    void Rotate_LookAt()
    {
        mCatchedTrans.LookAt(mCatchedTrans.parent);
    }

}
