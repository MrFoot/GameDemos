using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace FootStudio.Framework
{
    public class TouchInfo
    {
        public Vector3 screenPos;
        public Vector3 lastScreenPos;
        public Vector3 delta;
        public Vector3 totalDelta;
        public TouchPhase phase;
        public bool isOnUI;
        public Collider lastHit;

        public bool dragStart;
    }

    /// <summary>
    /// 单点触摸
    /// </summary>
    public class TouchController : MonoBehaviour
    {
        public float TouchDragThreshold = 20f;

        private Camera mainCamera;
        private TouchInfo currentTouch = null;

        private Dictionary<Collider, TouchHandler.OnTouchDownDelegate> ColliderTouchDownMapping;
        private Dictionary<Collider, TouchHandler.OnTouchUpDelegate> ColliderTouchUpMapping;
        private Dictionary<Collider, TouchHandler.OnDragDelegate> ColliderDragMapping;

        private static TouchController s_instance;

        public static TouchController Instance
        {
            get
            {
                return s_instance;
            }
        }

        void Awake()
        {
            TouchDragThreshold *= TouchDragThreshold;

            s_instance = this;

            ColliderTouchDownMapping = new Dictionary<Collider, TouchHandler.OnTouchDownDelegate>();
            ColliderTouchUpMapping = new Dictionary<Collider, TouchHandler.OnTouchUpDelegate>();
            ColliderDragMapping = new Dictionary<Collider, TouchHandler.OnDragDelegate>();
        }

        public TouchHandler Get(GameObject go)
        {
            return go.GetComponent<TouchHandler>();
        }

        public void RegisterOnTouchDown(Collider collider, TouchHandler.OnTouchDownDelegate del)
        {
            ColliderTouchDownMapping[collider] = del;
        }

        public void UnRegisterOnTouchDown(Collider collider)
        {
            if (collider != null && ColliderTouchDownMapping.ContainsKey(collider))
            {
                ColliderTouchDownMapping.Remove(collider);
            }
        }

        public void RegisterOnTouchUp(Collider collider, TouchHandler.OnTouchUpDelegate del)
        {
            ColliderTouchUpMapping[collider] = del;
        }

        public void UnRegisterOnTouchUp(Collider collider)
        {
            if (collider != null && ColliderTouchUpMapping.ContainsKey(collider))
            {
                ColliderTouchUpMapping.Remove(collider);
            }
        }

        public void RegisterOnDrag(Collider collider, TouchHandler.OnDragDelegate del)
        {
            ColliderDragMapping[collider] = del;
        }

        public void UnRegisterOnDrag(Collider collider)
        {
            if (collider != null && ColliderDragMapping.ContainsKey(collider))
            {
                ColliderDragMapping.Remove(collider);
            }
        }

        public void UnRegistAllTouch(Collider collider)
        {
            UnRegisterOnTouchDown(collider);
            UnRegisterOnTouchUp(collider);
            UnRegisterOnDrag(collider);
        }

        void OnDestroy()
        {
            ColliderTouchDownMapping.Clear();
            ColliderTouchUpMapping.Clear();
            ColliderDragMapping.Clear();
        }

        TouchInfo HandleTouchDown()
        {
            TouchInfo info = new TouchInfo();
            info.screenPos = Input.mousePosition;
            info.lastScreenPos = Input.mousePosition;
            info.delta = Vector3.zero;
            info.totalDelta = Vector3.zero;
            info.dragStart = false;
            info.phase = TouchPhase.Began;
            info.isOnUI = InputHelper.IsPointerOverGameObject();

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit lastHitInfo = default(RaycastHit);
            if (Physics.Raycast(ray, out lastHitInfo, 1000f))
            {
                if (!info.isOnUI && lastHitInfo.collider != null && ColliderTouchDownMapping.ContainsKey(lastHitInfo.collider))
                {
                    if (ColliderTouchDownMapping[lastHitInfo.collider] != null)
                    {
                        ColliderTouchDownMapping[lastHitInfo.collider]();
                    }
                    
                    info.lastHit = lastHitInfo.collider;
                }
                else
                {
                    info.lastHit = null;
                }
            }

            return info;
        }
        void HandleTouchStay(TouchInfo info)
        {
            if (info.lastHit == null) return;

            Vector3 curScreenPos = Input.mousePosition;

            if (curScreenPos != info.lastScreenPos)
            {
                info.screenPos = curScreenPos;
                info.delta = curScreenPos - info.lastScreenPos;
                info.totalDelta += info.delta;
                info.lastScreenPos = curScreenPos;

                float mag = info.totalDelta.sqrMagnitude;

                if (mag > TouchDragThreshold)
                {
                    info.dragStart = true;
                    info.phase = TouchPhase.Moved;
                    HandleTouchDrag(info);
                }
                else
                {
                    info.dragStart = false;
                    info.phase = TouchPhase.Stationary;
                }
            }
        }
        void HandleTouchDrag(TouchInfo info)
        {
            if (ColliderDragMapping.ContainsKey(info.lastHit))
            {
                if (ColliderDragMapping[info.lastHit] != null)
                {
                    ColliderDragMapping[info.lastHit](info.delta);
                }
            }
        }

        void HandleTouchUp(TouchInfo info)
        {
            if (info.lastHit == null) return;

            if (ColliderTouchUpMapping.ContainsKey(info.lastHit))
            {
                if (ColliderTouchUpMapping[info.lastHit] != null)
                {
                    ColliderTouchUpMapping[info.lastHit](!info.dragStart);
                }
            }

        }


        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                currentTouch = HandleTouchDown();
            }
            else if (Input.GetMouseButton(0))
            {
                HandleTouchStay(currentTouch);
            }
            else if (Input.GetMouseButtonUp(0))
            {
                HandleTouchUp(currentTouch);
            }
        }


    }
}