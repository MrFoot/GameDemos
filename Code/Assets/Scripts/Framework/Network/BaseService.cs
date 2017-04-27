using UnityEngine;
using System.Collections;
using System.IO;
using System;
using ServiceDat;

public class BaseService<T> : Singleton<T> where T : new()
{

	protected string ServiceUrl;

	public virtual void Init() {

	}

    protected void SendData<K>(K msg, soulgame.network.HttpDoneCallback callback, bool encrypt = false) where K : ProtoBuf.IExtensible
	{
        //这里的memorystream，能否考虑重复使用，并预分配内存？这样在频繁调用的情况下，就不会不断的重复分配内存了。
        MemoryStream data_ms = new MemoryStream();
        ProtoBuf.Serializer.Serialize<K>(data_ms, msg);

        NetworkManager.Instance.Send(ServiceUrl, callback, data_ms.ToArray());
	}

    protected void SendData(soulgame.network.HttpDoneCallback callback, bool encrypt = false)
    {
        NetworkManager.Instance.Send(ServiceUrl, callback, null);
    }
}
