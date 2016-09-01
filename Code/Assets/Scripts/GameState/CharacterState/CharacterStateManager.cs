using System;
using System.Collections.Generic;
using Soulgame.Util;
using UnityEngine;
using Soulgame.StateManagement;
using UnityEngine.SceneManagement;

public class CharacterStateManager : StateManager<BaseCharacterState, CharacterAction>
{
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

	public override bool FireAction(CharacterAction characterAction, object data)
	{
        bool flag = !StateManager.ActionTriggeredInUpdate && base.FireAction(characterAction, data); ;
		if (flag)
		{
            this.LastTriggeredAction = characterAction;
		}
		return flag;
	}
	
	protected override void StartStateChange()
	{
		StateManager.StateChanging = true;
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


