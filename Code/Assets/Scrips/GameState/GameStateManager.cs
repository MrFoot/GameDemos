
using System;
using System.Collections.Generic;
using Soulgame.Util;
using UnityEngine;
using Soulgame.StateManagement;

public abstract class GameStateManager<S, A> : StateManager<S, A> where S : StateManager<S, A>.State
{
	public StateManager<S, A>.StateChangeEvent OnStatePreExitPostRender;
	
	private List<Func<bool>> PendingDialogOpenChecks = new List<Func<bool>>();

	/// <summary>
	/// 游戏进入时加载的场景
	/// </summary>
	/// <value>The state of the entry.</value>
	public S EntryState
	{
		get;
		protected set;
	}
	
	public S InclusiveCurrentState
	{
		get;
		protected set;
	}
	
	public StateManager DialogStateManager
	{
		get;
		set;
	}
	
	public virtual void Init()
	{
		this.EnterInitialState();
	}
	
	public override void OnUpdate()
	{
		base.OnUpdate();
		if (base.CurrentState == null)
		{
			return;
		}
		if (StateManager.StateChanging)
		{
			return;
		}
		this.PendingDialogOpenChecks.Clear();
		S currentState = base.CurrentState;
		currentState.OnUpdate();
		this.CheckAndOpenAllDialogs();
		this.PendingDialogOpenChecks.Clear();
	}
	
	protected override bool BlockStateChange(S newState)
	{
		return newState == null || base.BlockStateChange(newState);
	}
	
	private void CheckAndOpenAllDialogs()
	{
		for (int i = 0; i < this.PendingDialogOpenChecks.Count; i++)
		{
			Func<bool> func = this.PendingDialogOpenChecks[i];
			if (!this.CanAutoOpen())
			{
				return;
			}
			if (func())
			{
				return;
			}
		}
	}
	
	public void TryToAutoOpen(Func<bool> openingMethods)
	{
		this.PendingDialogOpenChecks.Add(openingMethods);
	}
	
	public virtual bool CanAutoOpen()
	{
		return !StateManager.ActionTriggeredInUpdate && !StateManager.StateChanging;
	}
	
	public bool CheckAndOpenStates(List<IAutoOpenState> autoOpenStates)
	{
		S s = (S)((object)null);
		if (this.DialogStateManager != null && this.DialogStateManager.IsActive())
		{
			return false;
		}
		IAutoOpenState autoOpenState = null;
		for (int i = 0; i < autoOpenStates.Count; i++)
		{
			IAutoOpenState autoOpenState2 = autoOpenStates[i];
			if (autoOpenState2.CanOpen())
			{
				s = (autoOpenState2 as S);
				if (autoOpenState2.AutoClear())
				{
					autoOpenState = autoOpenState2;
				}
				break;
			}
		}
		if (autoOpenState != null)
		{
			autoOpenStates.Remove(autoOpenState);
		}
		bool result = false;
		if (s != null)
		{
			this.OnActionExecuting = true;
			result = this.ChangeState(s, this);
			this.OnActionExecuting = false;
		}
		return result;
	}
	
	protected abstract bool CanLoadLevel();
	
	protected abstract string LoadLevelName();
	
	protected override void StartStateChange()
	{
		this.InclusiveCurrentState = base.NextState;
		StateManager.StateChanging = true;
		StateManager.StateChangedInternal = true;
		if (this.CanLoadLevel())
		{
			Application.LoadLevel(this.LoadLevelName());
		}
		else
		{
			base.StartStateChange();
		}
	}
	
	public void EnterInitialState()
	{
		Pair<S, object> entryStateAndData = this.GetEntryStateAndData();
		this.Data = entryStateAndData.Second;
		this.EntryState = entryStateAndData.First;
		Assert.IsTrue(this.EntryState != null, "Entry state must never be null", new object[0]);
		base.NextState = this.EntryState;
		this.InclusiveCurrentState = base.NextState;
		if (base.PreviousState == null)
		{
			base.OnStatePreEnterEvent(base.NextState, base.PreviousState, this.Data);
		}
	}
	
	protected abstract Pair<S, object> GetEntryStateAndData();
	
	public virtual void OnLevelWasLoaded(int lvl)
	{
		this.OnStateChanged();
	}
	
	public override bool FireAction(A gameAction, object data)
	{
		return !StateManager.ActionTriggeredInUpdate && base.FireAction(gameAction, data);
	}
	
	public void OnStatePreExitPostRenderEvent()
	{
		Assert.IsTrue(StateManager.StateChanging);
		if (base.CurrentState == null)
		{
			return;
		}
		if (this.OnStatePreExitPostRender != null)
		{
			this.OnStatePreExitPostRender(base.NextState, this.Data);
		}
	}
	
	protected override void OnStateExitEvent(S currentState, S state, object data)
	{
		if (this.DialogStateManager != null)
		{
			this.DialogStateManager.OnGameStateExit(currentState, data);
		}
		base.OnStateExitEvent(currentState, state, data);
	}
	
	public abstract bool OnBackPress();
}


