using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using FootStudio.Util;

/// <summary>
/// 加载资源路径基本结构
/// </summary>
public struct PrefabInfo
{
    private string m_path;
    private string m_name;
    private bool m_isInternal;

    /// <summary>
    /// 加载路径
    /// </summary>
    public string Path
    {
        get
        {
            return m_path;
        }
        set
        {
            m_path = value;
        }
    }

    /// <summary>
    /// 加载资源名字
    /// </summary>
    public string Name
    {
        get
        {
            return m_name;
        }
    }

    /// <summary>
    /// 该资源是不是Bundle内部资源
    /// </summary>
    public bool IsInternal
    {
        get
        {
            return m_isInternal;
        }
    }

    public PrefabInfo(string path, string name, bool isInternal)
    {
        m_path = path;

        m_name = name;

        m_isInternal = isInternal;
    }
}

/// <summary>
/// Window manager.
/// </summary>
public class WindowManager : MonoBehaviour 
{
	//singleton instance
	private static WindowManager m_self = null;

	//prefab info for each window
	private static Dictionary<int, PrefabInfo> WindowPrefabsInfo = new Dictionary<int, PrefabInfo>();

	//z-depth for each window
	const int PERWINDOW_DEPTH = 200;
	const int MAX_WINDOW_NUM  = 16;

	//window list
	private List<WindowBase> m_windowGoList = new List<WindowBase>();
    private List<WindowBase> m_windowGoBack = new List<WindowBase>();
	private List<WindowBase> m_subWindowGoList = new List<WindowBase>();
	//private List<int> m_windowTypeList = new List<int>();
	private WindowBase m_currentWindow = null;
	private int m_topWinNum = 0;
	private int m_bottomWimNum = 0;

	public bool IsRetain = false;
	[HideInInspector]public bool IsWaitting = false;

	public Transform CommonCanvas = null;

	public int FirstWindow = -1;
	public int[] PreLoadWindows;

	//interface to get singleton instance
	public static WindowManager Instance
	{
		get
		{
			return m_self;
		}
	}

	public WindowBase CurrentWindow
	{
		get
		{
			return m_currentWindow;
		}
	}

	public int CurrentWindowType
	{
		get
		{
			return m_currentWindow.WinType;
		}
	}

	public static void SetWindowPrefabInfo(int winType, string path, string name, bool isInternal)
	{
		if(!WindowPrefabsInfo.ContainsKey(winType))
		{
			PrefabInfo info = new PrefabInfo(path, name, isInternal);
			WindowPrefabsInfo.Add(winType, info);
		}
	}

	public static bool GetWindowPrefabInfo(int winType, out PrefabInfo info)
	{
		if(WindowPrefabsInfo.ContainsKey(winType))
		{
			info = WindowPrefabsInfo[winType];
            return true;
		}

        info = new PrefabInfo();
		return false;
	}

	private void PreOpenWindow(int type)
	{
		if(!WindowPrefabsInfo.ContainsKey(type) || m_self == null)
		{
			#if UNITY_EDITOR
			Debug.LogError("PreOpenWindow,No such type window. TypeID = " + type);
			#endif
			return;
		}

		bool found = false;

		for(int i = 0; i < m_windowGoBack.Count; ++i)
		{
			if(m_windowGoBack[i].WinType == type)
			{
				found = true;
				break;
			}
		}

		GameObject go = null;
		if(!found)
		{
			//find the resource(prefab) path
			PrefabInfo info = WindowPrefabsInfo[type];
			
			//intialize gameobject

            Assert.IsTrue(!info.IsInternal, "Not Impl With Bundle");

            if (!info.IsInternal)
            {
                AssetManager.LoadAssetFromResources(info.Path, typeof(GameObject), (AssetManager.Asset asset) =>
                {
                    if (asset.State == AssetManager.State.Loaded)
                    {
                        go = GameObject.Instantiate(asset.AssetObject) as GameObject;
                    }
                });
            }
            
		}

		if(go != null) 
		{
			go.transform.parent = CommonCanvas;
			WindowBase win = go.GetComponent<WindowBase>();
			win.WinType = type;
			win.IsPreLoad = true;
            go.SetActive(false);
			m_windowGoBack.Add(win); 
            int[] subWinIDs = win.PreLoadSubWindowID();
            if (subWinIDs != null)
            {
                for (int i = 0; i < subWinIDs.Length; ++i)
                {
                    PreOpenWindow(subWinIDs[i]);
                }
            }
		}
	}

    public static WindowBase OpenSubWindow(int type, WindowBase parentWin, params System.Object[] args)
	{
		if(!WindowPrefabsInfo.ContainsKey(type) || m_self == null)
		{
			#if UNITY_EDITOR
			Debug.LogError("OpenSubWindow,No such type window. TypeID = " + type);
			#endif
			return null;
		}

		if(parentWin == null)
		{
			#if UNITY_EDITOR
			Debug.LogError("OpenSubWindow, must have a parent window");
			#endif
			return null;
		}

		//check if exist
		GameObject go = null;
		WindowBase win = null;
		int exist = 0;
		for(int i = 0; i < m_self.m_subWindowGoList.Count; ++i)
		{
			WindowBase window = m_self.m_subWindowGoList[i];
			if(window.WinType == type && window.IsSingleton)
			{
				win = window;
				go = win.MainObject;
				exist = 1;
                if (win.ParentWindow == parentWin)
                {
                    go.SetActive(true);
                    return win;
                }
				break;
			}
		}
		if(0 == exist)
		{
			for(int i = 0; i < m_self.m_windowGoList.Count; ++i)
			{
				WindowBase window = m_self.m_windowGoList[i];
				if(window.WinType == type && window.IsSingleton)
				{
					win = window;
					go = win.MainObject;
					exist = 2;
					break;
				}
			}
		}
		
		if(0 == exist)
		{
			for(int i = 0; i < m_self.m_windowGoBack.Count; ++i)
			{
				if(m_self.m_windowGoBack[i].WinType == type)
				{
					win = m_self.m_windowGoBack[i];
					go = win.MainObject;
					exist = 3;
					break;
				}
			}
		}
		
		if(0 == exist)
		{
			//find the resource(prefab) path
			PrefabInfo info = WindowPrefabsInfo[type];
			
			//intialize gameobject
            Assert.IsTrue(!info.IsInternal, "Not Impl With Bundle");

            if (!info.IsInternal)
            {
                AssetManager.LoadAssetFromResources(info.Path, typeof(GameObject), (AssetManager.Asset asset) =>
                {
                    if (asset.State == AssetManager.State.Loaded)
                    {
                        go = GameObject.Instantiate(asset.AssetObject) as GameObject;
                    }
                });
            }
		}

		go.SetActive(true);
        int depth = parentWin.GetSunWindowCount()+1;
		go.transform.parent = parentWin.CatchedTransform;
        go.transform.localScale = Vector3.one;
        //go.transform.localPosition = new Vector3(0,0,-depth);
		
		if(1 == exist)
		{
			//m_self.m_subWindowGoList.Remove(win);
			//m_self.m_subWindowGoList.Add(win);
		}
		else if(2 == exist)
		{
			m_self.m_windowGoList.Remove(win);
			m_self.m_subWindowGoList.Add(win);
		}
		else if(3 == exist)
		{
			m_self.m_windowGoBack.Remove(win);
			m_self.m_subWindowGoList.Add(win);
		}
		else
		{
			win = go.GetComponent<WindowBase>();
			if(win != null)
			{
				win.WinType = type;
				m_self.m_subWindowGoList.Add(win);
			}
			else
			{
				#if UNITY_EDITOR
				Debug.LogError("OpenSubWindow: Can't find WindowBase at window root. WinObj = " + go.name);
				#endif
				Destroy(go);
				return null;
			}
		}

		if(win != null)
		{
            if (win.ParentWindow != null) win.ParentWindow.RemoveSubWindow(win);
			win.IsPreLoad = parentWin.IsPreLoad;
			if(!win.Started) win.Init(args);
			win.IsSubWindow = true;

            win.MainCanvas.sortingLayerID = LayerMask.GetMask("UI");
            win.MainCanvas.sortingOrder = depth;
            parentWin.AddSubWindow(win);
			if(win.Started)
			{
				if(3 == exist && win.Closed)
				{
					win.OnOpen(args);
				}

                if(!win.Actived)
				    win.OnActive();
			}
		}

		return win;
	}

    public static WindowBase OpenWindow(int type, params System.Object[] args)
    {
        WindowBase winBase = OpenWindowWithPriorty(type, -1, args);

        if (winBase.IsOpenSound)
        {
            //AudioManager.PlaySound(AudioEnum.OPEN_WIN);
        }
        
        return winBase;
    }

    public static WindowBase OpenWindowWithPriorty(int type, int priority, params System.Object[] args)
	{
		if(!WindowPrefabsInfo.ContainsKey(type) || m_self == null)
		{
			#if UNITY_EDITOR
			Debug.LogError("No such type window. TypeID = " + type);
			#endif
			return null;
		}

        //Debug.LogWarning("Open window : " + type);
		//check if exist
		GameObject go = null;
		WindowBase win = null;
		int exist = 0;

		for(int i = 0; i < m_self.m_windowGoList.Count; ++i)
        {
			WindowBase window = m_self.m_windowGoList[i];
			if(window.WinType == type && window.IsSingleton)
            {
				win = window;
				go = win.MainObject;
				exist = 1;
                if (m_self.CurrentWindow == win || win.SortType == WindowBase.WindowSortType.AlwaysTop)
                {
                    go.SetActive(true);
                    return win;
                }
				break;
			}
		}

		if(0 == exist)
		{
			for(int i = 0; i < m_self.m_subWindowGoList.Count; ++i)
            {
				WindowBase window = m_self.m_subWindowGoList[i];
				if(window.WinType == type && window.IsSingleton)
				{
					win = window;
					go = win.MainObject;
					exist = 2;
					break;
				}
			}
		}

		if(0 == exist)
		{
			for(int i = 0; i < m_self.m_windowGoBack.Count; ++i)
            {
				if(m_self.m_windowGoBack[i].WinType == type)
                {
					win = m_self.m_windowGoBack[i];
					go = win.MainObject;
					exist = 3;
					break;
				}
			}
		}

		if(0 == exist)
		{
			//find the resource(prefab) path
			PrefabInfo info = WindowPrefabsInfo[type];
			//intialize gameobject
            Assert.IsTrue(!info.IsInternal, "Not Impl With Bundle");

            if (!info.IsInternal)
            {
                AssetManager.LoadAssetFromResources(info.Path, typeof(GameObject), (AssetManager.Asset asset) =>
                {
                    if (asset.State == AssetManager.State.Loaded)
                    {
                        go = GameObject.Instantiate(asset.AssetObject) as GameObject;
                    }
                });
            }
		}
		
		//add to UI-Root and set to top
		if(go == null)
		{
			#if UNITY_EDITOR
			Debug.LogError("OpenWindow: Found a NULL window. TypeID = " + type);
			#endif
			return null;
		}
      
		go.SetActive(true);

		if(1 == exist)
		{
			m_self.m_windowGoList.Remove(win);
			m_self.m_windowGoList.Add(win);
		}
		else if(2 == exist)
		{
			m_self.m_subWindowGoList.Remove(win);
			m_self.m_windowGoList.Add(win);
		}
		else if(3 == exist)
		{
			m_self.m_windowGoBack.Remove(win);
			m_self.m_windowGoList.Add(win);
		}
		else
		{
			go.transform.parent = m_self.CommonCanvas;

			win = go.GetComponent<WindowBase>();
			if(win != null)
			{
				win.WinType = type;
				m_self.m_windowGoList.Add(win);
			}
			else
			{
				#if UNITY_EDITOR
				Debug.LogError("OpenWindow: Can't find WindowBase at window root. WinObj = " + go.name);
				#endif
				Destroy(go);
				return null;
			}
		}

		win.IsPreLoad = false;
		if(!win.Started) win.Init(args);

		go.transform.localScale = Vector3.one;
		go.transform.localEulerAngles = Vector3.zero;
        go.transform.localPosition = new Vector3(0, 0, -1000);
        if (priority > 0)
        {
            win.SortType = WindowBase.WindowSortType.AlwaysTop;
            win.Priority = priority;
        }
		if(win.SortType == WindowBase.WindowSortType.AlwaysTop)
		{
            //Debug.LogWarning("window manager test " + win.Priority + priority);
			//go.transform.localPosition = new Vector3(0, 0, -600);
			if(exist != 1) ++m_self.m_topWinNum;
			for(int i = m_self.m_windowGoList.Count - m_self.m_topWinNum, max = m_self.m_windowGoList.Count; i < max; ++i)
			{
				if(m_self.m_windowGoList[i].Priority < win.Priority)
				{
					m_self.m_windowGoList.Remove(win);
					m_self.m_windowGoList.Insert(i, win);
					break;
				}
			}
		}
		else
		{
			if(m_self.m_topWinNum > 0)
            {
				m_self.m_windowGoList.Remove(win);
				m_self.m_windowGoList.Insert(m_self.m_windowGoList.Count - m_self.m_topWinNum, win);
			}
		}

        int containModelWinCount = 0;
        int lastContainModelWinIndex = -1;
        int currentIndex = -1;
		for(int i = 0, max = m_self.m_windowGoList.Count; i < max; ++i)
		{
			WindowBase window = m_self.m_windowGoList[i];
			if(window != null)
			{
                //Debug.Log("sort window : " + window.name + i);
                Vector3 pos = window.CatchedTransform.localPosition;
				if(window.SortType == WindowBase.WindowSortType.AlwaysTop)
				{
                    window.CatchedTransform.localPosition = new Vector3(pos.x, pos.y, (max - i) * 10 - 1000);
				}
				else if(window != win)
				{
                    window.CatchedTransform.localPosition = new Vector3(pos.x, pos.y, (max - i - m_self.m_topWinNum) * 500 - 200);
				}

                if (currentIndex > -1 && window.IsFullWindow && i > currentIndex)
                {
                    win.MainObject.SetActive(false);
                }

                if (window == win) currentIndex = i;

				if(window.MainCanvas != null)
				{
                    window.MainCanvas.sortingLayerID = LayerMask.GetMask("UI");
                    window.MainCanvas.sortingOrder = (i - containModelWinCount) * 20;
				}

                if (window.ContainModel && window != win && !win.IsFullWindow)
                {
                    lastContainModelWinIndex = i;
                    if (win.ContainModel)
                    {
                        window.MainObject.SetActive(false);
                    }
                }
			}
		}

		/*
		if(win.MainPanel != null)
		{
			if(win.SortType == WindowBase.WindowSortType.AlwaysTop)
			{
				win.MainPanel.depth = 200;
			}
			else
			{
				win.MainPanel.depth = 160;
			}
		}
		*/
		if(m_self.m_currentWindow != null)
		{
            if (m_self.m_currentWindow.Started && m_self.m_currentWindow.Actived)
            {
				m_self.m_currentWindow.OnDeactive();
			}
			else
			{
				m_self.m_currentWindow.Canceled = true;
			}

			//open a new window, how about current window? do we need to hide it?
			if(win.IsFullWindow)
			{
				int current = m_self.m_windowGoList.IndexOf(m_self.m_currentWindow);
                int thiswin = m_self.m_windowGoList.IndexOf(win)-1;
                if (thiswin < current) current = thiswin;

				for(int i = current; i >= 0; --i)
				{
					WindowBase winTmp = m_self.m_windowGoList[i];
					if(winTmp.MainObject.activeSelf)
					{
                        //Debug.Log("deactive current window : " + winTmp.name + i);
						winTmp.MainObject.SetActive(false);
					}
					else
					{
						break;
					}
				}
			}
		}
		m_self.m_currentWindow = win;

		if(win != null)
		{
			win.IsSubWindow = false;
            if (win.ParentWindow != null) win.ParentWindow.RemoveSubWindow(win);
            //win.ParentWindow = null;
			if(win.Started)
			{
				if(3 == exist && win.Closed)
				{
					win.OnOpen(args);
				}

                if (!win.Actived)
				    win.OnActive();
			}
            //if(win.IsFullWindow)
                //UICamera.ReleaseCurrentTouch();
		}

		return win;
	}

    public static void CloseAllWindow()
    {
        if (m_self == null)
        {
            #if UNITY_EDITOR
            Debug.LogError("CloseAllWindow: No Window Showing!");
            #endif
            return;
        }

        for (int i = m_self.m_windowGoList.Count - 1; i >= 0; --i)
        {
            WindowBase winTmp = m_self.m_windowGoList[i];
            if(winTmp != null)
            {
                if(winTmp.Actived) winTmp.OnDeactive();
                if(!winTmp.Closed) winTmp.OnClose();
                if (!winTmp.IsDestroyWhenClose) m_self.m_windowGoBack.Add(winTmp);
                winTmp.PostClose();
            }
        }

        for (int i = m_self.m_subWindowGoList.Count - 1; i >= 0; --i)
        {
            WindowBase winTmp = m_self.m_subWindowGoList[i];
            if (winTmp != null)
            {
                if (winTmp.Actived) winTmp.OnDeactive();
                if (!winTmp.Closed) winTmp.OnClose();
                if (!winTmp.IsDestroyWhenClose) m_self.m_windowGoBack.Add(winTmp);
                winTmp.PostClose();
            }
        }

        m_self.m_windowGoList.Clear();
        m_self.m_subWindowGoList.Clear();
        m_self.m_topWinNum = 0;
        m_self.m_currentWindow = null;
    }

    public static void CloseAllWindowReturnToFirst()
    {
        if (m_self == null)
        {
            #if UNITY_EDITOR
            Debug.LogError("CloseAllWindow: No Window Showing!");
            #endif
            return;
        }

        CloseAllWindow();

        if (m_self.FirstWindow > 0)
        {
            OpenWindow(m_self.FirstWindow);
        }
    }

	public static void CloseCurrentWindow()
	{
		if(m_self != null)
		{
            CloseWindow(m_self.m_currentWindow);
		}
	}

	public static void CloseWindow(WindowBase window)
	{
		if(m_self == null || m_self.m_currentWindow == null || window == null)
		{
			#if UNITY_EDITOR
			Debug.LogError("BackWindow: No Window Showing!"); 
			#endif
			return;
		}

        //Debug.LogWarning("close window  " + window.name);

        if (window.IsSubWindow || m_self.CurrentWindow != window)
        {
            if (window.Started && window.Actived)
            {
                window.OnDeactive();
            }
            else
            {
                window.Canceled = true;
            }
            int current = m_self.m_windowGoList.IndexOf(window);

            if (current >= 1)
            {
                WindowBase win = m_self.m_windowGoList[current - 1];

                if (win != null)
                {
                    //Debug.Log("close window  " + window.MainObject.activeSelf);
                    win.MainObject.SetActive(window.MainObject.activeSelf);
                }
            }
            window.MainObject.SetActive(false);
        }
        //if (m_self.CurrentWindow == window)
        else
        {
            int current = m_self.m_windowGoList.IndexOf(window);

            if (current < 1)
            {
                #if UNITY_EDITOR
                Debug.LogWarning("BackWindow: No Window Behind!");
                #endif
                //return;
            }
            window.MainObject.SetActive(false);
            if (window.Started && window.Actived)
            {
                window.OnDeactive();
            }
            else
            {
                window.Canceled = true;
            }
            WindowBase win = null;
            if (current - 1 >= 0) win = m_self.m_windowGoList[current - 1];

            if (win != null)
            {
                bool active = true;
                for (int i = current + 1; i < m_self.m_windowGoList.Count; ++i)
                {
                    if (m_self.m_windowGoList[i].IsFullWindow)
                    {
                        active = false;
                        break;
                    }
                }
                //if we didn't hide current window when show a new window, no need to active it
                for (int i = current - 1; i >= 0; --i)
                {
                    WindowBase winTmp = m_self.m_windowGoList[i];
                    //Debug.LogWarning("Close Window, current window: " + winTmp.name + ", count = " + current);
                    if (!winTmp.MainObject.activeSelf && active)
                        winTmp.MainObject.SetActive(true);
                    if (winTmp.IsFullWindow) break;
                }

                //win.transform.localScale = Vector3.one;
                Vector3 pos = win.CatchedTransform.localPosition;
                win.CatchedTransform.localPosition = new Vector3(pos.x, pos.y, -1000);
                //win.transform.localEulerAngles = Vector3.zero;
                /*
                if(win.MainPanel != null)
                {
                    win.MainPanel.depth = 160;
                }
                */
                if (win.Started && !win.Actived)
                {
                    win.OnActive();
                }
                m_self.m_currentWindow = win;
            }
        }

        if (!window.Closed) window.OnClose();
        m_self.RemoveWindow(window);

        window.PostClose();

	}
    /*
	public static void ForwardWindow()
	{
		if(m_self == null || m_self.m_currentWindow == null)
		{
			#if UNITY_EDITOR
			Debug.LogError("ForwardWindow: No Window Showing!");
			#endif
			return;
		}
		
		int current = m_self.m_windowGoList.IndexOf(m_self.m_currentWindow);
		if(current >= m_self.m_windowGoList.Count - 1)
		{
			#if UNITY_EDITOR
			Debug.LogWarning("ForwardWindow: No Window Above!");
			#endif
			return;
		}

		WindowBase win = m_self.m_currentWindow;

		win.transform.localPosition = new Vector3(0, 0, (m_self.m_windowGoList.Count - current) * 50 - 200);
		
		//if(win.MainPanel != null)
		//{
		//	win.MainPanel.depth = current * 10;
		//}
     
		if(win.Started)
		{
			win.OnDeactive();
		}
		else
		{
			win.Canceled = true;
		}

		win = m_self.m_windowGoList[current + 1];

		//do we need to hide it?
		if(win != null && win.IsFullWindow)
		{
			m_self.m_currentWindow.MainObject.SetActive(false);
		}

		if(win != null)
		{
			win.MainObject.SetActive(true);
			m_self.m_currentWindow = win;
			if(win.Started)
			{
				win.OnActive();
			}
		}
	}
    */
	void Awake()
	{
		if(m_self != null)
		{
			DestroyImmediate(m_self.gameObject);
		}

		m_self = this;

		if(IsRetain)
		{
			DontDestroyOnLoad(gameObject);
		}

		if(CommonCanvas == null)
		{
			CommonCanvas = transform;
		}

        //PreOpenWindow((int)WindowType.ConfirmWindow);
        //PreOpenWindow((int)WindowType.WaittingWindow);
        //PreOpenWindow((int)WindowType.NotifyWindow);

        //PreOpenWindow((int)WindowType.GemMainWin);

        if (PreLoadWindows != null)
        {
            for (int i = 0; i < PreLoadWindows.Length; ++i)
            {
                PreOpenWindow(PreLoadWindows[i]);
            }
        }

		Transform root = transform.root;
		Canvas uiRoot = root == null ? null : root.GetComponent<Canvas>();
		if(uiRoot != null)
		{
            /*
			if(GlobalMacro.screenRatio >= 1.5)
			{
				uiRoot.maximumHeight = 640;
				uiRoot.minimumHeight = 640;
			}
			else if(GlobalMacro.screenRatio < 1.35)
			{
				uiRoot.maximumHeight = 1136;
                uiRoot.minimumHeight = 1136;
			}
			else
			{
				uiRoot.maximumHeight = 720;
				uiRoot.minimumHeight = 720;
			}
             * */
		}
	}

	// Use this for initialization
	void Start () 
	{
		if(FirstWindow > 0)
        {
           OpenWindow(FirstWindow);
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
        
	}

	void OnDestroy ()
	{
		/*
		for(int i = 0, max = m_windowGoList.Count; i < max; ++i)
		{
			WindowBase window = m_windowGoList[i];
			if(window != null && window.gameObject != null)
			{
				Destroy(window.gameObject);
			}
		}
		*/
		m_windowGoList.Clear();
		/*
		for(int i = 0, max = m_windowGoBack.Count; i < max; ++i)
		{
			WindowBase window = m_windowGoBack[i];
			if(window != null && window.gameObject != null)
			{
				Destroy(window.gameObject);
			}
		}
		*/
		m_windowGoBack.Clear();
		/*
		for(int i = 0, max = m_subWindowGoList.Count; i < max; ++i)
		{
			WindowBase window = m_subWindowGoList[i];
			if(window != null && window.gameObject != null)
			{
				Destroy(window.gameObject);
			}
		}
		*/
		m_subWindowGoList.Clear();
		m_self = null;
	}

	private void RemoveWindow(WindowBase win)
	{
		if(win != null)
		{
			if(win.IsSubWindow)
			{
				if(m_subWindowGoList.Contains(win))
				{
					m_subWindowGoList.Remove(win);
				}
			}
			else
			{
				if(m_windowGoList.Contains(win))
				{
					m_windowGoList.Remove(win);
					if(win.SortType == WindowBase.WindowSortType.AlwaysTop)
					{
						m_topWinNum--;
					}
				}
			}

			if(!win.IsDestroyWhenClose && !m_windowGoBack.Contains(win))
			{
				m_windowGoBack.Add(win);
                if (win.IsSubWindow) win.CatchedTransform.parent = CommonCanvas;
			}
		}
	}

	static public WindowBase GetWindow(GameObject go)
	{
		if (go == null) return null;

		WindowBase win = go.GetComponent<WindowBase>();

		if (win == null)
		{
			Transform t = go.transform.parent;
			
			while (t != null && win == null)
			{
				win = t.gameObject.GetComponent<WindowBase>();
				t = t.parent;
			}
		}

		return win;
	}

	static public WindowBase GetWindow(int winType)
	{
		if (m_self != null)
		{
			for(int i = 0, max = m_self.m_windowGoList.Count; i < max; ++i)
			{
				WindowBase window = m_self.m_windowGoList[i];
				if(window != null && window.WinType == winType)
				{
					return window;
				}
			}
		}
		
		return null;
	}

    static public int GetCurrentWindow()
    {
        if (m_self != null && m_self.m_currentWindow != null)
        {
            return m_self.m_currentWindow.WinType;
        }

        return -1;
    }

    static public int GetTopWindow()
    {
        if (m_self != null && m_self.m_windowGoList.Count > 0)
        {
            return m_self.m_windowGoList[0].WinType;
        }

        return -1;
    }

    static public void HideUI()
    {
        if (m_self != null)
        {
            m_self.gameObject.SetActive(false);
        }
    }

    static public void ShowUI()
    {
        if (m_self != null)
        {
            m_self.gameObject.SetActive(true);
        }
    }
}
