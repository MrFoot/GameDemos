using UnityEngine;
using System.Collections;

namespace FootStudio.Framework
{
    [RequireComponent(typeof(Collider))]
    public class TouchHandler : MonoBehaviour
    {
        public delegate void OnTouchDownDelegate();  //按下检测
        public delegate void OnTouchUpDelegate(bool isClick);    //释放时检测
        public delegate void OnDragDelegate(Vector3 delta);       //按下时检测，移动超出阈值，每帧触发

        private OnTouchDownDelegate m_touch_down_delegate;
        private OnTouchUpDelegate m_touch_up_delegate;
        private OnDragDelegate m_drag_delegate;

        private Collider mCollider;

        void Awake()
        {
            mCollider = GetComponent<Collider>();
            Assert.NotNull(mCollider, "mCollider");
        }

        protected virtual void OnEnable()
        {
            if (TouchController.Instance != null)
            {
                TouchController.Instance.RegisterOnTouchDown(mCollider, m_touch_down_delegate);
                TouchController.Instance.RegisterOnTouchUp(mCollider, m_touch_up_delegate);
                TouchController.Instance.RegisterOnDrag(mCollider, m_drag_delegate);
            }
        }

        protected virtual void OnDisable()
        {
            if (TouchController.Instance != null)
            {
                TouchController.Instance.UnRegistAllTouch(mCollider);
            }
        }

        #region TouchDown
        public void AddTouchDownListener(OnTouchDownDelegate del)
        {
            if (m_touch_down_delegate == null)
                m_touch_down_delegate = del;
            else
                m_touch_down_delegate += del;
            TouchController.Instance.RegisterOnTouchDown(mCollider, m_touch_down_delegate);
        }

        public void RemoveTouchDownListener(OnTouchDownDelegate del)
        {
            if (m_touch_down_delegate != null)
                m_touch_down_delegate -= del;
            TouchController.Instance.RegisterOnTouchDown(mCollider, m_touch_down_delegate);
        }

        #endregion

        #region TouchUp

        public void AddTouchUpListener(OnTouchUpDelegate del)
        {
            if (m_touch_up_delegate == null)
                m_touch_up_delegate = del;
            else
                m_touch_up_delegate += del;
            TouchController.Instance.RegisterOnTouchUp(mCollider, m_touch_up_delegate);
        }

        public void RemoveTouchUpListener(OnTouchUpDelegate del)
        {
            if (m_touch_up_delegate != null)
                m_touch_up_delegate -= del;
            TouchController.Instance.RegisterOnTouchUp(mCollider, m_touch_up_delegate);
        }

        #endregion

        public void AddDragListener(OnDragDelegate del)
        {
            if (m_drag_delegate == null)
                m_drag_delegate = del;
            else
                m_drag_delegate += del;
            TouchController.Instance.RegisterOnDrag(mCollider, m_drag_delegate);
        }

        public void RemoveDragListener(OnDragDelegate del)
        {
            if (m_drag_delegate != null)
                m_drag_delegate -= del;
            TouchController.Instance.RegisterOnDrag(mCollider, m_drag_delegate);
        }
    }
}