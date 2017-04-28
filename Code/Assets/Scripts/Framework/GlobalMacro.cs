using UnityEngine;
using System.Collections;
using System.Collections.Generic;


/// <summary>
///作用：全局宏、枚举和常量
///第二启动界面，网络消息接收循环
///作者：陈翔
/// </summary>
public static class GlobalMacro 
{
    public const int GAME_VERSION_INT = 100;

	public const string GAME_VERSION_NAME = "1.0.2";

	public const int CST_MAX_CHARACTER_NUM = 3;
	
	public const float GLOBAL_GRAVITY = 20f;
	public const float GLOBAL_GRAVITY_RECIP = 0.05f;
	public const float MIN_AIR_HIT_HEIGHT = 0.3f;
	public const float NORMAL_HIT_HEIGHT_IN_AIR = 0.1f;

	public const float CHARACTER_GROUNDED_TO_STANDUP_TIME = 0f;
	public const float CHARACTER_DEAD_PREPARE_TIME = 0.2f;
	
	public const float PKMODE_HP_ANI_DURATION = 1f;
	
	public const string CST_ENEMY_TAG_NAME = "enemy";
	public const string CST_BREAKBOX_TAG_NAME = "breakableObject";
	public const string CST_PLAYER_TAG_NAME = "player";
	
	public const float DISTANCE_TOUCH_PLAYER = 2.56f;


	public static byte[] Key = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
	public static byte[] IV = { 0xAB, 0xCD, 0xEF, 0x12, 0x34, 0x56, 0x78, 0x90};
	#if UNITY_EDITOR
	public static string dataPath = Application.dataPath;
	#else
	public static string dataPath = Application.persistentDataPath;
	#endif
	public static float screenDpi = 240;
	public static float screenRatio = 1.777778f;
	//public static Color[] DIFFICULTY_SHOW_COLOR = {new Color(0.5078f, 0.42188f, 0.24219f), new Color(0.5664f, 0.14844f, 0.5586f), new Color(0.9453f, 0.39453f, 0.1289f)};
	public static string[] DIFFICULTY_SHOW_COLOR ={"[fce7ba]", "[fff200]", "[00a650]"};
}