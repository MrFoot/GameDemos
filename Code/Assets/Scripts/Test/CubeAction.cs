using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEditor;
using FootStudio.Framework;

public class CubeAction :  MonoBehaviour{

    private Transform Trans;

    private bool active = false;

    void Awake()
    {
    }

	void Start () {
        Trans = this.transform;
        TouchController.Instance.Get(gameObject).AddTouchUpListener(OnTouchUp);
	}

    void OnClick()
    {
        Debug.Log(gameObject.name + " OnClick");
    }

    void OnTouchDown()
    {
        Debug.Log(gameObject.name + " OnTouchDown");
    }

    void OnTouchUp(bool isClick)
    {
        if (isClick)
        {
            Debug.Log(gameObject.name + " Click");
        }
    }

    void OnDrag(Vector3 delta)
    {
        Debug.Log(gameObject.name + " OnDrag " + delta);
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

    }

    void OnMouseDrag()
    {

    }

    void OnMouseUp()
    {

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

    private void OnApplicationPause(bool gamePaused)
    {
        if (!gamePaused)
            InputHelper.IsPointerOverGameObject();
    }

	void Update () {
        
	}
}
