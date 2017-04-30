using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class WindowBase : MonoBehaviour 
{
	public enum WindowSortType
	{
		Normal = 0,
		AlwaysTop,
		AlwaysBottom,
	}

	[HideInInspector][System.NonSerialized] public GameObject MainObject = null;//Main GameObject
    [HideInInspector][System.NonSerialized] public Transform CatchedTransform = null;
    [HideInInspector][System.NonSerialized] public int WinType = 0;//window ID
    [HideInInspector][System.NonSerialized] public bool Actived = false;//if window is actived
    [HideInInspector][System.NonSerialized] public bool Started = false;
    [HideInInspector][System.NonSerialized] public bool Closed = true;
    [HideInInspector][System.NonSerialized] public bool Canceled = false;
    [HideInInspector][System.NonSerialized] public bool CancelAndClose = false;
	public WindowSortType SortType = WindowSortType.Normal;
	public bool IsSingleton = true;
	public int Priority = 65536;
	public bool IsSubWindow = false;
	public bool IsDestroyWhenClose = false;
	public bool IsUnloadResource = false;//if unload assetbundle when destroy window
	public bool IsFullWindow = true;
    public bool ContainModel = false;
    public bool IsOpenSound = true;   //用openWindow方法是否播放打开界面音效

    private WindowBase m_parentWindow = null;
	public List<GameObject> NotifyObject = new List<GameObject>();
	private List<WindowBase> m_subWindows = new List<WindowBase>();
    private bool m_isClosing = false;
	private System.Object[] m_initArgs = null;
	private bool m_isPreLoad = false;
	private Canvas m_canvas = null;
    private RectTransform m_rectTrans = null;
    private WindowSortType CatchedSortType = WindowSortType.Normal;
    private int CatchedPriority = 65536;
	public Canvas MainCanvas
	{
		get
		{
			return m_canvas;
		}
	}

	public bool IsPreLoad
	{
		get
		{
			return m_isPreLoad;
		}
		set
		{
			if(m_isPreLoad == value) return;

			m_isPreLoad = value;

			for(int i = 0, max = m_subWindows.Count; i < max; ++i)
			{
				if(m_subWindows[i] != null)
				{
					m_subWindows[i].IsPreLoad = value;
				}
			}
		}
	}

    public WindowBase ParentWindow
    {
        get
        {
            return m_parentWindow;
        }
    }

    public void AddSubWindow(WindowBase win)
    {
        if (win != null)
        {
            if(!m_subWindows.Contains(win))
                m_subWindows.Add(win);
            win.m_parentWindow = this;
        }
    }

    public void RemoveSubWindow(WindowBase win)
    {
        if (win != null)
        {
            if (m_subWindows.Contains(win))
                m_subWindows.Remove(win);
            win.m_parentWindow = null;
        }
    }

    public int GetSunWindowCount()
    {
        return m_subWindows.Count;
    }

    public WindowBase OpenSubWindow(int type, params System.Object[] args)
	{
		WindowBase win = WindowManager.OpenSubWindow(type, this, args);
		return win;
	}

	public virtual void OnOpen(System.Object[] args)
	{
        StopCoroutine("DelayHide"); // MainObject.SetActive(false);
        Closed = false;
		for(int i = 0, max = NotifyObject.Count; i < max; ++i)
		{
			if(NotifyObject[i] != null)
			{
				NotifyObject[i].SendMessage("OnOpenWindow", SendMessageOptions.DontRequireReceiver);
			}
		}
	}

	public virtual void OnClose()
	{
		//if(Closed) return;
		Canceled = false;
		CancelAndClose = false;
		for(int i = 0, max = NotifyObject.Count; i < max; ++i)
		{
			if(NotifyObject[i] != null)
			{
				NotifyObject[i].SendMessage("OnCloseWindow", SendMessageOptions.DontRequireReceiver);
			}
		}
        /*
		for(int i = 0, max = m_subWindows.Count; i < max; ++i)
		{
			if(m_subWindows[i] != null)
			{
				m_subWindows[i].OnClose();
			}
		}
        */
        if (ParentWindow != null) ParentWindow.OnSubWindowClose(WinType);
        Closed = true;
	}

	public virtual void OnActive()
	{
		//if(Actived) return;

		Actived = true;
		for(int i = 0, max = NotifyObject.Count; i < max; ++i)
		{
			if(NotifyObject[i] != null)
			{
				NotifyObject[i].SendMessage("OnActiveWindow", SendMessageOptions.DontRequireReceiver);
			}
		}
 
		for(int i = 0, max = m_subWindows.Count; i < max; ++i)
		{
			WindowBase win = m_subWindows[i];
			if(win != null && win.Started && !win.Actived)
			{
                win.OnActive();
			}
		}
	}

	public virtual void OnDeactive()
	{
		//if(!Actived) return;

		Actived = false;
		Canceled = false;
		CancelAndClose = false;
		for(int i = 0, max = NotifyObject.Count; i < max; ++i)
		{
			if(NotifyObject[i] != null)
			{
				NotifyObject[i].SendMessage("OnDeactiveWindow", SendMessageOptions.DontRequireReceiver);
			}
		}

		for(int i = 0, max = m_subWindows.Count; i < max; ++i)
		{
            WindowBase win = m_subWindows[i];
            if (win != null && win.Actived && win.Started)
			{
				m_subWindows[i].OnDeactive();
			}
		}
	}

    public virtual void OnSubWindowClose(int winID)
    { 
    }

	public void Close()
	{
		if(WindowManager.Instance != null)
		{
			WindowManager.CloseWindow(this);
		}
	}

    public void PostClose()
    {
        m_isClosing = true;
        if (ParentWindow != null && !ParentWindow.m_isClosing) ParentWindow.RemoveSubWindow(this);
        m_parentWindow = null;

        for (int i = 0, max = m_subWindows.Count; i < max; ++i)
        {
            if (m_subWindows[i] != null)
            {
                m_subWindows[i].Close();
            }
        }

        m_subWindows.Clear();

        if (MainObject != null)
        {
            if (IsDestroyWhenClose)
            {
                Destroy(MainObject);
                if (IsUnloadResource)
                {

                }
            }
            else
            {
                MainObject.SetActive(false);
            }
        }

        Priority = CatchedPriority;
        SortType = CatchedSortType;

        
        m_isClosing = false;
    }

	public void Init(System.Object[] args)
    {
		if(!Started)
		{
			m_initArgs = args;
		}
	}

	protected virtual void Awake()
	{
        //Actived = false;
		MainObject = gameObject;
		CatchedTransform = transform;
		m_canvas = GetComponent<Canvas>();
        m_rectTrans = GetComponent<RectTransform>();
        //m_rectTrans.anc
		//m_canvas.IsInherited = true;
        CatchedPriority = Priority;
        CatchedSortType = SortType;
	}

	void Start()
    {
		if(IsPreLoad) 
		{
			Started = true;
			//MainObject.SetActive(false);
            StartCoroutine("DelayHide");
			return;
		}
        
		if(Closed) OnOpen(m_initArgs);
		if(!Actived) OnActive();
		Started = true;

		if(CancelAndClose)
		{
            if (Actived) OnDeactive();
            if (!Closed) OnClose();
		}
		else if(Canceled)
		{
            if (Actived) OnDeactive();
		}

		m_initArgs = null;
	}

    IEnumerator DelayHide()
    {
        yield return null;

        MainObject.SetActive(false);
       
    }

    protected virtual void OnDestroy()
    {
        if (!Actived) OnDeactive();
        if (!Closed) OnClose();
    }

	public void RegisterNotify(GameObject go)
	{
		if(!NotifyObject.Contains(go) && go != null)
		{
			NotifyObject.Add(go);
            Debug.Log("================================RegisterNotify");
		}
	}

	public void UnregisterNotify(GameObject go)
	{
		if(NotifyObject.Contains(go))
		{
			NotifyObject.Remove(go);
		}
	}

    public virtual int[] PreLoadSubWindowID() { return null; }
}
