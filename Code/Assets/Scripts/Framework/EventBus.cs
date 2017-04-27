using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace FootStudio.Event
{
	public delegate void OnEvent(object eventData);

	public class EventBus {

		private const string Tag = "EventBus";
		
		private readonly EventListenerList eventListenerList;
		
		public EventBus()
		{
			this.eventListenerList = new EventListenerList();
		}
		
		public void FireEvent(int eventId)
		{
			this.FireEvent(eventId, null);
		}
		
		public void FireEvent(int eventId, object eventData)
		{
			HashSet<OnEvent> allListeners = this.eventListenerList.GetAllListeners(eventId);
			if (allListeners == null || allListeners.Count == 0)
			{
				return;
			}
			List<OnEvent> list = new List<OnEvent>(allListeners);
			foreach (OnEvent current in list)
			{
				current(eventData);
			}
		}
		
		public void AddListener(int eventId, OnEvent listener)
		{
			this.eventListenerList.Add(eventId, listener);
		}
		
		public void RemoveListener(int eventId, OnEvent listener)
		{
			this.eventListenerList.Remove(eventId, listener);
		}
	}

}
