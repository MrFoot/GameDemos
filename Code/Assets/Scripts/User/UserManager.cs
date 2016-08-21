using System;
using Soulgame.Util;
using System.Collections.Generic;


public class UserManager
{

	public static string UserInfoKey = "UserManager.Userinfo";
	public static string UserBoughtAddOnsKey = "UserManager.UserBoughtAddOns";
	public static string UserEnabledAddOnsKey = "UserManager.UserEnabledAddOns";
	public static string UserNewlyUnlockedAddOnsKey = "UserManager.UserNewlyUnlockedAddOns";

	public void Init() {

	}


	public static void ClearPrefs() {
		UserPrefs.Remove(UserInfoKey);
		UserPrefs.Remove(UserBoughtAddOnsKey);
		UserPrefs.Remove(UserEnabledAddOnsKey);
		UserPrefs.Remove(UserNewlyUnlockedAddOnsKey);
	}
}


