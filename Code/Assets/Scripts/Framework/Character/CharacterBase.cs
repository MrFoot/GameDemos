using UnityEngine;
using System.Collections;
using FootStudio.Util;

namespace FootStudio.Framework
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(BaseModelCtrl))]
    [RequireComponent(typeof(BaseModelRenderCtrl))]
    public class CharacterBase<A> : MonoBehaviour
    {

        #region Properties
        public BaseStateManager<A> CharacterStateManager
        {
            get;
            protected set;
        }

        public BaseAnimatorCtrl AnimatorCtrl
        {
            get;
            private set;
        }

        public BaseModelCtrl Model
        {
            get;
            private set;
        }

        public BaseModelRenderCtrl ModelRender
        {
            get;
            private set;
        }

        #endregion

        public CharacterBase() { }

        void Awake()
        {
            this.AnimatorCtrl = this.GetComponent<BaseAnimatorCtrl>();
            this.Model = this.GetComponent<BaseModelCtrl>();
            this.ModelRender = this.GetComponent<BaseModelRenderCtrl>();
        }

        void Start()
        {
            Assert.NotNull(CharacterStateManager, "CharacterStateManager");
            CharacterStateManager.Init();
        }


        public void OnApplicationPause()
        {

        }

        public void OnApplicationResume()
        {

        }

        public void Update()
        {
            this.CharacterStateManager.OnUpdate();
        }

    }
}
