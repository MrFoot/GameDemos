using UnityEngine;
using System.Collections;

namespace FootStudio.Framework
{
    /// <summary>
    /// 管理 Animator, 提供 Animator 的 各种方法集合
    /// </summary>
    [RequireComponent(typeof(Animator))]
    public class BaseAnimatorCtrl : MonoBehaviour
    {

        private Animator mAnimator;

        void Awake()
        {
            mAnimator = this.GetComponent<Animator>();
        }

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}