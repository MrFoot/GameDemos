using UnityEngine;
using System.Collections;

namespace FootStudio.Framework
{
    /// <summary>
    /// Character 模型的控制，提供包括模型的运动学/非运动学的变换，处理碰撞监测等
    /// </summary>
    public class BaseModelCtrl : MonoBehaviour
    {
        public Transform CatchedTrans
        {
            get;
            private set;
        }

        public GameObject CatchedGo
        {
            get;
            private set;
        }

        void Awake()
        {
            CatchedTrans = transform;
            CatchedGo = gameObject;
        }

        void Start()
        {
        }

        void Update()
        {
        }

        public void Show()
        {
            CatchedGo.SetActive(true);
        }

        public void Hide()
        {
            CatchedGo.SetActive(false);
        }

        public virtual void MoveTo(Vector3 to)
        {

        }

    }
}