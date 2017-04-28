using UnityEngine;
using System.Collections;
using System.Text;
using System;

public class GameNetwork {

	//public string Url = "http://123.206.181.32:9501/";
	//public string Url = "http://www.mybears.com/api/public/";
	public string Url = "http://xapi2.soulgame.mobi/";
	public string NewUrl = "http://xapi2.soulgame.mobi/";

	private GameObject waitWindow; 

	private HttpDoneCallback callb;

	private long requestMsgTime;

	public void Init() {

		RequestMsg ();
		//TimerManager.StartTimer (RequestMsg, 60f, true);
	}

	public void RequestMsg() {
		if (HasNet) {
			if (ServiceTime.CurUnixTime - this.requestMsgTime > 5) {
				this.requestMsgTime = ServiceTime.CurUnixTime;
			}
		}
	}

	public void UploadUserInfo() {
		//进度条访问网络，如果没有网络不上传数据
		if (Application.internetReachability == NetworkReachability.NotReachable) {
			SceneSwitcher.IsNetLoaded = true;
			//Debug.Log ("UploadUserInfo not net");
			return;
		}
	}

	public bool HasNet {
		get {
			return Application.internetReachability != NetworkReachability.NotReachable;
		}
	}

	public void OpenWaitWindow(HttpDoneCallback callback) {
		callb = callback;
		//waitWindow = (WaitWindow)WindowManager.OpenWindow ((int)WindowType.WaitWin);
	}

	public bool WaitWindowIsOpened () {
		if (waitWindow == null)
			return false;
		return !waitWindow.activeSelf;
	}

	/// <summary>
	/// 直接关闭等待窗口
	/// </summary>
	public void CloseWaitWindow() {
		if (waitWindow == null)
			return;
		//waitWindow.Close ();
	}

	/// <summary>
	/// 关闭等待并且取消网络回调
	/// </summary>
	public void CloseWaitAndCancelCallBack() {
		if (waitWindow == null)
			return;
		if (callb != null)
			HttpLite.CancelHttpRequest (callb);
		//waitWindow.Close ();
	}

	public void SetServiceTime(string time) {
		long t;

		if (long.TryParse(time, out t))
		{
			ServiceTime.CurUnixTime = t;
		}
	}

//	public void Send(string serviceUrl, HttpDoneCallback callback, string data) {
//		string url = Url + serviceUrl;
//		HttpLite.HttpRequest (url, callback, data);
//	}
}
