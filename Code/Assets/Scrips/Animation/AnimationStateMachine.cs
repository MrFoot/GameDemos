using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using Soulgame.Util;

public class AnimationStateMachine : MonoBehaviour {

	public enum DebugInfoType
	{
		None,
		Default,
		Detailed
	}

	public enum ParameterType
	{
		Int,
		Float,
		Bool,
		BoolTrigger,
		IntTrigger
	}

	public enum ConditionMode
	{
		Less,
		LessOrEqual,
		Equal,
		GreaterOrEqual,
		Greater,
		NotEqual,
		BitSet,
		BitNotSet
	}

	[Serializable]
	public class BlendTreeArea
	{
		public int a;
		
		public int b;
		
		public int c;
		
		public BlendTreeArea(int _a, int _b, int _c)
		{
			this.a = _a;
			this.b = _b;
			this.c = _c;
		}
	}

	[Serializable]
	public class Base
	{
		public string Name; 
	}

	[Serializable]
	public class Parameter : AnimationStateMachine.Base
	{
		public AnimationStateMachine.ParameterType ParameterType;
		
		public float ValueFloat;
		
		public int ValueInt;
		
		public void SetInt(int i)
		{
			this.ValueInt = i;
		}
		
		public void SetFloat(float f)
		{
			this.ValueFloat = f;
		}
		
		public void SetBool(bool b)
		{
			this.ValueInt = ((!b) ? 0 : 1);
		}
		
		public void SetBoolTrigger()
		{
			this.SetBool(true);
		}
		
		public void SetIntTrigger(int i)
		{
			this.SetInt(i);
		}
		
		public void ResetTrigger()
		{
			this.ValueFloat = 0f;
			this.ValueInt = 0;
		}
	}

	[Serializable]
	public class Condition : AnimationStateMachine.Base
	{
		public int ParameterIndex = -1;
		
		public float ValueFloat;
		
		public int ValueInt;
		
		public int ValueParameterIndex = -1; 
		
		public AnimationStateMachine.ConditionMode ConditionMode;
		
		public AnimationStateMachine.Parameter Parameter
		{
			get;
			set;
		}
		
		public AnimationStateMachine.Parameter ValueParameter
		{
			get;
			set;
		}
		
		public void ResetTrigger()
		{
			if (this.Parameter.ParameterType != AnimationStateMachine.ParameterType.BoolTrigger && this.Parameter.ParameterType != AnimationStateMachine.ParameterType.IntTrigger)
			{
				return;
			}
			this.Parameter.ResetTrigger();
		}
	}

	[Serializable]
	public class Transition : AnimationStateMachine.Base
	{
		public int DestinationStateIndex = -1;
		
		public bool Enabled = true;

		/// <summary>
		/// 能否被中断
		/// </summary>
		public bool Atomic = true;
		
		public bool UpdateTime = true;
		
		public float Duration;
		
		public AnimationCurve AnimationCurve;
		
		public AnimationStateMachine.Condition[] Conditions; 
		
		public AnimationClipEvent.Event[] PrePlayEvents;
		
		public AnimationClipEvent.Event[] PostPlayEvents;
		
		public AnimationStateMachine.State DestinationState
		{
			get;
			set;
		}
		
		public void ResetTriggers()
		{
			int num = 0;
			while (this.Conditions != null && num < this.Conditions.Length)
			{
				this.Conditions[num].ResetTrigger();
				num++;
			}
		}
	}

	[Serializable]
	public class ClipInfo
	{
		public string AnimationClipName;
		
		public AnimationClipInfo AnimationClipInfo;
	}
	
	[Serializable]
	public class AnimationSetClipInfo
	{
		public AnimationStateMachine.ClipInfo[] ClipInfos;
	}
	
	[Serializable]
	public class BlendTreePoint
	{
		public Vector2 Position;
		
		public AnimationStateMachine.AnimationSetClipInfo[] AnimationSetClipInfos;
		
		public bool IsValid(int animationSetIndex)
		{
			return this.AnimationSetClipInfos[animationSetIndex].ClipInfos[0].AnimationClipName.Length > 0;
		}
		
		public AnimationStateMachine.ClipInfo GetClipInfo(int animationSetIndex, int index)
		{
			AnimationStateMachine.AnimationSetClipInfo animationSetClipInfo = this.AnimationSetClipInfos[animationSetIndex];
			return (animationSetClipInfo.ClipInfos.Length == 0) ? null : animationSetClipInfo.ClipInfos[index % animationSetClipInfo.ClipInfos.Length];
		}
	}

	[Serializable]
	public class MixTransform
	{
		public string TransformName;
		
		public Transform Transform;
		
		public bool Recursive;
	}
	
	[Serializable]
	public class OverrideLayerWeightData
	{
		public int LayerIndex;
		
		public string LayerName;
		
		public AnimationCurve WeightCurve;
	}
	
	[Serializable]
	public class AnimationSet : AnimationStateMachine.Base
	{
	}
	
	public struct ActiveAnimation
	{
		public AnimationState AnimationState;
		
		public AnimationStateMachine.ClipInfo ClipInfo;
		
		public bool DestroyOnStop;
	}

	[Serializable]
	public class Layer : AnimationStateMachine.Base
	{
		private const int MaxIterationCount = 10;
		
		public string Comment;
		
		public AnimationBlendMode AnimationBlendMode;
		
		public float Weight = 1f;
		
		public AnimationStateMachine.State[] States;
		
		public int DefaultStateIndex = -1;
		
		public AnimationStateMachine.MixTransform[] MixTransforms;
		
		public Vector2 EditorPositionOffset = Vector2.zero;
		
		public bool Enabled = true;
		
		public float NormalizedTime;
		
		public float PreviousNormalizedTime;
		
		public float CurrentNormalizedTime;
		
		public float PreviousNormalizedSpeed = 1f;
		
		public float CurrentNormalizedSpeed = 1f;
		
		public float Speed = 1f;
		
		public float TransitionSpeed = 1f;
		
		public float CurrentStateWeight = 1f;
		
		public float MaxPreviousStateWeight;
		
		public float MaxCurrentStateWeight;
		
		public int AnimationIndex;
		
		public int LoopCount;
		
		public List<AnimationStateMachine.ActiveAnimation> PreviousActiveAnimations;
		
		public List<AnimationStateMachine.ActiveAnimation> CurrentActiveAnimations;
		
		public List<AnimationStateMachine.Transition> Transitions;
		
		public AnimationStateMachine.State DefaultState
		{
			get;
			set;
		}
		
		public AnimationStateMachine.State PreviousState
		{
			get;
			set;
		}
		
		public AnimationStateMachine.State CurrentState
		{
			get;
			set;
		}
		
		public int PreviousStateIndex
		{
			get;
			set;
		}
		
		public int CurrentStateIndex
		{
			get;
			set;
		}
		
		public AnimationStateMachine.Transition CurrentTransition
		{
			get;
			set;
		}
		
		private Transform FindTransform(Transform root, string name)
		{
			if (root.name == name)
			{
				return root;
			}
			for (int i = 0; i < root.childCount; i++)
			{
				Transform transform = this.FindTransform(root.GetChild(i), name);
				if (transform != null)
				{
					return transform;
				}
			}
			return null;
		}
		
		private void InitializeEvents(AnimationClipEvent.Event[] events)
		{
			for (int i = 0; i < events.Length; i++)
			{
				AnimationClipEvent.Event @event = events[i];
				@event.Initialize();
			}
		}
		
		public void Initialize()
		{
			this.PreviousActiveAnimations = new List<AnimationStateMachine.ActiveAnimation>();
			this.CurrentActiveAnimations = new List<AnimationStateMachine.ActiveAnimation>();
			this.Transitions = new List<AnimationStateMachine.Transition>();
			for (int i = 0; i < this.States.Length; i++)
			{
				AnimationStateMachine.State state = this.States[i];
				this.InitializeEvents(state.EnterEvents);
				this.InitializeEvents(state.ExitEvents);
				for (int j = 0; j < state.Transitions.Length; j++)
				{
					AnimationStateMachine.Transition transition = state.Transitions[j];
					this.InitializeEvents(transition.PrePlayEvents);
					this.InitializeEvents(transition.PostPlayEvents);
				}
			}
		}
		
		public void InitializeMixTransforms(Animation animation)
		{
			for (int i = 0; i < this.MixTransforms.Length; i++)
			{
				AnimationStateMachine.MixTransform mixTransform = this.MixTransforms[i];
				mixTransform.Transform = this.FindTransform(animation.transform, mixTransform.TransformName);
				if (mixTransform.Transform == null)
				{
					GameLog.ErrorT("AnimationStateMachine", "MixTransform {0} not found on {1}!", new object[] {
						mixTransform.TransformName,
						animation.name
					});
				}
			}
		}
		
		public void Reset(Animation animation, MonoBehaviour[] monoBehaviours)
		{
			this.TriggerStopEvents(this.CurrentActiveAnimations, monoBehaviours);
			this.AnimationIndex = 0;
			this.PreviousState = null;
			this.CurrentState = null;
			this.PreviousStateIndex = -1;
			this.CurrentStateIndex = -1;
			this.StopAnimationStates(animation, this.PreviousActiveAnimations);
			this.StopAnimationStates(animation, this.CurrentActiveAnimations);
			this.PreviousActiveAnimations.Clear();
			this.CurrentActiveAnimations.Clear();
			this.Transitions.Clear();
		}
		
		public bool CheckCondition(AnimationStateMachine.Condition condition)
		{
			if (condition.Parameter.ParameterType == AnimationStateMachine.ParameterType.Float)
			{
				float num = (condition.ValueParameter == null) ? condition.ValueFloat : condition.ValueParameter.ValueFloat;
				switch (condition.ConditionMode)
				{
				case AnimationStateMachine.ConditionMode.Less:
					return condition.Parameter.ValueFloat < num;
				case AnimationStateMachine.ConditionMode.LessOrEqual:
					return condition.Parameter.ValueFloat <= num;
				case AnimationStateMachine.ConditionMode.Equal:
					return Mathf.Abs(condition.Parameter.ValueFloat - num) < 0.0001f;
				case AnimationStateMachine.ConditionMode.GreaterOrEqual:
					return condition.Parameter.ValueFloat >= num;
				case AnimationStateMachine.ConditionMode.Greater:
					return condition.Parameter.ValueFloat > num;
				case AnimationStateMachine.ConditionMode.NotEqual:
					return Mathf.Abs(condition.Parameter.ValueFloat - num) >= 0.0001f;
				case AnimationStateMachine.ConditionMode.BitSet:
					Assert.IsTrue(false, "BitSet cannot be used with float parameter!", new object[0]);
					break;
				case AnimationStateMachine.ConditionMode.BitNotSet:
					Assert.IsTrue(false, "BitNotSet cannot be used with float parameter!", new object[0]);
					break;
				}
			}
			else
			{
				int num2 = (condition.ValueParameter == null) ? condition.ValueInt : condition.ValueParameter.ValueInt;
				switch (condition.ConditionMode)
				{
				case AnimationStateMachine.ConditionMode.Less:
					return condition.Parameter.ValueInt < num2;
				case AnimationStateMachine.ConditionMode.LessOrEqual:
					return condition.Parameter.ValueInt <= num2;
				case AnimationStateMachine.ConditionMode.Equal:
					return condition.Parameter.ValueInt == num2;
				case AnimationStateMachine.ConditionMode.GreaterOrEqual:
					return condition.Parameter.ValueInt >= num2;
				case AnimationStateMachine.ConditionMode.Greater:
					return condition.Parameter.ValueInt > num2;
				case AnimationStateMachine.ConditionMode.NotEqual:
					return condition.Parameter.ValueInt != num2;
				case AnimationStateMachine.ConditionMode.BitSet:
					return (condition.Parameter.ValueInt & 1 << num2) > 0;
				case AnimationStateMachine.ConditionMode.BitNotSet:
					return (condition.Parameter.ValueInt & 1 << num2) == 0;
				}
			}
			return false;
		}
		
		private AnimationStateMachine.Transition CheckStateTransitions(AnimationStateMachine.State state) 
		{
			for (int i = 0; i < state.Transitions.Length; i++)
			{
				AnimationStateMachine.Transition transition = state.Transitions[i];
				if (transition.Enabled)
				{
					bool flag = true;
					for (int j = 0; j < transition.Conditions.Length; j++)
					{
						AnimationStateMachine.Condition condition = transition.Conditions[j];
						if (!this.CheckCondition(condition))
						{
							flag = false;
							break;
						}
					}
					if (flag)
					{
						return transition;
					}
				}
			}
			return null;
		}
		
		private float UpdateWeight(float weight, float weightTarget, float weightSpeed, float deltaTime)
		{
			if (weightSpeed <= 0f)
			{
				return weightTarget;
			}
			if (weight < weightTarget)
			{
				return Mathf.Min(weight + weightSpeed * deltaTime, weightTarget);
			}
			return Mathf.Max(weight - weightSpeed * deltaTime, weightTarget);
		}
		
		private void UpdateAnimationState(AnimationStateMachine.ActiveAnimation activeAnimation, float lastStateNormalizedTime, float stateNormalizedTime, bool looped, MonoBehaviour[] monoBehaviours)
		{
			float num = lastStateNormalizedTime;
			float num2 = stateNormalizedTime;
			if (this.AnimationBlendMode == AnimationBlendMode.Additive)
			{
				float num3 = AnimationStateMachine.AdditiveCutTime * activeAnimation.ClipInfo.AnimationClipInfo.ooLength;
				float num4 = 1f - num3;
				num = num * num4 + num3;
				num2 = num2 * num4 + num3;
			}
			if (activeAnimation.ClipInfo.AnimationClipInfo.AnimationClipEvent != null && monoBehaviours != null)
			{
				float minTime = (this.AnimationBlendMode != AnimationBlendMode.Additive) ? 0f : 0.0333343334f;
				float length = activeAnimation.AnimationState.length;
				float startTime = num * length;
				float endTime = num2 * length;
				if (looped)
				{
					activeAnimation.ClipInfo.AnimationClipInfo.AnimationClipEvent.UpdateEvents(startTime, length, minTime, monoBehaviours);
					activeAnimation.ClipInfo.AnimationClipInfo.AnimationClipEvent.UpdateEvents(0f, endTime, minTime, monoBehaviours);
				}
				else
				{
					activeAnimation.ClipInfo.AnimationClipInfo.AnimationClipEvent.UpdateEvents(startTime, endTime, minTime, monoBehaviours);
				}
			}
			activeAnimation.AnimationState.normalizedTime = num2;
		}
		
		private void UpdateAnimationTime(AnimationStateMachine.State state, ref float normalizedTime, ref int loopCount, float normalizedSpeed, List<AnimationStateMachine.ActiveAnimation> activeAnimations, MonoBehaviour[] monoBehaviours)
		{
			float time = normalizedTime;
			bool flag = false;
			if (state.UseNormalizedTime)
			{
				if (monoBehaviours != null)
				{
					time = this.NormalizedTime;
					normalizedTime = this.NormalizedTime;
				}
			}
			else
			{
				if (this.AnimationBlendMode == AnimationBlendMode.Additive && activeAnimations.Count == 1)
				{
					float length = activeAnimations[0].ClipInfo.AnimationClipInfo.AnimationClip.length;
					if (length > AnimationStateMachine.AdditiveCutTime)
					{
						float num = length / (length - AnimationStateMachine.AdditiveCutTime);
						normalizedSpeed *= num;
					}
				}
				normalizedTime += normalizedSpeed;
				if (state.Loop)
				{
					if (normalizedTime >= state.LoopRange.y)
					{
						flag = true;
						loopCount++;
						float num2 = state.LoopRange.y - state.LoopRange.x;
						normalizedTime = ((num2 <= 0f) ? state.LoopRange.x : (normalizedTime - num2));
					}
				}
				else if (normalizedTime > state.PlayRange.y)
				{
					normalizedTime = state.PlayRange.y;
				}
			}
			float lastStateNormalizedTime = state.TimeAnimationCurve.Evaluate(time);
			float stateNormalizedTime = state.TimeAnimationCurve.Evaluate(normalizedTime);
			for (int i = 0; i < activeAnimations.Count; i++)
			{
				this.UpdateAnimationState(activeAnimations[i], lastStateNormalizedTime, stateNormalizedTime, flag, monoBehaviours);
			}
			if (this.AnimationBlendMode == AnimationBlendMode.Additive && flag)
			{
				for (int j = 0; j < activeAnimations.Count; j++)
				{
					normalizedTime = Mathf.Max(normalizedTime, activeAnimations[j].AnimationState.normalizedTime);
				}
			}
		}
		
		private void StopAnimationStates(Animation animation, List<AnimationStateMachine.ActiveAnimation> activeAnimations)
		{
			for (int i = 0; i < activeAnimations.Count; i++)
			{
				AnimationStateMachine.ActiveAnimation activeAnimation = activeAnimations[i];
				AnimationClip clip = activeAnimation.AnimationState.clip;
				if (!(clip == null))
				{
					if (animation != null)
					{
						animation.Stop(clip.name);
						animation.RemoveClip(clip.name);
					}
					if (activeAnimation.DestroyOnStop)
					{
						UnityEngine.Object.DestroyImmediate(clip);
					}
				}
			}
		}
		
		private void UpdateWeights(Animation animation, float deltaTime, AnimationStateMachine.Parameter timeParameter, MonoBehaviour[] monoBehaviours, AnimationStateMachine.Layer[] layers, int layerIndex)
		{
			float num = 1f;
			if (this.CurrentState != null)
			{
				float num2 = this.CurrentNormalizedSpeed * this.CurrentState.Speed * this.Speed * deltaTime;
				float num3 = 1f;
				this.CurrentStateWeight = this.UpdateWeight(this.CurrentStateWeight, 1f, this.TransitionSpeed * this.Speed, deltaTime);
				num = ((this.CurrentTransition == null || this.CurrentTransition.AnimationCurve == null) ? this.CurrentStateWeight : this.CurrentTransition.AnimationCurve.Evaluate(this.CurrentStateWeight));
				if (num >= 1f)
				{
					this.CurrentTransition = null;
				}
				this.MaxCurrentStateWeight = num;
				if (this.CurrentTransition != null && !this.CurrentTransition.UpdateTime)
				{
					num3 = 0f;
				}
				if (this.CurrentActiveAnimations.Count > 0)
				{
					this.UpdateAnimationTime(this.CurrentState, ref this.CurrentNormalizedTime, ref this.LoopCount, num2 * num3, this.CurrentActiveAnimations, monoBehaviours);
					if (this.CurrentState.TimeInSeconds)
					{
						timeParameter.ValueFloat += deltaTime * num3;
					}
					else
					{
						timeParameter.ValueFloat = (float)this.LoopCount + this.CurrentNormalizedTime;
					}
					for (int i = 0; i < this.CurrentActiveAnimations.Count; i++)
					{
						this.CurrentActiveAnimations[i].AnimationState.weight *= num * this.CurrentState.Weight;
					}
				}
				else
				{
					timeParameter.ValueFloat += deltaTime * num3;
				}
			}
			if (this.PreviousState != null)
			{
				float num4 = (1f - num) * this.MaxPreviousStateWeight;
				float normalizedSpeed = this.PreviousNormalizedSpeed * this.PreviousState.Speed * this.Speed * deltaTime;
				int num5 = 0;
				this.UpdateAnimationTime(this.PreviousState, ref this.PreviousNormalizedTime, ref num5, normalizedSpeed, this.PreviousActiveAnimations, null);
				for (int j = 0; j < this.PreviousActiveAnimations.Count; j++)
				{
					this.PreviousActiveAnimations[j].AnimationState.weight *= num4 * this.PreviousState.Weight;
				}
				if (num4 <= Mathf.Epsilon)
				{
					this.StopAnimationStates(animation, this.PreviousActiveAnimations);
					this.PreviousActiveAnimations.Clear();
					this.PreviousNormalizedSpeed = 0f;
					this.PreviousNormalizedTime = 0f;
					this.MaxPreviousStateWeight = 0f;
					this.PreviousState = null;
				}
			}
		}
		
		private void SetActiveAnimationsUsed(List<AnimationStateMachine.ActiveAnimation> activeAnimations)
		{
			for (int i = 0; i < activeAnimations.Count; i++)
			{
				AnimationStateMachine.ActiveAnimation activeAnimation = activeAnimations[i];
				if (activeAnimation.ClipInfo.AnimationClipInfo != null)
				{
					activeAnimation.ClipInfo.AnimationClipInfo.SetUsed();
				}
			}
		}
		
		public void Update(Animation animation, float deltaTime, AnimationStateMachine.Parameter timeParameter, int index, int animationSetIndex, ref AnimationStateMachineCallbacks callbacks, MonoBehaviour[] monoBehaviours, AnimationStateMachine.Layer[] layers, bool debugInfo)
		{
			if (!this.Enabled)
			{
				this.Reset(animation, monoBehaviours);
				return;
			}
			if (this.DefaultState == null)
			{
				throw new InvalidProgramException("Default state is null!");
			}
			int num = 0;
			while (this.CurrentState == null || this.CurrentState.Switch || this.CurrentTransition == null || !this.CurrentTransition.Atomic)
			{
				AnimationStateMachine.Transition transition = null;
				if (this.CurrentState == null)
				{
					transition = new AnimationStateMachine.Transition();
					transition.DestinationState = this.DefaultState;
					transition.DestinationStateIndex = this.DefaultStateIndex;
					transition.Duration = 0f;
				}
				else
				{
					if (num == 0)
					{
						transition = this.CheckStateTransitions(this.States[0]);
					}
					if (transition == null)
					{
						transition = this.CheckStateTransitions(this.CurrentState);
					}
				}
				if (transition == null)
				{
					break;
				}
				callbacks.PreStateChange(index, this.CurrentStateIndex, transition.DestinationStateIndex);
				timeParameter.ValueFloat = 0f;
				this.OnTransition(animation, timeParameter, transition, index, animationSetIndex, ref callbacks, monoBehaviours);
				deltaTime = 0f;
				callbacks.PostStateChange(index, this.PreviousStateIndex, this.CurrentStateIndex);
				num++;
				if (num >= 10)
				{
					throw new InvalidProgramException("Infinite loop!");
				}
			}

			if (this.CurrentState.Switch)
			{
				timeParameter.ValueFloat += deltaTime;
			}
			else
			{
				if (this.PreviousState != null)
				{
					this.PreviousState.UpdateBlendTree(this.PreviousActiveAnimations, ref this.PreviousNormalizedSpeed, this);
				}
				this.CurrentState.UpdateBlendTree(this.CurrentActiveAnimations, ref this.CurrentNormalizedSpeed, this);
				this.UpdateWeights(animation, deltaTime, timeParameter, monoBehaviours, layers, index);
			}
			this.SetActiveAnimationsUsed(this.PreviousActiveAnimations);
			this.SetActiveAnimationsUsed(this.CurrentActiveAnimations);
		}
		
		public void PostUpdate()
		{
			for (int i = 0; i < this.Transitions.Count; i++)
			{
				this.Transitions[i].ResetTriggers();
			}
			this.Transitions.Clear();
		}
		
		private void TriggerStopEvents(List<AnimationStateMachine.ActiveAnimation> activeAnimations, MonoBehaviour[] monoBehaviours)
		{
			for (int i = 0; i < activeAnimations.Count; i++)
			{
				AnimationStateMachine.ActiveAnimation activeAnimation = activeAnimations[i];
				if (!(activeAnimation.ClipInfo.AnimationClipInfo.AnimationClipEvent == null))
				{
					activeAnimation.ClipInfo.AnimationClipInfo.AnimationClipEvent.OnExit(monoBehaviours);
				}
			}
		}
		
		private void TriggerStateEvents(AnimationClipEvent.Event[] events, MonoBehaviour[] monoBehaviours)
		{
			if (events == null)
			{
				return;
			}
			for (int i = 0; i < events.Length; i++)
			{
				AnimationClipEvent.Event @event = events[i];
				@event.Invoke(monoBehaviours);
			}
		}
		
		private void OnTransition(Animation animation, AnimationStateMachine.Parameter timeParameter, AnimationStateMachine.Transition transition, int index, int animationSetIndex, ref AnimationStateMachineCallbacks callbacks, MonoBehaviour[] monoBehaviours)
		{
			this.TriggerStopEvents(this.CurrentActiveAnimations, monoBehaviours);
			if (this.CurrentState != null)
			{
				this.TriggerStateEvents(this.CurrentState.ExitEvents, monoBehaviours);
			}
			if (this.CurrentState != null && !this.CurrentState.Switch)
			{
				List<AnimationStateMachine.ActiveAnimation> previousActiveAnimations = this.PreviousActiveAnimations;
				this.PreviousActiveAnimations = this.CurrentActiveAnimations;
				this.CurrentActiveAnimations = previousActiveAnimations;
				this.PreviousNormalizedSpeed = this.CurrentNormalizedSpeed;
				this.CurrentNormalizedSpeed = 0f;
				this.PreviousNormalizedTime = this.CurrentNormalizedTime;
				this.CurrentNormalizedTime = 0f;
				this.MaxPreviousStateWeight = this.MaxCurrentStateWeight;
				this.MaxCurrentStateWeight = 0f;
				this.PreviousState = this.CurrentState;
				this.PreviousStateIndex = this.CurrentStateIndex;
			}
			else if (this.CurrentState == null)
			{
				this.TransitionSpeed = 0f;
			}
			if (this.CurrentState != null && (!this.CurrentState.Switch || this.CurrentTransition == null || transition.Duration > this.CurrentTransition.Duration))
			{
				this.CurrentTransition = transition;
				this.TransitionSpeed = ((transition.Duration <= 0f) ? 0f : (1f / transition.Duration));
			}
			this.LoopCount = 0;
			this.CurrentState = transition.DestinationState;
			this.CurrentStateIndex = transition.DestinationStateIndex;
			this.CurrentNormalizedTime = this.CurrentState.PlayRange.x;
			this.TriggerStateEvents(this.CurrentState.EnterEvents, monoBehaviours);
			if (transition != null)
			{
				this.TriggerStateEvents(transition.PrePlayEvents, monoBehaviours);
			}
			if (!this.CurrentState.Switch)
			{
				this.CurrentStateWeight = ((this.TransitionSpeed <= 0f) ? 1f : 0f);
				if (this.TransitionSpeed <= 0f)
				{
					this.StopAnimationStates(animation, this.PreviousActiveAnimations);
					this.PreviousActiveAnimations.Clear();
				}
				this.StopAnimationStates(animation, this.CurrentActiveAnimations);
				this.CurrentActiveAnimations.Clear();
				if (this.CurrentState.Play(animation, this, index, animationSetIndex, ref callbacks, monoBehaviours))
				{
					this.AnimationIndex = ((this.AnimationIndex <= 0) ? 1 : 0);
				}
			}
			if (transition != null)
			{
				this.TriggerStateEvents(transition.PostPlayEvents, monoBehaviours);
			}
			this.Transitions.Add(transition);
		}
		
		public int FindStateIndex(string name)
		{
			return AnimationStateMachine.FindBaseIndex<AnimationStateMachine.State>(name, this.States);
		}
		
		public bool StopLoop()
		{
			if (this.CurrentState == null || !this.CurrentState.CanStopLoop)
			{
				return false;
			}
			for (int i = 0; i < this.CurrentActiveAnimations.Count; i++)
			{
				this.CurrentActiveAnimations[i].AnimationState.wrapMode = WrapMode.ClampForever;
			}
			return true;
		}
		
		private AnimationStateMachine.Parameter GetParameterByIndex(int parameterIndex, AnimationStateMachine.Parameter[] parameters)
		{
			return (parameterIndex == -1) ? null : parameters[parameterIndex];
		}
		
		private AnimationStateMachine.State GetStateByIndex(int stateIndex, AnimationStateMachine.State[] states)
		{
			return (stateIndex == -1) ? null : states[stateIndex];
		}
		
		public void UpdateReferences(AnimationStateMachine.Parameter[] parameters)
		{
			this.DefaultState = ((this.DefaultStateIndex == -1) ? null : this.States[this.DefaultStateIndex]);
			for (int i = 0; i < this.States.Length; i++)
			{
				AnimationStateMachine.State state = this.States[i];
				state.ParameterX = this.GetParameterByIndex(state.ParameterXIndex, parameters);
				state.ParameterY = this.GetParameterByIndex(state.ParameterYIndex, parameters);
				state.ParameterIndex = this.GetParameterByIndex(state.ParameterIndexIndex, parameters);
				for (int j = 0; j < state.Transitions.Length; j++)
				{
					AnimationStateMachine.Transition transition = state.Transitions[j];
					transition.DestinationState = this.GetStateByIndex(transition.DestinationStateIndex, this.States);
					for (int k = 0; k < transition.Conditions.Length; k++)
					{
						AnimationStateMachine.Condition condition = transition.Conditions[k];
						condition.Parameter = this.GetParameterByIndex(condition.ParameterIndex, parameters);
						condition.ValueParameter = this.GetParameterByIndex(condition.ValueParameterIndex, parameters);
					}
				}
			}
		}
	}
	
	[Serializable]
	public class State : AnimationStateMachine.Base
	{
		public enum PriorityEnum
		{
			Low,
			Normal,
			High
		}
		
		public string Comment;
		
		public AnimationStateMachine.Transition[] Transitions;
		
		public Vector2 EditorPosition;
		
		public AnimationStateMachine.BlendTreePoint[] BlendTreePoints;
		
		public AnimationStateMachine.BlendTreeArea[] BlendTreeAreas;
		
		public AnimationCurve TimeAnimationCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
		
		public int ParameterXIndex = -1;
		
		public int ParameterYIndex = -1;
		
		public int ParameterIndexIndex = -1;
		
		public bool UseNormalizedTime;
		
		public bool Loop;
		
		public bool CanStopLoop;
		
		public Vector2 PlayRange = new Vector2(0f, 1f);
		
		public Vector2 LoopRange = new Vector2(0f, 1f);
		
		public bool Switch;
		
		public bool TimeInSeconds;
		
		public int AttributeMask;
		
		public AnimationClipEvent.Event[] EnterEvents;
		
		public AnimationClipEvent.Event[] ExitEvents;
		
		public float Speed = 1f;
		
		public float Weight = 1f;
		
		public AnimationStateMachine.State.PriorityEnum Priority = AnimationStateMachine.State.PriorityEnum.Normal;
		
		public AnimationStateMachine.Parameter ParameterX
		{
			get;
			set;
		}
		
		public AnimationStateMachine.Parameter ParameterY
		{
			get;
			set;
		}
		
		public AnimationStateMachine.Parameter ParameterIndex
		{
			get;
			set;
		}
		
		public bool Play(Animation animation, AnimationStateMachine.Layer layer, int layerIndex, int animationSetIndex, ref AnimationStateMachineCallbacks callbacks, MonoBehaviour[] monoBehaviours)
		{
			if (!this.BlendTreePoints[0].IsValid(0))
			{
				if (layerIndex == 0)
				{
					GameLog.ErrorT("AnimationStateMachine", "Base layer State {0} without animation!", new object[]{
						this.Name
					});
				}
				return false;
			}
			int num = (this.ParameterIndex != null) ? this.ParameterIndex.ValueInt : -1;
			if (num < 0)
			{
				num = UnityEngine.Random.Range(0, 100);
			}
			for (int i = 0; i < this.BlendTreePoints.Length; i++)
			{
				AnimationStateMachine.BlendTreePoint blendTreePoint = this.BlendTreePoints[i];
				AnimationStateMachine.ClipInfo clipInfo = blendTreePoint.GetClipInfo(animationSetIndex, num);
				if (animationSetIndex > 0 && (clipInfo == null || clipInfo.AnimationClipName.Length == 0))
				{
					clipInfo = blendTreePoint.GetClipInfo(0, num);
				}
				if (clipInfo == null)
				{
					GameLog.ErrorT("AnimationStateMachine", "State {0} without animation!", new object[] {
						this.Name
					});
				}
				else
				{
					AnimationStateMachine.ActiveAnimation item = this.PlayClip(animation, clipInfo, layer, layerIndex, layer.AnimationIndex * 10 + i, ref callbacks);
					if (clipInfo.AnimationClipInfo.AnimationClipEvent != null)
					{
						clipInfo.AnimationClipInfo.AnimationClipEvent.OnEnter(monoBehaviours);
					}
					layer.CurrentActiveAnimations.Add(item);
				}
			}
			return true;
		}
		
		public AnimationStateMachine.ActiveAnimation PlayClip(Animation animation, AnimationStateMachine.ClipInfo clipInfo, AnimationStateMachine.Layer layer, int layerIndex, int animationIndex, ref AnimationStateMachineCallbacks callbacks)
		{
			clipInfo.AnimationClipInfo = callbacks.LoadAnimationClipInfo(clipInfo.AnimationClipName, this.Priority);
			Assert.NotNull(clipInfo.AnimationClipInfo, "PlayClip", "AnimationClipInfo {0} for state {0}", new object[] {
				clipInfo.AnimationClipName,
				this.Name
			});
			Assert.NotNull(clipInfo.AnimationClipInfo.AnimationClipAsset, "PlayClip", "AnimationClipInfo.AnimationClipAsset {0} for state {1}", new object[] {
				clipInfo.AnimationClipName,
				this.Name
			});
			Assert.NotNull(clipInfo.AnimationClipInfo.AnimationClip, "PlayClip", "AnimationClipInfo.AnimationClip {0} for state {1}", new object[]{
				clipInfo.AnimationClipName,
				this.Name
			});
			bool destroyOnStop = false;
			AnimationClip animationClip = clipInfo.AnimationClipInfo.AnimationClip;
			int num = layerIndex * 20 + animationIndex;
			AnimationState animationState = animation[animationClip.name];
			if (animationState != null && animationState.layer != num)
			{
				animationClip = UnityEngine.Object.Instantiate<AnimationClip>(animationClip);
				animationClip.name = string.Format("{0}_Layer#{1}", animationClip.name, num);
				destroyOnStop = true;
			}
			animation.AddClip(animationClip, animationClip.name);
			animationState = animation[animationClip.name];
			animationState.blendMode = layer.AnimationBlendMode;
			animationState.wrapMode = WrapMode.ClampForever;
			animationState.layer = num;
			for (int i = 0; i < layer.MixTransforms.Length; i++)
			{
				AnimationStateMachine.MixTransform mixTransform = layer.MixTransforms[i];
				if (!(mixTransform.Transform == null))
				{
					animationState.AddMixingTransform(mixTransform.Transform, mixTransform.Recursive);
				}
			}
			animation.Play(animationClip.name, PlayMode.StopSameLayer);
			animationState.speed = 0f;
			animationState.weight = 0f;
			animationState.enabled = true;
			return new AnimationStateMachine.ActiveAnimation
			{
				AnimationState = animationState,
				ClipInfo = clipInfo,
				DestroyOnStop = destroyOnStop
			};
		}
		
		public int FindBestBlendTreeArea(Vector2 position, out Vector2 bestBc)
		{
			int result = -1;
			float num = 1000f;
			bestBc = Vector3.zero;
			for (int i = 0; i < this.BlendTreeAreas.Length; i++)
			{
				AnimationStateMachine.BlendTreeArea blendTreeArea = this.BlendTreeAreas[i];
				Vector3 a = this.BlendTreePoints[blendTreeArea.a].Position;
				Vector3 b = this.BlendTreePoints[blendTreeArea.b].Position;
				Vector3 c = this.BlendTreePoints[blendTreeArea.c].Position;
				Vector2 vector;
				Vector2 v = TriangleUtils.GetClosestPointOnTriangle(a, b, c, position, out vector);
				float num2 = Vector3.Distance(v, position);
				if (num2 <= num)
				{
					num = num2;
					bestBc = vector;
					result = i;
				}
			}
			return result;
		}
		
		public void UpdateBlendTree(List<AnimationStateMachine.ActiveAnimation> activeAnimations, ref float normalizedSpeed, AnimationStateMachine.Layer layer)
		{
			if (activeAnimations.Count == 0)
			{
				return;
			}
			if (this.BlendTreePoints.Length == 1)
			{
				activeAnimations[0].AnimationState.weight = layer.Weight;
				normalizedSpeed = 1f / activeAnimations[0].AnimationState.length;
			}
			else if (this.BlendTreeAreas.Length == 0)
			{
				float num = this.ParameterX.ValueFloat;
				int num2 = 0;
				for (int i = 0; i < this.BlendTreePoints.Length; i++)
				{
					if (num > this.BlendTreePoints[i].Position.x)
					{
						num2 = i;
					}
				}
				int num3 = Mathf.Min(num2 + 1, this.BlendTreePoints.Length - 1);
				float x = this.BlendTreePoints[num2].Position.x;
				float x2 = this.BlendTreePoints[num3].Position.x;
				num = ((num2 != num3) ? (1f - (Mathf.Clamp(num, x, x2) - x) / (x2 - x)) : 1f);
				for (int j = 0; j < activeAnimations.Count; j++)
				{
					activeAnimations[j].AnimationState.weight = 0f;
				}
				activeAnimations[num2].AnimationState.weight = layer.Weight * num;
				if (num2 != num3)
				{
					activeAnimations[num3].AnimationState.weight = layer.Weight * (1f - num);
				}
				normalizedSpeed = 1f / Mathf.Lerp(activeAnimations[num2].AnimationState.length, activeAnimations[num3].AnimationState.length, num);
			}
			else
			{
				float valueFloat = this.ParameterX.ValueFloat;
				float valueFloat2 = this.ParameterY.ValueFloat;
				Vector3 v = new Vector3(valueFloat, valueFloat2, 0f);
				Vector2 bc = Vector3.zero;
				int num4 = this.FindBestBlendTreeArea(v, out bc);
				if (num4 != -1)
				{
					AnimationStateMachine.BlendTreeArea blendTreeArea = this.BlendTreeAreas[num4];
					for (int k = 0; k < activeAnimations.Count; k++)
					{
						activeAnimations[k].AnimationState.weight = 0f;
					}
					AnimationState animationState = activeAnimations[blendTreeArea.a].AnimationState;
					AnimationState animationState2 = activeAnimations[blendTreeArea.b].AnimationState;
					AnimationState animationState3 = activeAnimations[blendTreeArea.c].AnimationState;
					animationState.weight = layer.Weight * (1f - bc.x - bc.y);
					animationState2.weight = layer.Weight * bc.x;
					animationState3.weight = layer.Weight * bc.y;
					normalizedSpeed = 1f / TriangleUtils.GetPositionFromBarycentric(new Vector3(animationState.length, 0f, 0f), new Vector3(animationState2.length, 0f, 0f), new Vector3(animationState3.length, 0f, 0f), bc).x;
				}
			}
		}
	}

	private const int SmoothDeltaTimeCount = 10;
	
	public const string Tag = "AnimationStateMachine";
	
	private const float AdditiveCutTime = 0.0333333351f;
	
	private const int MaxAnimationsPerLayer = 10;
	
	private MonoBehaviour[] MonoBehaviours;
	
	private SmoothDeltaTime SmoothDeltaTime;
	
	public AnimationStateMachineCallbacks Callbacks;
	
	public AnimationStateMachine.Layer[] Layers;
	
	public AnimationStateMachine.Parameter[] Parameters;
	
	public string[] AttributeNames;
	
	public AnimationStateMachine.AnimationSet[] AnimationSets;
	
	public Animation TargetAnimation;
	
	public float Speed = 1f;
	
	public int CurrentAnimationSetIndex;
	
	public int EditorLayerIndex;
	
	public int CurrentLayerIndex
	{
		get;
		private set;
	}
	
	public AnimationStateMachine.DebugInfoType DebugInfo
	{
		get;
		set;
	}
	
	public static int FindBaseIndex<T>(string name, T[] bases) where T : AnimationStateMachine.Base
	{
		for (int i = 0; i < bases.Length; i++)
		{
			if (bases[i].Name == name)
			{
				return i;
			}
		}
		return -1;
	}
	
	public int FindParameterIndex(string name)
	{
		return AnimationStateMachine.FindBaseIndex<AnimationStateMachine.Parameter>(name, this.Parameters);
	}
	
	public int FindLayerIndex(string name)
	{
		return AnimationStateMachine.FindBaseIndex<AnimationStateMachine.Layer>(name, this.Layers);
	}
	
	public int FindLayerStateIndex(int layerIndex, string name)
	{
		return this.Layers[layerIndex].FindStateIndex(name);
	}
	
	public int GetLayerCurrentStateIndex(int index)
	{
		AnimationStateMachine.Layer layer = this.Layers[index];
		return layer.CurrentStateIndex;
	}
	
	public float GetLayerCurrentStateWeight(int index)
	{
		AnimationStateMachine.Layer layer = this.Layers[index];
		return layer.CurrentStateWeight;
	}
	
	public string GetLayerStateName(int layerIndex, int index)
	{
		return this.Layers[layerIndex].States[index].Name;
	}
	
	public int GetLayerStateAttributeMask(int index)
	{
		AnimationStateMachine.Layer layer = this.Layers[index];
		return (layer.CurrentState == null) ? 0 : layer.CurrentState.AttributeMask;
	}
	
	public void SetLayerNormalizedTime(int index, float time)
	{
		this.Layers[index].NormalizedTime = time;
	}
	
	public void SetLayerCurrentNormalizedTime(int index, float time)
	{
		this.Layers[index].CurrentNormalizedTime = time;
	}
	
	public float GetLayerCurrentNormalizedTime(int index)
	{
		return this.Layers[index].CurrentNormalizedTime;
	}
	
	public void GetLayerAnimationTime(int index, out float time, out float length)
	{
		AnimationStateMachine.Layer layer = this.Layers[index];
		if (layer.CurrentActiveAnimations.Count > 0)
		{
			time = layer.CurrentActiveAnimations[0].AnimationState.time;
			length = layer.CurrentActiveAnimations[0].ClipInfo.AnimationClipInfo.AnimationClip.length;
		}
		else
		{
			time = 0f;
			length = 0f;
		}
	}
	
	public bool IsLayerPlaying(int index)
	{
		return this.Layers[index].CurrentActiveAnimations.Count > 0;
	}
	
	public void SetLayerWeight(int index, float weight)
	{
		this.Layers[index].Weight = weight;
	}
	
	public float GetLayerWeight(int index)
	{
		return this.Layers[index].Weight;
	}
	
	public void EnableLayer(int index, bool enable)
	{
		this.Layers[index].Enabled = enable;
	}
	
	public int FindAnimationSetIndex(string name)
	{
		return AnimationStateMachine.FindBaseIndex<AnimationStateMachine.AnimationSet>(name, this.AnimationSets);
	}
	
	public void SetIntParameter(int index, int parameter)
	{
		this.Parameters[index].SetInt(parameter);
	}
	
	public int GetIntParameter(int index)
	{
		return this.Parameters[index].ValueInt;
	}
	
	public void SetFloatParameter(int index, float parameter)
	{
		this.Parameters[index].SetFloat(parameter);
	}
	
	public float GetFloatParameter(int index)
	{
		return this.Parameters[index].ValueFloat;
	}
	
	public void SetBoolParameter(int index, bool parameter)
	{
		this.Parameters[index].SetBool(parameter);
	}
	
	public void SetIntTriggerParameter(int index, int parameter)
	{
		this.Parameters[index].SetInt(parameter);
	}
	
	public void SetBoolTriggerParameter(int index)
	{
		this.Parameters[index].SetBoolTrigger();
	}
	
	public bool GetBoolParameter(int index)
	{
		return this.Parameters[index].ValueInt != 0;
	}
	
	public void ResetTriggerParameter(int index)
	{
		this.Parameters[index].ResetTrigger();
	}
	
	public void Reset()
	{
		for (int i = 0; i < this.Parameters.Length; i++)
		{
			this.Parameters[i].ResetTrigger();
		}
		for (int j = 0; j < this.Layers.Length; j++)
		{
			this.Layers[j].Reset(this.TargetAnimation, this.MonoBehaviours);
		}
		this.Callbacks.Reset();
	}
	
	public void UpdateMonoBehaviours()
	{
		this.MonoBehaviours = base.GetComponents<MonoBehaviour>();
	}
	
	public void CustomUpdate(float deltaTime)
	{
		if (this.TargetAnimation == null || !base.gameObject.activeInHierarchy)
		{
			return;
		}
		this.Callbacks.PreUpdate();
		for (int i = 0; i < this.Layers.Length; i++)
		{
			this.CurrentLayerIndex = i;
			this.Callbacks.PreUpdateLayer(i);
			this.Layers[i].Update(this.TargetAnimation, this.Speed * deltaTime, this.Parameters[i], i, this.CurrentAnimationSetIndex, ref this.Callbacks, this.MonoBehaviours, this.Layers, this.DebugInfo != AnimationStateMachine.DebugInfoType.None);
			this.Callbacks.PostUpdateLayer(i);
		}
		for (int j = 0; j < this.Layers.Length; j++)
		{
			this.Layers[j].PostUpdate();
		}
		this.Callbacks.PostUpdate();
	}
	
	public void SetTargetAnimation(Animation animation)
	{
		this.TargetAnimation = animation;
		if (this.TargetAnimation != null)
		{
			for (int i = 0; i < this.Layers.Length; i++)
			{
				AnimationStateMachine.Layer layer = this.Layers[i];
				layer.InitializeMixTransforms(this.TargetAnimation);
			}
		}
	}
	
	public Animation GetTargetAnimation()
	{
		return this.TargetAnimation;
	}
	
	public AnimationClipPair LoadAnimationClipInfo(string name)
	{
		return null;
	}
	
	private void Awake()
	{
		this.SmoothDeltaTime = new SmoothDeltaTime(10);
		for (int i = 0; i < this.Layers.Length; i++)
		{
			AnimationStateMachine.Layer layer = this.Layers[i];
			layer.UpdateReferences(this.Parameters);
			layer.Initialize();
		}
		this.SetTargetAnimation(this.TargetAnimation);
		this.UpdateMonoBehaviours();
	}
	
	private void OnEnable()
	{
		if (this.TargetAnimation == null)
		{
			return;
		}
		this.Reset();
	}
	
	private void OnDisable()
	{
		if (this.TargetAnimation == null)
		{
			return;
		}
		for (int i = 0; i < this.Layers.Length; i++)
		{
			AnimationStateMachine.Layer layer = this.Layers[i];
			layer.Reset(this.TargetAnimation, this.MonoBehaviours);
		}
	}
	
	private void Update()
	{
		this.SmoothDeltaTime.Update(Time.deltaTime);
		float deltaTime = Mathf.Min(Time.deltaTime, this.SmoothDeltaTime.DeltaTime);
		this.CustomUpdate(deltaTime);
	}
	
	public void DrawDebug(AnimationStateMachine.DebugInfoType debugInfo)
	{
		if (this.TargetAnimation == null || debugInfo == AnimationStateMachine.DebugInfoType.None)
		{
			return;
		}
		GUILayout.BeginArea(new Rect(10f, (float)Screen.height / 4f, (float)Screen.width - 20f, (float)Screen.height / 2f), GUI.skin.box);
		GUI.color = Color.green;
		foreach (AnimationState animationState in this.TargetAnimation)
		{
			AnimationStateMachine.Layer layer = null;
			AnimationStateMachine.ClipInfo clipInfo = null;
			for (int i = 0; i < this.Layers.Length; i++)
			{
				layer = this.Layers[i];
				int num = layer.CurrentActiveAnimations.FindIndex((AnimationStateMachine.ActiveAnimation a) => a.AnimationState == animationState);
				if (num != -1)
				{
					clipInfo = layer.CurrentActiveAnimations[num].ClipInfo;
					break;
				}
			}
			string text = (clipInfo == null) ? ((!(animationState.clip != null)) ? "none" : animationState.clip.name) : clipInfo.AnimationClipName;
			string text2;
			if (debugInfo == AnimationStateMachine.DebugInfoType.Default)
			{
				text2 = string.Format("Clip:{0}", text);
			}
			else
			{
				text2 = string.Format("Layer:{0}/{1} Clip:{2} Time{3}/{4} BlendMode:{5} Weight:{6}", new object[] {
					(layer == null) ? string.Empty : layer.Name,
					animationState.layer,
					text,
					animationState.time,
					animationState.length,
					animationState.blendMode,
					animationState.weight
				});
			}
			GUILayout.Label(text2, new GUILayoutOption[0]);
		}
		GUILayout.EndArea();
	}
}
