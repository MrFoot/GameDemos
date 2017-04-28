using UnityEngine;
using System.Collections;

public abstract class IPlatformSDK {

	public abstract void Init ();

	public abstract void GetDeviceId ();

	public abstract void AskForNetwork ();

	public abstract void DoPay (int product);

	public abstract void SendNotify (int id, string title, string context, string icon, string sound, string className, int time);

	public abstract void CancelNotify (int id);

	public abstract void UmengEvent(string ev);

	public abstract void StartLevel (int level);
    public abstract void FinishLevel(int level);
    public abstract void FailLevel(int level);

	public abstract string GetImagePath();

    public abstract void MicphonePermission();

	public abstract void ShareImage (string image);

	public abstract void ShareContext(string title, string context, string url, string icon);

	public abstract void Pay(double cash, int diamond);
	public abstract void Pay(double cash, string item, int amount);

    public abstract void Buy(string item, int amount, double price);

	public abstract void Use(string item, int amount, double price);

	public abstract void ExitGame ();

	public abstract void MoreGame ();

	public abstract void ReflashPhoto (string path);

	public abstract string GetSdcardPath();

	public abstract void CallPhone (string num);

	public abstract void CopyString (string str);

	public abstract bool verifyPack ();

	public abstract string GetMetaValue (string name);
}
