using System;
using FootStudio.Util;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;
using UnityEngine;


public class UserManager
{
	public enum Role
	{
		None,
		Big = 901001,
		Two = 902001,
	}

	public static string UserIdKey = "UserManager.UserId";
	public static string UserNameKey = "UserManager.UserName";
	public static string UserCoinKey = "UserManager.UserCoin";
	public static string UserDiamondKey = "UserManager.UserDiamond";
	public static string UserXPKey = "UserManager.UserXP";
	public static string UserCreateTimeKey = "UserManager.CreateTime";
	public static string UserGiveTimeKey = "UserManager.GiveTime";
	public static string UserShareIDKey = "UserManager.ShareID";

	private string CoinEncrypt;
	private string XPEncrypt;
	private string DiamondEncrypt;
	private string ShareEncrypt;

	private int Coin = 0;
	private double XP = 0.0;
	private int Diamond = 0;

	public void Init() {
		LoadUserCoin ();
		LoadUserDiamond ();
		LoadUserXP ();
		SaveCreateTime ();
		//测试 
		//this.SetDiamond(50);

	}

	public void FirstIn() {
		this.SetCoin (1000);
		this.SetDiamond (100);
	}

	#region coin
	public void LoadUserCoin() {
		CoinEncrypt = UserPrefs.GetString (UserCoinKey, "");
		if (CoinEncrypt != "") {
			int c;
			if (int.TryParse (EncryptUtils.Base64Decrypt (CoinEncrypt), out c)) {
				this.Coin = c;
			} else {
				this.Coin = 0;
			}
		} else {
			SaveCoin ();
		}
	}

	public void SetCoin(int coin) {
		this.Coin = coin;
		SaveCoin ();
	}

	public int GetCoin() {
		return this.Coin;
	}

	/// <summary>
	/// 传的需要增加的值
	/// </summary>
	/// <returns>The coin.</returns>
	/// <param name="coin">Coin.</param>
	public int ChangeCoin(int coin) {
		int c;
		if (int.TryParse (EncryptUtils.Base64Decrypt (CoinEncrypt), out c)) {
			this.Coin = c + coin;
		} else {
			GameLog.Error ("金币解析异常");
		}
		SaveCoin ();
		return this.Coin;
	}

	public void SaveCoin() {
		if (this.Coin < 0)
			this.Coin = 0;
		//NGUIDebug.Log ("save coin " + Coin);
		CoinEncrypt = EncryptUtils.Base64Encrypt (Coin.ToString ());
		UserPrefs.SetString (UserCoinKey, CoinEncrypt);
		UserPrefs.SaveDelayed();
	}
	#endregion

	#region diamond
	public void LoadUserDiamond() {
		DiamondEncrypt = UserPrefs.GetString (UserDiamondKey, "");
		if (DiamondEncrypt != "") {
			int c;
			if (int.TryParse (EncryptUtils.Base64Decrypt (DiamondEncrypt), out c)) {
				this.Diamond = c;
			} else {
				this.Diamond = 0;
			}
		} else {
			SaveDiamond ();
		}
	}

	public void SetDiamond(int diamond) {
		this.Diamond = diamond;
		SaveDiamond ();
	}

    public int GetDiamond()
    {
        return this.Diamond;
    }

	/// <summary>
	/// 传的需要增加的值
	/// </summary>
	/// <returns>The coin.</returns>
	/// <param name="coin">Coin.</param>
	public int ChangeDiamond(int diamond) {
		int c;
		if (int.TryParse (EncryptUtils.Base64Decrypt (DiamondEncrypt), out c)) {
			this.Diamond = c + diamond;
		} else {
			GameLog.Error ("钻石解析异常");
		}
		SaveDiamond ();
		return this.Diamond;
	}

	public void SaveDiamond() {
		//NGUIDebug.Log (this.Diamond);
		if (this.Diamond < 0)
			this.Diamond = 0;
		DiamondEncrypt = EncryptUtils.Base64Encrypt (Diamond.ToString ());
		UserPrefs.SetString (UserDiamondKey, DiamondEncrypt);
		UserPrefs.SaveDelayed();
	}
	#endregion

	#region XP
	public void LoadUserXP() {
		XPEncrypt = UserPrefs.GetString (UserXPKey, "");
		if (XPEncrypt != "") {
			double c;
			if (double.TryParse (EncryptUtils.Base64Decrypt (XPEncrypt), out c)) {
				this.XP = c;
			} else {
				this.XP = 0.0;
			}
		} else {
			SaveXP ();
		}
	}

	public double GetXP() {
		return this.XP;
	}

	/// <summary>
	/// 传的需要增加的经验值
	/// </summary>
	/// <returns>The coin.</returns>
	/// <param name="coin">Coin.</param>
	public double ChangeXP(double xp) {
		double c;
		if (double.TryParse (EncryptUtils.Base64Decrypt (XPEncrypt), out c)) {
			this.XP = c + xp;
		} else {
			GameLog.Error ("经验值解析异常");
		}
		SaveXP ();
		return this.XP;
	}

	public void SetXp(double xp) {
		this.XP = xp;
		SaveXP ();
	}

	public void SaveXP() {
		XPEncrypt = EncryptUtils.Base64Encrypt (XP.ToString ());
		UserPrefs.SetString (UserXPKey, XPEncrypt);
		UserPrefs.SaveDelayed();
	}
	#endregion

	#region id
	public string GetUserId() {
		string id = UserPrefs.GetString (UserIdKey, "");
		if (string.IsNullOrEmpty (id)) {
			Main.Instance.SDKManager.GetDeviceID ();
		}
		return id;
	}

	public bool HasUserId() {
		//Debug.Log (UserPrefs.GetString (UserIdKey, ""));
		return !string.IsNullOrEmpty (UserPrefs.GetString (UserIdKey, ""));
	}

	public void SetUserId(string id) {
		//Debug.Log (id);
		UserPrefs.SetString (UserIdKey, id);
		UserPrefs.Save ();
	}
	#endregion

	#region CreateTime
	public void SaveCreateTime() {
		if (!UserPrefs.HasKey(UserCreateTimeKey))
			UserPrefs.SetDateTime (UserCreateTimeKey, DateTime.Now);
	}

	public void SetCreateTime(string time) {
		long t;
		if (long.TryParse(time, out t))
		{
			UserPrefs.SetDateTime (UserCreateTimeKey, Utils.Unix2DateTime(t));
		}
	}

    public void SetCreateTime(DateTime time)
    {
        UserPrefs.SetDateTime(UserCreateTimeKey, time);
    }

	public DateTime GetCreateTime() {
		return UserPrefs.GetDateTime (UserCreateTimeKey, DateTime.Now);
	}
	#endregion

	#region shareid
	public void SetShareID(int id) {
		UserPrefs.SetInt (UserShareIDKey, id);
		UserPrefs.SaveDelayed ();
	}

	public int GetShareID() {
		return UserPrefs.GetInt (UserShareIDKey, 0);
	}
	#endregion

	public static void ClearPrefs() {
		UserPrefs.Remove(UserCoinKey);
		UserPrefs.Remove(UserDiamondKey);
		UserPrefs.Remove(UserXPKey);
		UserPrefs.Remove(UserIdKey);
		UserPrefs.Remove(UserNameKey);
		UserPrefs.Remove(UserCreateTimeKey);
		UserPrefs.Remove(UserGiveTimeKey);
		UserPrefs.Remove(UserShareIDKey);
	}
}


