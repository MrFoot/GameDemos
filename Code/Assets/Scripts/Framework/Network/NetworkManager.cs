using UnityEngine;
using System.Collections;

public class NetworkManager : Singleton<NetworkManager> {

	//private string Url = "http://127.0.0.1/Garfield/rank.php";
    private string Url = "http://api.soulgame.mobi/index.php?r=bear/";

	public void Init() {

	}

    public void Send(string serviceUrl, soulgame.network.HttpDoneCallback callback, byte[] data)
    {
		string url = Url + serviceUrl;
        soulgame.network.HttpLite.HttpRequest(url, callback, data);
	}
}
