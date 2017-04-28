using UnityEngine;
using System.Collections;

public class EditorSDK : IPlatformSDK {

	public override void Init ()
	{
		
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
		Debug.Log ("event " + ev);
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
		return ShareManager.PersistentDataPath;
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
		Debug.Log ("Pay " + cash + " buy " + diamond + " diamond");
	}

	public override void Pay (double cash, string item, int amount)
	{
		Debug.Log ("Pay " + cash + " buy " + amount + " " + item);
	}

    public override void Buy(string item, int amount, double price)
    {
        Debug.Log("Buy " + price + " buy " + amount + " " + item);
    }

	public override void Use(string item, int amount, double price)
	{
		Debug.Log("Use " + price + " use " + amount + " " + item);
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
