using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public delegate void TimerCallBack();
public delegate void TimerCallBack1(System.Object arg1);

///<summary>
///function：simple timer, automaticlly excute functions. must updated by TimerManager
///author：chenxiang
///</summary> 
public class Timer
{
	private bool m_repeat = false;
	private float m_elapsed = 0;
	private float m_interval = 0;
	private bool m_enable = false;
    private bool m_pause = false;

    public bool Pause
    {
        get { return m_pause; }
        set { m_pause = value; }
    }
	
	private TimerCallBack m_callback = null;
	private TimerCallBack1 m_callback1 = null;
	private int m_callBackArgNum = 0;
    System.Object arg1 = null;
	
	public int CakkBackArgNum
	{
		get
		{
			return m_callBackArgNum;
		}
		set
		{
			m_callBackArgNum = value;
		}
	}
	
	public TimerCallBack CallBack
	{
		get
		{
			return m_callback;
		}
	}
	
	public TimerCallBack1 CallBack1
	{
		get
		{
			return m_callback1;
		}
	}
	
	public bool Repeat
	{
		set
		{
			m_repeat = value;
		}
	}	
	
	public bool Enable
	{
		get
		{
			return m_enable;
		}
		set
		{
			m_enable = value;
		}
	}
	
	public void SetDelay(float delay)
	{
		m_elapsed = 0;
		m_interval = delay;
		m_enable = true;
	}
	
	public void ResetDelay(float delay)
	{
		m_interval = delay;
		m_enable = true;
	}
	
	public Timer(TimerCallBack callback, float interval, bool repeat)
	{
		m_elapsed = 0;
		m_callback = callback;
		m_callback1 = null;
		m_interval = interval;
		m_repeat = repeat;
		m_enable = true;
		m_callBackArgNum = 0;
		arg1 = null;
	}

    public Timer(TimerCallBack1 callback, float interval, bool repeat, System.Object arg)
	{
		m_elapsed = 0;
		m_callback = null;
		m_callback1 = callback;
		m_interval = interval;
		m_repeat = repeat;
		m_enable = true;
		m_callBackArgNum = 1;
		arg1 = arg;
	}
	
	public void Update(float time)
	{
		if(0 == m_callBackArgNum)
		{
			if (m_callback != null && m_callback.Target == null)
			{
				m_enable = false;
				return;
			}
		}
		else if(1 == m_callBackArgNum)
		{
			if (m_callback1 != null && m_callback1.Target == null)
			{
				m_enable = false;
				return;
			}
		}
		
		m_elapsed += time;
		//TimerCallBack callback = new TimerCallBack(m_callback);
		//TimerCallBack1 callback1 = new TimerCallBack1(m_callback1);
		
		if (m_enable && m_elapsed > m_interval)
		{
			if (m_repeat)
			{
				m_elapsed = 0;
			}
			else
			{
				m_enable = false;
				//DHMain.m_timerManager.StopTimer(this);
			}
			
			if(0 == m_callBackArgNum)
			{
				if (m_callback != null) 
					m_callback();
			}
			else if(1 == m_callBackArgNum)
			{
				if (m_callback1 != null) 
					m_callback1(arg1);
			}
		}
	}
}

///<summary>
///function：timer manager. maintain a list of timer, and updates them. provide interfaces to start or stop a timer.
///author：chenxiang
///</summary> 
public static class TimerManager 
{
	//private static DHTimerManager m_timerManager = new DHTimerManager();
	private static List<Timer> timerList = new List<Timer>();
	private static List<Timer> removeList = new List<Timer>();
	private static List<Timer> addList = new List<Timer>();
	
	public static void StartTimer(TimerCallBack callback, float delay)
	{
		StartTimer(callback, delay, false);
	}
	
	public static void StartTimer(TimerCallBack callback, float delay, bool repeat)
	{
		StartTimer(callback, delay, repeat, false);
	}
	
	public static void StartTimer(TimerCallBack callback, float delay, bool repeat, bool reset)
	{
		#if UNITY_EDITOR	
		//Debug.Log("StartTimer callback = " + callback + " time = " + delay + " repeat = " + repeat);
		#endif
		if (callback == null)
		{
			return;
		}
		
		if(delay <= 0)
		{
			delay = 0.001f;
		}
		#if UNITY_EDITOR			
		//Debug.Log("starttimer 1 timerlist.count = " + timerList.Count + "  " + callback.Target + " " + callback.Method.Name);
		#endif		
		foreach (Timer timer in timerList)
		{
			if (callback == timer.CallBack && timer.Enable)
			{
				#if UNITY_EDITOR	
				//Debug.Log("StartTimer callback repeat");
				#endif
				timer.Repeat = repeat;
				if(reset)
				{
					timer.ResetDelay(delay);
				}
				else
				{
					timer.SetDelay(delay);
				}
				return;
			}
		}
		#if UNITY_EDITOR			
		//Debug.Log("starttimer 2 addlist.count = " + addList.Count);
		#endif		
		foreach (Timer timer in addList)
		{
			if (callback == timer.CallBack && timer.Enable)
			{
				timer.Repeat = repeat;
				if(reset)
				{
					timer.ResetDelay(delay);
				}
				else
				{
					timer.SetDelay(delay);
				}
				#if UNITY_EDITOR	
				//Debug.Log("starttimer 2 repeat "  + delay);
				#endif
				return;
			}
		}
		#if UNITY_EDITOR			
		//Debug.Log("starttimer 3");
		#endif		
		Timer newTimer = new Timer(callback, delay, repeat);
		
		addList.Add(newTimer);
	}
	
	public static void StartTimer(TimerCallBack1 callback, float delay, System.Object arg)
	{
		StartTimer(callback, delay, false, arg);
	}

    public static void StartTimer(TimerCallBack1 callback, float delay, bool repeat, System.Object arg)
	{
		#if UNITY_EDITOR	
		//Debug.Log("StartTimer callback = " + callback + " time = " + delay + " repeat = " + repeat);
		#endif
		if (callback == null)
		{
			return;
		}
		
		if(delay <= 0)
		{
			delay = 0.001f;
		}
		#if UNITY_EDITOR			
		//Debug.Log("starttimer 1 timerlist.count = " + timerList.Count + "  " + callback.Target + " " + callback.Method.Name);
		#endif		
		foreach (Timer timer in timerList)
		{
			if (callback == timer.CallBack1 && timer.Enable)
			{
				#if UNITY_EDITOR	
				//Debug.Log("StartTimer callback repeat");
				#endif
				timer.Repeat = repeat;
				timer.SetDelay(delay);
				return;
			}
		}
		#if UNITY_EDITOR			
		//Debug.Log("starttimer 2 addlist.count = " + addList.Count);
		#endif		
		foreach (Timer timer in addList)
		{
			if (callback == timer.CallBack1 && timer.Enable)
			{
				timer.Repeat = repeat;
				timer.SetDelay(delay);
				#if UNITY_EDITOR	
				//Debug.Log("starttimer 2 repeat "  + delay);
				#endif
				return;
			}
		}
		#if UNITY_EDITOR			
		//Debug.Log("starttimer 3");
		#endif		
		Timer newTimer = new Timer(callback, delay, repeat, arg);
		
		addList.Add(newTimer);
	}
	
	public static void ResetTimer(TimerCallBack callback, float delay)
	{
		ResetTimer(callback, delay, false);
	}
	
	public static void ResetTimer(TimerCallBack callback, float delay, bool repeat)
	{
		if (callback == null)
		{
			return;
		}
		
		if(delay <= 0)
		{
			delay = 0.001f;
		}
		
		foreach (Timer timer in timerList)
		{
			if (callback == timer.CallBack && timer.Enable)
			{
				#if UNITY_EDITOR	
				//Debug.Log("StartTimer callback repeat");
				#endif
				timer.Repeat = repeat;
				timer.ResetDelay(delay);
				break;
			}
		}
	}
	
	public static void Update(float time)
	{
		#if UNITY_EDITOR	
		//Debug.Log("dhtimermanager 1 count = " + timerList.Count);
		#endif
		foreach (Timer timer in removeList)
		{
			timerList.Remove(timer);
			#if UNITY_EDITOR	
			//Debug.Log("DHTimerManager Updates timer count = " + timerList.Count);
			#endif
		}
		
		removeList.Clear();
		
		foreach (Timer timer in addList)
		{
			if (timer.Enable)
			{
				timerList.Add(timer);
			}
		}
		
		addList.Clear();
		#if UNITY_EDITOR			
		//Debug.Log("dhtimermanager 2 count = " + timerList.Count);
		#endif		
		foreach (Timer timer in timerList)
		{
			if (timer != null && timer.Enable)
			{
				/*if (timer.CallBack != null && timer.CallBack.Target == null)
			{
				timer.Enable = false;
				continue;
			}*/
                if (!timer.Pause)
                {
                    timer.Update(time);
                }
			}
			else
			{
				StopTimer(timer);
			}
		}
		#if UNITY_EDITOR			
		//Debug.Log("dhtimermanager 3 count = " + timerList.Count);
		#endif
	}
	
	public static void StopTimer(Timer timer)
	{
		#if UNITY_EDITOR	
		//Debug.Log("DHTimerManager remove timer" + timer.CallBack.Target + " " + timer.CallBack.Method.Name);
		#endif
		if (timer != null)
		{
			#if UNITY_EDITOR	
			//Debug.Log("DHTimerManager remove timer count = " + timerList.Count);
			#endif
			timer.Enable = false;

			if(!removeList.Contains(timer))
			{
				removeList.Add(timer);
			}
		}
	}
	
	public static void StopTimer(TimerCallBack callback)
	{
		#if UNITY_EDITOR	
		//Debug.Log("DHTimerManager  array count = " + timerList.Count);
		#endif
		foreach (Timer timer in timerList)
		{
			if (timer == null)
			{
				#if UNITY_EDITOR	
				Debug.LogError("StopTimer error!!!");
				#endif
			}
			#if UNITY_EDITOR				
			//Debug.Log(" bool = " + timer.CallBack.Target == null);
			#endif	
			if (timer.CallBack1 != null) {
				continue;
			}
			if ((timer.CallBack == null || timer.CallBack.Target == null)
			    || (callback == timer.CallBack && timer.Enable))
			{	
				#if UNITY_EDITOR	
				//Debug.Log("stoptimer target1 " + callback.Target + " target2 " + timer.CallBack.Target);
				#endif
				StopTimer(timer);
				//continue;
			}
		}	
		
		foreach (Timer timer in addList)
		{
			if (callback == timer.CallBack && timer.Enable)
			{
				timer.Enable = false;
			}
		}
	}
	
	public static void StopTimer(TimerCallBack1 callback)
	{
		#if UNITY_EDITOR	
		//Debug.Log("DHTimerManager  array count = " + timerList.Count);
		#endif
		foreach (Timer timer in timerList)
		{
			if (timer == null)
			{
				#if UNITY_EDITOR	
				//Debug.LogError("StopTimer 2 error!!!");
				#endif
			}
			#if UNITY_EDITOR				
			//Debug.Log(" bool = " + timer.CallBack.Target == null);
			#endif		
			if (timer.CallBack != null)
				continue;
			if ((timer.CallBack1 == null || timer.CallBack1.Target == null)
			    || (callback == timer.CallBack1 && timer.Enable))
			{		
				#if UNITY_EDITOR	
				//Debug.Log("stoptimer target1 " + callback.Target + " target2 " + timer.CallBack.Target);
				#endif
				StopTimer(timer);
				//continue;
			}
		}	
		
		foreach (Timer timer in addList)
		{
			if (callback == timer.CallBack1 && timer.Enable)
			{
				timer.Enable = false;
			}
		}
	}
	
	public static void StopAllTimerByTarget(System.Object target)
	{
		foreach (Timer timer in timerList)
		{
			if (timer != null && timer.Enable)
			{	
				if((timer.CallBack != null && target == timer.CallBack.Target)
				   ||(timer.CallBack1 != null && target == timer.CallBack1.Target))
				{
					StopTimer(timer);
				}
			}
		}	
		
		foreach (Timer timer in addList)
		{
			if (timer != null && timer.Enable)
			{
				if((timer.CallBack != null && target == timer.CallBack.Target)
				   ||(timer.CallBack1 != null && target == timer.CallBack1.Target))
				{
					timer.Enable = false;
				}	
			}
		}
	}

    public static void StartPauseTimer(TimerCallBack callback,bool state)
    {
        foreach (Timer timer in timerList)
        {
            if ((callback == timer.CallBack && timer.Enable))
            {
                timer.Pause = state;
                test = callback;
            }
        }

        foreach (Timer timer in addList)
        {
            if (callback == timer.CallBack && timer.Enable)
            {
                timer.Pause = state;
            }
        }
    }

    public static TimerCallBack test;
}
