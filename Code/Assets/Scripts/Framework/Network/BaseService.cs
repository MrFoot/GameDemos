using UnityEngine;
using System.Collections;
using ProtoBuf;
using System.Text;

public abstract class BaseService<T>
{

    protected abstract string ServiceUrl
    {
        get;
    }

    protected void SendData<K>(K msg, HttpDoneCallback callback, bool openWait = false, bool askNet = false) where K : IExtensible
    {
        if (askNet)
        {
            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                Main.Instance.SDKManager.OpenNet();
                return;
            }
        }

        if (openWait)
        {
            Main.Instance.GameNetwork.OpenWaitWindow(callback);
        }

        HttpLite.HttpRequest<K>(Main.Instance.GameNetwork.Url + ServiceUrl, callback, msg);
    }

    protected void SendDataNew<K>(K msg, HttpDoneCallback callback, bool openWait = false, bool askNet = false) where K : IExtensible
    {
        if (askNet)
        {
            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                Main.Instance.SDKManager.OpenNet();
                return;
            }
        }

        if (openWait)
        {
            Main.Instance.GameNetwork.OpenWaitWindow(callback);
        }
#if UNITY_EDITOR
        Debug.Log("url:" + Main.Instance.GameNetwork.NewUrl + ServiceUrl);
#endif
        HttpLite.HttpRequest<K>(Main.Instance.GameNetwork.NewUrl + ServiceUrl, callback, msg);
    }
}