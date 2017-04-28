using UnityEngine;
using System.Collections;
using FootStudio.Util;
using FootStudio.Framework;
using System;
using System.IO;
using ZXing;
using ZXing.QrCode;

public class ShareManager {

	private bool isTodayShare;

	//private ShareWindow ShareWindow;

	private bool isShareSuc;

	private const string CurrentShareDayKey = "ShareManager.CurrentShareDay";

	private DateTime CurrentShareTime;

	private int AllTimes;
	private int DayTimes;
	private int CurrentTimes;

	/// <summary>
	/// 分享页面的链接更改为  page.soulgame.mobi/xiong/index.html?mac=123&timestamp=1321312&token=c6a91c704ac0421f5d92e20836f99ef9&channel=xiaomi
	///mac 为 手机唯一标识(sdk_id)
	///timestamp 为 时间戳 类似：1492156016
	///token 为 md5($timestamp . md5($mac . 'xiongdada'))
	///channel 为 渠道类型；需要一个配表（所有渠道）
	/// </summary>
	/// <value>The share URL.</value>
	private string shareUrl {
		get {
			return string.Format ("http://page.soulgame.mobi/xiong/index.html?mac={0}&timestamp={1}&token={2}&channel={3}",
				Main.Instance.UserManager.GetUserId(),
				ServiceTime.CurUnixTime.ToString(),
				MD5Tool.GetMd5Hash(ServiceTime.CurUnixTime.ToString() + MD5Tool.GetMd5Hash(Main.Instance.UserManager.GetUserId() + "xiongdada")),
				UmengController.ChannelItem.soulgame_channel); 
			//"http://xiong.soulgame.mobi/wap/index/share/" + Main.Instance.UserManager.GetShareID () + ".html";
		}
	}

	public static bool isApply = false;
	public static Texture2D encoded = new Texture2D(256, 256);

	public void Init() {
		//默认分享过了
		isShareSuc = false;
	}

	public void SetData(int allTimes, int dayTimes, int currentTimes) {
		this.AllTimes = allTimes;
		this.DayTimes = dayTimes;
		this.CurrentTimes = currentTimes;
		#if UNITY_EDITOR
		Debug.Log("all:" + this.AllTimes + ", day:" + this.DayTimes + ", current:" + this.CurrentTimes);
		#endif
		Main.Instance.PurchaseManager.BuyCoin (this.CurrentTimes * 10);
	}

	public void OpenShareWindow() {
        //this.ShareWindow = (ShareWindow)WindowManager.OpenWindow ((int)WindowType.ShareWindow);
        //#if UNITY_EDITOR
        //Debug.Log(Main.Instance.UserManager.GetUserId());
        //Debug.Log(ServiceTime.CurUnixTime.ToString());
        //Debug.Log(MD5Tool.GetMd5Hash(ServiceTime.CurUnixTime.ToString() + MD5Tool.GetMd5Hash(Main.Instance.UserManager.GetUserId() + "xiongdada")));
        //Debug.Log(UmengController.ChannelItem.soulgame_channel);
        //Debug.Log(shareUrl);
        //#endif
        //GetEncodeTexture ();
        //this.ShareWindow.Texture.mainTexture = encoded;
        //SetLabel ();
	}

	private void SetLabel() {
        //this.ShareWindow.ChangeAllLabel ((this.AllTimes * 10).ToString ());
        //this.ShareWindow.ChangeTodayLabel ((this.DayTimes * 10).ToString ());
	}

	public void ShareBtn() {
		//AddOnItem addon = Main.Instance.AddOnManager.GetAddOn(9101401);
		//Main.Instance.AddOnManager.BuyAddOn (addon, true);
		Main.Instance.EventBus.RemoveListener (EventId.SDK_SHARE, new OnEvent (ShareCallBack));
		Main.Instance.EventBus.AddListener (EventId.SDK_SHARE, new OnEvent (ShareCallBack));
        Main.Instance.SDKManager.ShareContext("国民动漫熊大、熊二、光头强，陪您欢乐闹翻天！", "《我的熊大熊二》全新玩法重磅来袭！特色养成、温馨农场，超多玩法让您停不下来！", shareUrl);
	}

	public void ShareCallBack(object state) {
		Main.Instance.EventBus.RemoveListener (EventId.SDK_SHARE, new OnEvent (ShareCallBack));
		string info = (string)state;
		if (info == "ok") {
			//WindowManager.OpenWindow ((int)WindowType.TipsWindow, "分享成功！");

		} else {
			//WindowManager.OpenWindow ((int)WindowType.TipsWindow, "分享失败！");
		}
	}

	public void ShareMiniGame() {
		GameObject go = (GameObject)Resources.Load ("UI/ShareWindow/UI Root Shoot", typeof(GameObject));
		GameObject result = UnityEngine.Object.Instantiate<GameObject>(go);
		ScreenShoot ss = result.GetComponent<ScreenShoot> ();
		ss.Shoot ();
	}

	public static string PersistentDataPath
	{
		get
		{
			string text = Application.persistentDataPath;
			if (!Directory.Exists(text))
			{
				try
				{
					Directory.CreateDirectory(text);
				}
				catch (Exception ex)
				{
					Debug.LogError(string.Format("FileUtils.PersistentDataPath - Error creating {0}. Exception={1}", text, ex.Message));
				}
			}
			return text;
		}
	}

	public static void ClearPrefabs() {
		UserPrefs.Remove (CurrentShareDayKey);
	}


	public static Color32[] Encode(string textForEncoding, int width, int height)  
	{  
		var writer = new BarcodeWriter  
		{  
			Format = BarcodeFormat.QR_CODE,  
			Options = new QrCodeEncodingOptions  
			{  
				Height = height,  
				Width = width  
			}  
		};  
		return writer.Write(textForEncoding);  
	} 

	public void GetEncodeTexture() {
		if (isApply)
			return;
		Color32[] color32 = Encode(shareUrl, encoded.width, encoded.height);  
		encoded.SetPixels32(color32);   //根据转换来的32位颜色值来计算二维码的像素  
		encoded.Apply();
	}
}
