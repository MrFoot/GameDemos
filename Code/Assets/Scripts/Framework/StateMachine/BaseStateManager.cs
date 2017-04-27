using System;
using System.Collections.Generic;
using FootStudio.Util;
using UnityEngine;
using FootStudio.StateManagement;

namespace FootStudio.Framework{
    
    public abstract class BaseStateManager<A> : StateManager<BaseState<A>, A>
    {

        private const string MsgInvalidToFireAction = "It's invalid to fire an action inside any of the State's methods. Fired action {0} in current state {1}.";
        private const string MsgCantCallOnActionTwiceFromTheSameStack = "Can't call OnAction from the same stack twice or more...";
        private object PassForwardData;

        private bool mStateChanging = false;

        public BaseState<A> EntryState
        {
            get;
            protected set;
        }

	    protected override string Tag
	    {
		    get
		    {
                return "CharacterStateManager";
		    }
	    }

        public A LastTriggeredAction
	    {
		    private get;
		    set;
	    }

        public BaseStateManager()
	    {
            
	    }

        public abstract void Init();
	
	    public override void OnUpdate()
	    {
		    base.OnUpdate();

            if (mStateChanging)
                mStateChanging = false;
	    }

	    protected override bool BlockStateChange(BaseState<A> newState)
	    {
		    return newState == null || base.BlockStateChange(newState);
	    }

	    protected override void OnStateChanged()
	    {
		    base.OnStateChanged();

	    }
	
	    protected override void ToNullState()
	    {
		    Main.Instance.QuitApp();
	    }

	    public override void OnAppPause()
	    {
		    base.OnAppPause();
	    }

        public override bool FireAction(A characterAction, object data) {
            if (this.CurrentState == null)
            {
                return false;
            }
            Assert.IsTrue(!this.ActionProcessing, MsgInvalidToFireAction, new object[]
			    {
				    characterAction,
				    this.CurrentState
			    });
            if (mStateChanging)
            {
                return false;
            }
            this.ActionProcessing = true;
            Assert.IsTrue(!this.OnActionExecuting, MsgCantCallOnActionTwiceFromTheSameStack, new object[0]);
            this.PassForwardData = data;
            this.HandleFireAction(characterAction, data);
            this.PassForwardData = null;
            this.ActionProcessing = false;
            StateManager.ActionTriggeredInUpdate = true;

            this.LastTriggeredAction = characterAction;
            return true;
        }

	    protected override void StartStateChange()
	    {
            mStateChanging = true;
		    StateManager.StateChangedInternal = true;

		    base.StartStateChange();
	    }
	
	    protected void EnterInitialState(BaseState<A> initialState)
	    {
		    this.Data = this;
            this.EntryState = initialState;
		    Assert.IsTrue(this.EntryState != null, "Entry state must never be null", new object[0]);
		    base.NextState = this.EntryState;
		    if (base.PreviousState == null)
		    {
			    base.OnStatePreEnterEvent(base.NextState, base.PreviousState, this.Data);
		    }

            this.OnStateChanged();
	    }

    }

}
