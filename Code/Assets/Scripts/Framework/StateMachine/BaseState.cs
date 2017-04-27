using System;
using FootStudio.StateManagement;
using FootStudio.Util;


namespace FootStudio.Framework
{
    /// <summary>
    /// 状态机基类，泛型A ： 用于响应Action
    /// </summary>
    /// <typeparam name="A"></typeparam>
    public abstract class BaseState<A> : StateManager<BaseState<A>, A>.State
    {
        public virtual string Tag
        {
            get
            {
                return base.GetType().Name;
            }
        }

        protected BaseState(BaseStateManager<A> stateManager)
            : base(stateManager)
        {
        }

        public virtual void OnPreExit(BaseState<A> nextState, object data)
        {
            base.OnPreExit(nextState, data);
        }

        public virtual BaseState<A> OnPreEnter(BaseState<A> previousState, object data)
        {
            return base.OnPreEnter(previousState, data);
        }

        public virtual void OnEnter(BaseState<A> previousState, object data)
        {
            
        }

        public virtual void OnExit(BaseState<A> nextState, object data)
        {

        }

        public virtual void OnAction(A characterAction, object data)
        {

        }

        public override void OnAppResume()
        {
            base.OnAppResume();

        }

        public override void OnAppPause()
        {
            base.OnAppPause();

        }

        public override void OnUpdate()
        {
            base.OnUpdate();
        }

    }
}


