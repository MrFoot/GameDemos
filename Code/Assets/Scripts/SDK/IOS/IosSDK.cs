using UnityEngine;
using System.Collections;

public class IosSDK : IPlatformSDK {

	private UmengController Umeng;

	public override void Init ()
	{
		this.Umeng = new UmengController ();
	}

	public override void GetDeviceId ()
	{
		Main.Instance.UserManager.SetUserId(SystemInfo.deviceUniqueIdentifier);
	}

	public override void AskForNetwork ()
	{

	}

	public override void DoPay (int product)
	{
		Main.Instance.EventBus.FireEvent (EventId.SDK_PAY, new SDKManager.PayBackData(product, true));
	}

	public override void SendNotify (int id, string title, string context, string icon, string sound, string className, int time)
	{

	}

	public override void CancelNotify (int id)
	{

	}

	public override void UmengEvent (string ev)
	{
		this.Umeng.Event (ev);
	}

    public override void StartLevel(int level)
    {
        Debug.Log("StartLevel" + level);
    }

    public override void FinishLevel(int level)
    {
        Debug.Log("FinishLevel" + level);
    }

    public override void FailLevel(int level)
    {
        Debug.Log("FailLevel" + level);
    }

	public override string GetImagePath ()
	{
		return Application.persistentDataPath;
	}

	public override void MicphonePermission()
	{
		Debug.Log("Set Micphone Permission");
	}

	public override void ShareImage (string image)
	{
		Debug.Log ("Share Image " + image);
	}

	public override void ShareContext (string title, string context, string url, string icon)
	{
		Debug.Log ("Share Context " + title + "," + context + "," + url);
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
		//WindowManager.OpenWindow((int)WindowType.CommonConfirmWindow);
	}

	public override void MoreGame() {
		Debug.Log ("More Game Btn Click!");
	}

	public override void ReflashPhoto (string path)
	{
		Debug.Log ("ReflashPhoto " + path);
	}

	public override string GetSdcardPath ()
	{
		return Application.persistentDataPath;
	}

	public override void CallPhone (string num)
	{
		Debug.Log ("打电话 " + num);
	}

	public override void CopyString (string str)
	{
		Debug.Log ("复制id " + str);
	}

	public override bool verifyPack ()
	{
		return true;
	}

	public override string GetMetaValue (string name)
	{
		return "";
	}
}
