
using System;
using UnityEngine;
using Soulgame.Util;

[Serializable]
public class AnimationClipEvent : ScriptableObject
{
	[Serializable]
	public class Event
	{
		public enum ParameterType
		{
			None,
			Int,
			Bool,
			Float,
			String,
			ObjectReference,
			AnimationCurve
		}
		
		[Serializable]
		public class Parameter
		{
			public AnimationClipEvent.Event.ParameterType ParameterType;
			
			public int IntOrBoolParameter;
			
			public float FloatParameter;
			
			public string StringParameter;
			
			public UnityEngine.Object ObjectReferenceParameter;
			
			public AnimationCurve AnimationCurveParameter;
		}
		
		public float Time;
		
		public string Method;
		
		public AnimationClipEvent.Event.Parameter[] Parameters;
		
		public object[] ParameterObjects;
		
		public void Initialize()
		{
			if (this.Parameters == null)
			{
				return;
			}
			this.ParameterObjects = new object[this.Parameters.Length];
			for (int i = 0; i < this.Parameters.Length; i++)
			{
				AnimationClipEvent.Event.Parameter parameter = this.Parameters[i];
				switch (parameter.ParameterType)
				{
				case AnimationClipEvent.Event.ParameterType.Int:
					this.ParameterObjects[i] = parameter.IntOrBoolParameter;
					break;
				case AnimationClipEvent.Event.ParameterType.Bool:
					this.ParameterObjects[i] = (parameter.IntOrBoolParameter > 0);
					break;
				case AnimationClipEvent.Event.ParameterType.Float:
					this.ParameterObjects[i] = parameter.FloatParameter;
					break;
				case AnimationClipEvent.Event.ParameterType.String:
					this.ParameterObjects[i] = parameter.StringParameter;
					break;
				case AnimationClipEvent.Event.ParameterType.ObjectReference:
					this.ParameterObjects[i] = parameter.ObjectReferenceParameter;
					break;
				case AnimationClipEvent.Event.ParameterType.AnimationCurve:
					this.ParameterObjects[i] = parameter.AnimationCurveParameter;
					break;
				}
			}
		}
		
		public void Invoke(MonoBehaviour[] monoBehaviours)
		{
			for (int i = 0; i < monoBehaviours.Length; i++)
			{
				InvokeUtils.Invoke(monoBehaviours[i], this.Method, this.ParameterObjects);
			}
		}
	}
	
	[SerializeField]
	public AnimationClipEvent.Event[] Events;
	
	[SerializeField]
	public AnimationClipEvent.Event[] EnterEvents;
	
	[SerializeField]
	public AnimationClipEvent.Event[] ExitEvents;
	
	private void OnEnable()
	{
		if (this.EnterEvents != null)
		{
			for (int i = 0; i < this.EnterEvents.Length; i++)
			{
				AnimationClipEvent.Event @event = this.EnterEvents[i];
				@event.Initialize();
			}
		}
		if (this.ExitEvents != null)
		{
			for (int j = 0; j < this.ExitEvents.Length; j++)
			{
				AnimationClipEvent.Event event2 = this.ExitEvents[j];
				event2.Initialize();
			}
		}
		if (this.Events != null)
		{
			for (int k = 0; k < this.Events.Length; k++)
			{
				AnimationClipEvent.Event event3 = this.Events[k];
				event3.Initialize();
			}
		}
	}
	
	public void OnEnter(MonoBehaviour[] monoBehaviours)
	{
		if (this.EnterEvents != null)
		{
			for (int i = 0; i < this.EnterEvents.Length; i++)
			{
				AnimationClipEvent.Event @event = this.EnterEvents[i];
				@event.Invoke(monoBehaviours);
			}
		}
	}
	
	public void OnExit(MonoBehaviour[] monoBehaviours)
	{
		if (this.ExitEvents != null)
		{
			for (int i = 0; i < this.ExitEvents.Length; i++)
			{
				AnimationClipEvent.Event @event = this.ExitEvents[i];
				@event.Invoke(monoBehaviours);
			}
		}
	}
	
	public void UpdateEvents(float startTime, float endTime, float minTime, MonoBehaviour[] monoBehaviours)
	{
		if (Mathf.Abs(endTime - startTime) <= Mathf.Epsilon)
		{
			return;
		}
		for (int i = 0; i < this.Events.Length; i++)
		{
			AnimationClipEvent.Event @event = this.Events[i];
			float num = Mathf.Max(@event.Time, minTime);
			if (num >= startTime && num < endTime)
			{
				@event.Invoke(monoBehaviours);
			}
		}
	}
}


