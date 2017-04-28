using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using FootStudio.Util;

public class SDKManager {

	public class PayBackData {
		public int Id;
		public bool State;

		public PayBackData(int id, bool state) {
			this.Id = id;
			this.State = state;
		}
	}

	public static string BundleVersion = "1.0.0";
	public static int BundleVersionCode = 100;

	public static string GameName = "我的熊大熊二";

    public static string ClassName = "com.soulgame.bear.LaunchActivity";

	public const string GameIcon = "app_icon";

	private IPlatformSDK sdk;

	public int CurrentPayId;

	public void Init() {
		
		#if UNITY_EDITOR
		sdk = new EditorSDK();
		BuglyAgent.ConfigDebugMode (true);
		#elif UNITY_ANDROID
		//sdk = new EditorSDK();
		sdk = new AndroidSDK();
		BuglyAgent.ConfigDebugMode (false);
		//BuglyAgent.InitWithAppId ("79668fd96a");
		#elif UNITY_IPHONE
		sdk = new IosSDK();
		BuglyAgent.ConfigDebugMode (false);
        BuglyAgent.InitWithAppId("0d4290d8fc");
        #endif
        BuglyAgent.EnableExceptionHandler ();
		sdk.Init ();

		if (!Main.Instance.UserManager.HasUserId ()) {
			GetDeviceID ();
		}
	}

	/// <summary>
	/// 取设备id
	/// </summary>
	/// <returns>The device I.</returns>
	public void GetDeviceID() {
		sdk.GetDeviceId ();
	}
	/// <summary>
	/// 提示用户打开网络
	/// </summary>
	public void OpenNet() {
		sdk.AskForNetwork ();
	}

	#region umeng
	public void UmengEvent(string ev) {
		this.sdk.UmengEvent (ev);
	}

	public void StartLevel(int level) {
        this.sdk.StartLevel(level);
	}

    public void FinishLevel(int level)
    {
        this.sdk.FinishLevel(level);
	}

    public void FailLevel(int level)
    {
        this.sdk.FailLevel(level);
	}

	public void UmengPay(int id) {
		SDKProduct.ProductData data = SDKProduct.Products [id];
        if (id == SDKProduct.Diamond50 || id == SDKProduct.Diamond150 || id == SDKProduct.Diamond500)
        {
            this.sdk.Pay(data.Price, data.Name, data.Amount);
		} else {
            this.sdk.Pay(data.Price, data.Amount);
		}
	}

    public void UmengBuy(string item, int amount, int price)
    {
		this.sdk.Buy (item, amount, price);
    }

	public void UmengUse(string item, int amount, int price)
	{
		this.sdk.Use (item, amount, price);
	}
	#endregion
	public void DoPay(int product) {
		this.CurrentPayId = product;
		this.sdk.DoPay (product);
	}

	public void SendNotify (int id, string title, string context, string icon, string sound, string className, int time)
	{
		this.sdk.SendNotify (id, title, context, icon, sound, className, time);
	}

	public void CancelNotify (int id)
	{
		this.sdk.CancelNotify (id);
	}

	#region share
	public string GetImagePath() {
		return this.sdk.GetImagePath ();
	}

    public void MicphonePermission()
    {
        sdk.MicphonePermission();
    }

	public void ShareImage (string image)
	{
		this.sdk.ShareImage (image);
	}

	public void ShareContext (string title, string context, string url, string icon = SDKManager.GameIcon)
	{
		this.sdk.ShareContext (title, context, url, icon);
	}
	#endregion



	public void ExitGame() {
		this.sdk.ExitGame ();
	}

	public void MoreGame() {
		this.sdk.MoreGame ();
	}

	public void ReflashPhoto(string path) {
		this.sdk.ReflashPhoto (path);
	}

	public string GetSdcardPath() {
		return this.sdk.GetSdcardPath ();
	}

	public void CallPhone (string num)
	{
		this.sdk.CallPhone (num);
	}

	public void CopyString (string str)
	{
		this.sdk.CopyString (str);
	}

	public void VerifyPack() {
		if (!this.sdk.verifyPack ()) {
			//WindowManager.OpenWindow ((int)WindowType.TipsWindow, "请下载官方正版游戏！", new TipsWindowOkBtn(this.TipCallBack), new TipsWindowOkBtn(this.TipCallBack));
		}
	}

	private void TipCallBack() {
		Application.Quit ();
	}

		public string GetMetaValue(string name) {
		return this.sdk.GetMetaValue (name);
		}

}
