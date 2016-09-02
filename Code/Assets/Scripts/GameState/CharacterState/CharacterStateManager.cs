using System;
using System.Collections.Generic;
using Soulgame.Util;
using UnityEngine;
using Soulgame.StateManagement;
using UnityEngine.SceneManagement;

public class CharacterStateManager : StateManager<BaseCharacterState, CharacterAction>
{

    private const string MsgInvalidToFireAction = "It's invalid to fire an action inside any of the State's methods. Fired action {0} in current state {1}.";
    private const string MsgCantCallOnActionTwiceFromTheSameStack = "Can't call OnAction from the same stack twice or more...";
    private object PassForwardData;

    private bool mStateChanging = false;

	public BaseCharacterState EntryState
	{
		get;
		protected set;
	}

    public CharacterBase CharacterBase {
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

	#region State
    public IdleState IdleState {
        get;
        private set;
    }

    public EscapeState EscapeState {
        get;
        private set;
    }

    public PlayingState PlayingState {
        get;
        private set;
    }



	#endregion
    public CharacterAction LastTriggeredAction
	{
		private get;
		set;
	}

    public CharacterStateManager(CharacterBase characterBase)
	{
        Debug.Log("new CharacterStateManager");
        this.CharacterBase = characterBase;

        this.IdleState = new IdleState(this);
        this.EscapeState = new EscapeState(this);
        this.PlayingState = new PlayingState(this);
	}
	
	public void Init()
	{
		this.EnterInitialState();
	}
	
	public override void OnUpdate()
	{
		base.OnUpdate();

        if (mStateChanging)
            mStateChanging = false;
	}

	protected override bool BlockStateChange(BaseCharacterState newState)
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

    public override bool FireAction(CharacterAction characterAction, object data) {
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
	
	public void EnterInitialState()
	{
		this.Data = this;
        this.EntryState = this.IdleState;
		Assert.IsTrue(this.EntryState != null, "Entry state must never be null", new object[0]);
		base.NextState = this.EntryState;
		if (base.PreviousState == null)
		{
			base.OnStatePreEnterEvent(base.NextState, base.PreviousState, this.Data);
		}

        this.OnStateChanged();
	}

}


