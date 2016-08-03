
using System;
using Soulgame.Event;


namespace Soulgame.Util
{
	public class AppSession
	{
		protected const string Tag = "AppSession";
		
		private const string FirstStartTimePref = "GameState.FirstStartDateTime";
		
		private const string StartTimePref = "AppSession.StartTime";
		
		private const string PauseTimePref = "AppSession.PauseTime";
		
		private const string SessionIdPref = "AppSession.SessionId";
		
		protected static readonly TimeSpan SessionResetPauseDuration = TimeSpan.FromMinutes(3.0);
		
		protected DateTime StartTime;
		
		protected DateTime PauseTime;
		
		public virtual DateTime FirstStartTime
		{
			get;
			private set;
		}
		
		public virtual bool FirstStart
		{
			get;
			private set;
		}
		
		public virtual int SessionId
		{
			get;
			private set;
		}
		
		public virtual TimeSpan PreviousSessionDuration
		{
			get;
			private set;
		}
		
		public virtual TimeSpan PauseDuration
		{
			get;
			private set;
		}
		
		public EventBus EventBus
		{
			get;
			set;
		}
		
		public static void ClearPrefs()
		{
			UserPrefs.Remove("GameState.FirstStartDateTime");
			UserPrefs.Remove("AppSession.StartTime");
			UserPrefs.Remove("AppSession.PauseTime");
			UserPrefs.Remove("AppSession.SessionId");
		}
		
		public virtual void Init()
		{
			DateTime utcNow = DateTime.UtcNow;
			this.FirstStartTime = UserPrefs.GetDateTime("GameState.FirstStartDateTime", DateTime.MinValue);
			if (this.FirstStartTime == DateTime.MinValue)
			{
				this.FirstStartTime = utcNow;
				UserPrefs.SetDateTime("GameState.FirstStartDateTime", this.FirstStartTime);
				UserPrefs.Save();
				this.FirstStart = true;
			}
			this.StartTime = UserPrefs.GetDateTime("AppSession.StartTime", utcNow);
			this.PauseTime = UserPrefs.GetDateTime("AppSession.PauseTime", utcNow);
			this.SessionId = UserPrefs.GetInt("AppSession.SessionId", 0);
			this.AfterInit();
		}
		
		protected virtual void AfterInit()
		{
			this.OnAppResume();
		}
		
		public virtual void OnAppResume()
		{
			DateTime utcNow = DateTime.UtcNow;
			if (this.PauseTime > utcNow)
			{
				return;
			}
			if (utcNow > this.PauseTime + AppSession.SessionResetPauseDuration)
			{
				this.CreateNewSession(this.PauseTime);
				this.FireNewSessionEvent();
			}
		}
		
		public virtual void OnAppPause()
		{
			UserPrefs.SetDateTime("AppSession.StartTime", this.StartTime);
			this.PauseTime = DateTime.UtcNow;
			UserPrefs.SetDateTime("AppSession.PauseTime", this.PauseTime);
		}
		
		public virtual void ForceNewSession(bool fireEvent)
		{
			this.CreateNewSession(DateTime.UtcNow);
			if (fireEvent)
			{
				this.FireNewSessionEvent();
			}
		}
		
		protected virtual void CreateNewSession(DateTime toTime)
		{
			DateTime utcNow = DateTime.UtcNow;
			this.PreviousSessionDuration = toTime - this.StartTime;
			this.PauseDuration = utcNow - this.PauseTime;
			if (this.PreviousSessionDuration.Milliseconds < 0)
			{
			}
			this.StartTime = utcNow;
			this.PauseTime = this.StartTime;
			this.SessionId++;
			this.FirstStart = false;
			UserPrefs.SetDateTime("AppSession.StartTime", this.StartTime);
			UserPrefs.SetDateTime("AppSession.PauseTime", this.PauseTime);
			UserPrefs.SetInt("AppSession.SessionId", this.SessionId);
			UserPrefs.SaveDelayed();
		}
		
		protected virtual void FireNewSessionEvent()
		{
			this.EventBus.FireEvent(EventId.NEW_SESSION, this.PreviousSessionDuration);
		}
	}
}

