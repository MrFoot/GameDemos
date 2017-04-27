using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEditor;

public class TouchInfo
{
    public Vector3 pos;
    public Vector3 lastPos;
    public Vector3 delta;
    public Vector3 totalDelta;

    public bool dragStart;
}

public class CubeAction :  MonoBehaviour{

    private float touchDragThreshold = 20f * 20f;

    private Transform Trans;

    private TouchInfo currentTouch = null;

    private bool active = false;

    void Awake()
    {
    }

	void Start () {
        Trans = this.transform;
	}


    void OnMouseEnter()
    {
        //Debug.Log(gameObject.name + " : OnMouseEnter");
    }

    void OnMouseExit()
    {
        //Debug.Log(gameObject.name + " : OnMouseExit");
    }

    void OnMouseDown()
    {
        currentTouch = new TouchInfo();
        currentTouch.pos = Input.mousePosition;
        currentTouch.lastPos = Input.mousePosition;
        currentTouch.delta = Vector3.zero;
        currentTouch.totalDelta = Vector3.zero;
        currentTouch.dragStart = false;
    }

    void OnMouseDrag()
    {
        if(!currentTouch.dragStart)
        {
            currentTouch.pos = Input.mousePosition;
            currentTouch.delta = currentTouch.pos - currentTouch.lastPos;
            currentTouch.totalDelta += currentTouch.delta;
            currentTouch.lastPos = Input.mousePosition;

            float mag = currentTouch.totalDelta.sqrMagnitude;

            if (mag > touchDragThreshold)
            {
                currentTouch.dragStart = true;
                Debug.Log("dragStart");

            }
        }
    }

    void OnMouseUp()
    {
        if (!currentTouch.dragStart)
        {
            OnClick();
        }

        currentTouch = null;
    }

    void OnClick()
    {
        Debug.Log("OnClick");
    }

    void OnBecameVisible()
    {
        //Debug.Log("OnBecameVisible");
    }

    void OnBecameInvisible()
    {
        //Debug.Log("OnBecameInvisible");
    }

    void OnEnable()
    {
    }

    void OnDisable()
    {

    }

    //public override void OnBeginDrag(PointerEventData data)
    //{
    //    MyDebug("OnBeginDrag called.");
    //}

    //public override void OnCancel(BaseEventData data)
    //{
    //    MyDebug("OnCancel called.");
    //}

    //public override void OnDeselect(BaseEventData data)
    //{
    //    MyDebug("OnDeselect called.");
    //}

    //public override void OnDrag(PointerEventData data)
    //{
    //    MyDebug("OnDrag called.");
    //}

    //public override void OnDrop(PointerEventData data)
    //{
    //    MyDebug("OnDrop called.");
    //}

    //public override void OnEndDrag(PointerEventData data)
    //{
    //    MyDebug("OnEndDrag called.");
    //}

    //public override void OnInitializePotentialDrag(PointerEventData data)
    //{
    //    MyDebug("OnInitializePotentialDrag called.");
    //}

    //public override void OnMove(AxisEventData data)
    //{
    //    MyDebug("OnMove called.");
    //}

    //public override void OnPointerClick(PointerEventData data)
    //{
    //    MyDebug("OnPointerClick called.");
    //}

    //public override void OnPointerDown(PointerEventData data)
    //{
    //    MyDebug("OnPointerDown called.");
    //}

    //public override void OnPointerEnter(PointerEventData data)
    //{
    //    MyDebug("OnPointerEnter called.");
    //}

    //public override void OnPointerExit(PointerEventData data)
    //{
    //    MyDebug("OnPointerExit called.");
    //}

    //public override void OnPointerUp(PointerEventData data)
    //{
    //    MyDebug("OnPointerUp called.");
    //}

    //public override void OnScroll(PointerEventData data)
    //{
    //    MyDebug("OnScroll called.");
    //}

    //public override void OnSelect(BaseEventData data)
    //{
    //    MyDebug("OnSelect called.");
    //}

    //public override void OnSubmit(BaseEventData data)
    //{
    //    MyDebug("OnSubmit called.");
    //}

    //public override void OnUpdateSelected(BaseEventData data)
    //{
    //    MyDebug("OnUpdateSelected called.");
    //}


	void Update () {
        if (active)
            Trans.Rotate(Vector3.up,Time.deltaTime * 200);
	}
}
