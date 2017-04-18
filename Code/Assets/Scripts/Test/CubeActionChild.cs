using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CubeActionChild : CubeAction
{

    private float touchDragThreshold = 20f * 20f;

    private Transform Trans;

    private TouchInfo currentTouch = null;

    private bool active = false;

	void Start () {
        Trans = this.transform;
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
