using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

public class AndroidSDK : IPlatformSDK {

	private AndroidJavaObject mJavaObject;
	private AndroidJavaClass mJavaClass;
	private AndroidJavaClass mSoulSDKClass;
	private AndroidJavaClass mSoulNotifyClass;
	private AndroidJavaClass mSoulShareClass;
    private AndroidJavaClass mSoulMicphoneClass;


	private UmengController Umeng;

	[DllImport ("soulgame_checkmd5")]
	private static extern bool soulgameCheckMD5InUnity3D();

	public override void Init ()
	{
		this.mJavaClass = new AndroidJavaClass ("com.unity3d.player.UnityPlayer");
		this.mJavaObject = mJavaClass.GetStatic<AndroidJavaObject> ("currentActivity");
		this.mSoulSDKClass = new AndroidJavaClass ("com.soulgame.SoulSdk");
		//this.mSoulSDKClass.CallStatic ("setDebugMode", true);
		this.mSoulNotifyClass = new AndroidJavaClass ("com.soulgame.notification.NotifyUtil");
		this.mSoulShareClass = new AndroidJavaClass ("com.soulgame.share.ShareManager");

        this.mSoulMicphoneClass = new AndroidJavaClass("com.soulgame.bear.CheckRecordPermission");

		this.Umeng = new UmengController ();
	}

	public override void GetDeviceId ()
	{
		this.mSoulSDKClass.CallStatic ("login");
	}

	public override void AskForNetwork ()
	{
		this.mSoulSDKClass.CallStatic ("openNetSettings", this.mJavaObject);
	}
	public override void DoPay (int product)
	{
		#if UNITY_EDITOR
		Main.Instance.EventBus.FireEvent (EventId.SDK_PAY, new SDKManager.PayBackData(product, true));
		#elif FREE
		Main.Instance.EventBus.FireEvent (EventId.SDK_PAY, new SDKManager.PayBackData(product, true));
		#else
		SDKProduct.ProductData data = SDKProduct.Products [product];
       	this.mSoulSDKClass.CallStatic ("pay", this.mJavaObject, data.Key);
		#endif
		Main.Instance.SDKManager.UmengPay (product);
	}

	public override void SendNotify (int id, string title, string context, string icon, string sound, string className, int time)
	{
		this.mSoulNotifyClass.CallStatic ("sendDelayNotify", this.mJavaObject, id, title, context, icon, sound, className, time);
	}

	public override void CancelNotify (int id)
	{
		this.mSoulNotifyClass.CallStatic ("cancelDelayNotify", this.mJavaObject, id);
	}

	public override void UmengEvent (string ev)
	{
		this.Umeng.Event (ev);
	}

	public override void StartLevel(int level) {
        this.Umeng.StartLevel(level);
	}

	public override void FinishLevel(int level) {
        this.Umeng.FinishLevel(level);
	}

    public override void FailLevel(int level)
    {
        this.Umeng.FailLevel(level);
	}

	public override string GetImagePath ()
	{
		return ShareManager.PersistentDataPath;
	}

    public override void MicphonePermission()
    {
        mSoulMicphoneClass.CallStatic("check", mJavaObject);
    }

	public override void ShareImage (string image)
	{
		//Debug.Log ("Share Image " + image);
		mSoulShareClass.CallStatic("shareActionBoard", this.mJavaObject, image);
	}

	public override void ShareContext (string title, string context, string url, string icon)
	{
		//Debug.Log ("Share Context " + title + "," + context + "," + url);
		mSoulShareClass.CallStatic("shareActionBoard", this.mJavaObject, title, context, url, icon);
	}

	public override void Pay (double cash, int diamond)
	{
		this.Umeng.Pay (cash, diamond);
	}

	public override void Pay (double cash, string item, int amount)
	{
		this.Umeng.Pay (cash, item, amount);
	}

    public override void Buy(string item, int amount, double price)
    {
        this.Umeng.Buy(item, amount, price);
    }

	public override void Use(string item, int amount, double price)
	{
		this.Umeng.Use(item, amount, price);
	}

	public override void ExitGame() {
		this.mSoulSDKClass.CallStatic ("exit", this.mJavaObject);
	}

	public override void MoreGame() {
		this.mSoulSDKClass.CallStatic ("moreGame", this.mJavaObject);
	}

	public override void ReflashPhoto (string path)
	{
		this.mSoulSDKClass.CallStatic ("sendRefreshPhotoBroadcast", this.mJavaObject, path);
	}

	public override string GetSdcardPath ()
	{
		return this.mSoulSDKClass.CallStatic<string> ("getSDCardPath");
	}

	public override void CallPhone (string num)
	{
		this.mSoulSDKClass.CallStatic ("callPhone", num);
	}

	public override void CopyString (string str)
	{
		this.mSoulSDKClass.CallStatic ("copyContent", str);
	}

	public override bool verifyPack ()
	{
		return soulgameCheckMD5InUnity3D ();
		//return true;
	}

	public override string GetMetaValue (string name)
	{
		return this.mSoulSDKClass.CallStatic<string> ("getMetaData", this.mJavaObject, name);
	}
}
