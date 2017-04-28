using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using FootStudio.Util;

public class PushManager {

	private int pushMinTime = 10;

	private int pushSpan = 120;

	private List<PushData> pushList = new List<PushData>();

	public void Init() {
		pushList.Clear ();
	}

	public void OnAppPause() {
		//Debug.Log ("pushmanager push");
		pushList.Clear ();
		CaculateEat ();
		CaculatePee ();
		CaculateWakeUp ();
		CaculateFarm ();
		CaculateDay ();

		for (int i = 0; i < pushList.Count; i++) {
			//Debug.Log (pushList [i].Context + " " + pushList [i].Time);
			Main.Instance.SDKManager.SendNotify (i + 1, pushList [i].Title, pushList [i].Context, SDKManager.GameIcon, pushList[i].Sound, SDKManager.ClassName, pushList [i].Time);
		}
//		UserPrefs.SetCollection
	}

	public void OnAppResume() {
		//Debug.Log ("pushmanager resume");

		for (int i = 1; i < 6; i++) {
			Main.Instance.SDKManager.CancelNotify (i);
		}
	}

	/// <summary>
	/// 计算要吃食物
	/// </summary>
	private void CaculateEat() {
		int needTime = 10;
		AddPushData (needTime, "我饿了，快来喂我吃的吧！", "s3016");
	}

	/// <summary>
	/// 上厕所
	/// </summary>
	private void CaculatePee() {
        int needTime = 10;
		AddPushData (needTime, "快点，快点，我要上厕所！","s3023");
	}

	/// <summary>
	/// 醒了
	/// </summary>
	private void CaculateWakeUp() {
        int needTime = 10;
		AddPushData (needTime, "我醒了，快来跟我一起玩吧！","s3013");
	}

	/// <summary>
	/// 农场
	/// </summary>
	private void CaculateFarm() {
        int needTime = 10;
		AddPushData (needTime, "农场的水果蔬菜成熟了，快去收获吧！","s3011");
	}

	/// <summary>
	/// 定时通知，一般设置一天
	/// </summary>
	private void CaculateDay() {
		AddPushData (60*60, "好无聊哦，快来跟我玩吧！","s3001");
	}

	private void AddPushData(int needTime, string context, string sound) {
		PushData push = new PushData ();
		push.Title = SDKManager.GameName;
		push.Context = context;
		push.Sound = sound;
		push.Time = needTime + pushSpan;
		AddPushList (push);
	}

	private void AddPushList(PushData push) {
		for(int i=0; i<pushList.Count; i++) {
			if (push.Time > pushList [i].Time - pushSpan && push.Time < pushList [i].Time + pushSpan) {
				push.Time = pushList [i].Time + pushSpan;
				AddPushList (push);
				return;
			}
		}
		pushList.Add (push);
	}
}
