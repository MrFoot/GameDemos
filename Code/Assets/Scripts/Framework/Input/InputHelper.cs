using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

namespace FootStudio.Framework
{
    public static class InputHelper
    {
        public static bool IsPointerOverGameObject()
        {

#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return true;
            }
        }
        return false;
#else
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                // Check if finger is over a UI element
                if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
                {
                    Debug.Log("Touched the UI");
                    return true;
                }
            }
            return false;
#endif


        }


    }

}