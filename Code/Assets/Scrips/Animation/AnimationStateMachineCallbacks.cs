
using System;

public struct AnimationStateMachineCallbacks
{
	public delegate AnimationClipInfo LoadAnimationClipInfoDelegate(string name, AnimationStateMachine.State.PriorityEnum statePriority);
	
	public delegate void ResetEvent();
	
	public delegate void StateChangeEvent(int layerIndex, int previousStateIndex, int newStateIndex);
	
	public delegate void UpdateEvent();
	
	public delegate void UpdateLayerEvent(int layerIndex);
	
	public AnimationStateMachineCallbacks.LoadAnimationClipInfoDelegate OnLoadAnimationClipInfo;
	
	public event AnimationStateMachineCallbacks.ResetEvent OnResetEvent;
	
	public event AnimationStateMachineCallbacks.StateChangeEvent OnPreStateChangeEvent;
	
	public event AnimationStateMachineCallbacks.StateChangeEvent OnPostStateChangeEvent;
	
	public event AnimationStateMachineCallbacks.UpdateEvent OnPreUpdateEvent;
	
	public event AnimationStateMachineCallbacks.UpdateEvent OnPostUpdateEvent;
	
	public event AnimationStateMachineCallbacks.UpdateLayerEvent OnPreUpdateLayerEvent;
	
	public event AnimationStateMachineCallbacks.UpdateLayerEvent OnPostUpdateLayerEvent;
	
	public AnimationClipInfo LoadAnimationClipInfo(string name, AnimationStateMachine.State.PriorityEnum statePriority)
	{
		if (this.OnLoadAnimationClipInfo != null)
		{
			return this.OnLoadAnimationClipInfo(name, statePriority);
		}
		return AnimationManager.LoadFromResources(name, 15f);
	}
	
	public void Reset()
	{
		if (this.OnResetEvent != null)
		{
			this.OnResetEvent();
		}
	}
	
	public void PreStateChange(int layerIndex, int previousStateIndex, int newStateIndex)
	{
		if (this.OnPreStateChangeEvent != null)
		{
			this.OnPreStateChangeEvent(layerIndex, previousStateIndex, newStateIndex);
		}
	}
	
	public void PostStateChange(int layerIndex, int previousStateIndex, int newStateIndex)
	{
		if (this.OnPostStateChangeEvent != null)
		{
			this.OnPostStateChangeEvent(layerIndex, previousStateIndex, newStateIndex);
		}
	}
	
	public void PreUpdate()
	{
		if (this.OnPreUpdateEvent != null)
		{
			this.OnPreUpdateEvent();
		}
	}
	
	public void PostUpdate()
	{
		if (this.OnPostUpdateEvent != null)
		{
			this.OnPostUpdateEvent();
		}
	}
	
	public void PreUpdateLayer(int layerIndex)
	{
		if (this.OnPreUpdateLayerEvent != null)
		{
			this.OnPreUpdateLayerEvent(layerIndex);
		}
	}
	
	public void PostUpdateLayer(int layerIndex)
	{
		if (this.OnPostUpdateLayerEvent != null)
		{
			this.OnPostUpdateLayerEvent(layerIndex);
		}
	}
}


