using UnityEngine;
using System.Collections;

public class SDKHandler : MonoBehaviour {

	void Awake() {
		DontDestroyOnLoad (this.gameObject);
	}

	public void LoginSucCallBack(string id) {
		//NGUIDebug.Log ("LoginSucCallBack " + id);
		Main.Instance.UserManager.SetUserId (id);
		//Main.Instance.GameNetwork.UserService.FirstInGame ();
	}

	public void LoginFailCallBack(string msg) {
		//NGUIDebug.Log ("LoginFailCallBack " + msg);
	}

	public void NetWorkChangeCallBack(string state) {
		//NGUIDebug.Log ("NetWorkChangeCallBack " + state);
		if (state == "wifi" || state == "mobile") {
			if (!Main.Instance.UserManager.HasUserId ()) {
				Main.Instance.SDKManager.GetDeviceID ();
			} else {
				//Main.Instance.GameNetwork.UserService.UploadUserInfo ();
			}
		}
	}

	public void onShareResult(string state) {
		//NGUIDebug.Log ("onShareResult " + state);
		Main.Instance.EventBus.FireEvent (EventId.SDK_SHARE, state);
	}

	public void PaySuccCallBack(string msg) {
		//NGUIDebug.Log ("PaySuccCallBack " + msg);
		Main.Instance.EventBus.FireEvent (EventId.SDK_PAY, new SDKManager.PayBackData(Main.Instance.SDKManager.CurrentPayId, true));
		Main.Instance.SDKManager.UmengPay (Main.Instance.SDKManager.CurrentPayId);
	}

	public void PayFailCallBack(string msg) {
		//NGUIDebug.Log ("PayFailCallBack " + msg);
		Main.Instance.EventBus.FireEvent (EventId.SDK_PAY, new SDKManager.PayBackData(Main.Instance.SDKManager.CurrentPayId, false));
	}
}
