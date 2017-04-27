
using System;
using System.Collections.Generic;
using FootStudio.Util;


namespace FootStudio.Framework
{
	public class EventListenerList
	{
		private readonly Dictionary<int, HashSet<OnEvent>> listenerMap;
		
		public EventListenerList()
		{
			this.listenerMap = new Dictionary<int, HashSet<OnEvent>>();
		}
		
		public void Add(int eventId, OnEvent listener)
		{
			Assert.NotNull(listener, "listener");
			HashSet<OnEvent> hashSet = this.GetAllListeners(eventId);
			if (hashSet == null)
			{
				hashSet = new HashSet<OnEvent>();
				this.listenerMap.Add(eventId, hashSet);
			}
			bool flag = hashSet.Add(listener);
			if (flag)
			{
			}
		}
		
		public void Remove(int eventId, OnEvent listener)
		{
			Assert.NotNull(listener, "listener");
			HashSet<OnEvent> allListeners = this.GetAllListeners(eventId);
			if (allListeners == null)
			{
				return;
			}
			bool flag = allListeners.Remove(listener);
			if (flag)
			{
			}
		}
		
		public HashSet<OnEvent> GetAllListeners(int eventId)
		{
			HashSet<OnEvent> result;
			this.listenerMap.TryGetValue(eventId, out result);
			return result;
		}
	}
}

