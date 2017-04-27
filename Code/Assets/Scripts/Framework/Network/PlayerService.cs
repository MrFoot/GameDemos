using UnityEngine;
using System.Collections;
using ServiceDat;
using System;
using System.IO;
using System.Text;

public class PlayerService : BaseService<PlayerService>
{

	public static string IMEI_PREFS = "imei_prefs_name";

	public override void Init ()
	{
		base.Init ();
        ServiceUrl = "getRandUser";
        //ServiceUrl = "getUser";
		GetInfo ();
	}

    public void UpdateLoadSelfData()
    {
        UploadItemInfo info = new UploadItemInfo();
		//info.imei = SDkManager.Instance.GetDeviceId ();
        info.imei = "asdfsdf";
		info.name = "";
        info.item = "123456789";
		SendData (info, GetPlayerInfoCallback, false);
    }

    public void GetRandUserData()
    {
        SendData (GetPlayerInfoCallback, false);
    }

	private void GetInfo() {
        UploadItemInfo info = new UploadItemInfo();
		//info.imei = SDkManager.Instance.GetDeviceId ();
        info.imei = "asdfsdf";
		info.name = "";
        info.item = "123456789";
		SendData (info, GetPlayerInfoCallback, false);
	}

    private void GetPlayerInfoCallback(byte[] data, soulgame.network.HttpProc.HttpState state)
    {
        if (state != soulgame.network.HttpProc.HttpState.E_HTTP_STATE_DOWNLOAD_FINISHED)
        {
            //
			return;
		}

		Debug.Log (Encoding.UTF8.GetString(data));

		try {
            MemoryStream data_ms = new MemoryStream(data);
            UploadItemInfo mPlayerInfo = ProtoBuf.Serializer.Deserialize<UploadItemInfo>(data_ms);
            Debug.Log(mPlayerInfo.name + "," + mPlayerInfo.item + "," + mPlayerInfo.imei);
        }
        catch (Exception e)
        {
			Debug.LogError(e.ToString());
		}
	}
}
