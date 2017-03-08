using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CubeAction :  MonoBehaviour{
    private Transform Trans;
    
    private bool active = false;

	void Start () {
        Trans = this.transform;
	}

    void OnMouseEnter()
    {
        //Debug.Log(gameObject.name + " : OnMouseEnter");
    }

    void OnMouseOver()
    {
        if(Input.GetMouseButton(0))
        {
            //Debug.Log(gameObject.name + " : OnMouseOver");
        }
        
    }

    void OnMouseExit()
    {
        //Debug.Log(gameObject.name + " : OnMouseExit");
        
    }

    void OnBecameVisible()
    {
        Debug.Log("OnBecameVisible");
    }

    void OnBecameInvisible()
    {
        Debug.Log("OnBecameInvisible");
    }

    void OnEnable()
    {
        Debug.Log("OnEnable");
    }

    void OnDisable()
    {
        Debug.Log("OnDisable : " + (int)Instruction.Health + " size = " + sizeof(Instruction));

    }

    enum Instruction : byte
    {
        Health = 0xFF,
        
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
