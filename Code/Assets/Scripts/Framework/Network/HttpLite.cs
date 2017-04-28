using UnityEngine;
using System.Collections;
using System;
using System.Net;
using System.Collections.Generic;
using System.Text;
using System.Threading;

public delegate void HttpDoneCallback(byte[] data, HttpProc.HttpState state);


///<summary>
///function：start a thread to send http request and recieve response.
///author：chenxiang
///date：2016-10-17
///</summary> 
public class HttpProc
{
	private WebClient client = new WebClient();
	
	private HttpState m_myHttpState = HttpState.E_HTTP_STATE_IDLE;
	
	private HttpStateCB m_httpStateCB = null;
	private string m_myUrl = null;
	
    private byte[] m_httpData;

	private byte[] mPostData;
	
	public enum HttpState : int
	{
		E_HTTP_STATE_IDLE,
		E_HTTP_STATE_DOWNLOADING,
		E_HTTP_STATE_ERROR,
		E_HTTP_STATE_DOWNLOAD_FINISHED,
		E_HTTP_STATE_MAX
	}

    public byte[] getHttpData()
    {
        return m_httpData;
    }

	
	public interface HttpStateCB
	{
		void httpStateNotify(HttpState state);
	}
	
	private void httpDownloadProc()
	{
		m_myHttpState = HttpState.E_HTTP_STATE_DOWNLOADING;
		if (m_httpStateCB != null)
		{
			m_httpStateCB.httpStateNotify(HttpState.E_HTTP_STATE_DOWNLOADING);
		}
		
		try
		{
            client.Encoding = System.Text.Encoding.UTF8;
               
            m_httpData =  client.DownloadData(m_myUrl);
            
			if (m_httpStateCB != null)
			{
				m_httpStateCB.httpStateNotify(HttpState.E_HTTP_STATE_DOWNLOAD_FINISHED);
			}
		}
		catch (Exception e)
		{
			#if DHTRACE	
			Debug.Log ("http responcse error!!!!!");
			#endif
			m_myHttpState = HttpState.E_HTTP_STATE_ERROR;
			if (m_httpStateCB != null)
			{
				m_httpStateCB.httpStateNotify(HttpState.E_HTTP_STATE_ERROR);
			}
		}
	}
	
	private void httpPostProc()
	{
		m_myHttpState = HttpState.E_HTTP_STATE_DOWNLOADING;
		if (m_httpStateCB != null)
		{
			m_httpStateCB.httpStateNotify(HttpState.E_HTTP_STATE_DOWNLOADING);
		}
		
		try
		{
            client.Encoding = System.Text.Encoding.UTF8;

            m_httpData = client.UploadData(m_myUrl, mPostData);
            
			if (m_httpStateCB != null)
			{
				m_httpStateCB.httpStateNotify(HttpState.E_HTTP_STATE_DOWNLOAD_FINISHED);
			}
		}
		catch (Exception e)
		{
			#if UNITY_EDITOR
			Debug.Log ("http responcse error!!!!! Exception : \n" + e.ToString());
			#endif
			m_myHttpState = HttpState.E_HTTP_STATE_ERROR;
			if (m_httpStateCB != null)
			{
				m_httpStateCB.httpStateNotify(HttpState.E_HTTP_STATE_ERROR);
			}
		}
	}
	
	public void httpRequest(string url, HttpStateCB cb)
	{
		if (m_myHttpState != HttpState.E_HTTP_STATE_DOWNLOADING)
		{
			m_myUrl = url;
			m_httpStateCB = cb;
			
			ThreadStart threadStart = new ThreadStart(httpDownloadProc);
			Thread thread = new Thread(threadStart);
			thread.Start();
		}
	}
	
	public void httpPostRequest(string url,  HttpStateCB cb, byte[] postData)
	{
		if (m_myHttpState != HttpState.E_HTTP_STATE_DOWNLOADING)
		{
			m_myUrl = url;
			m_httpStateCB = cb;
			
			mPostData = postData;
			
			ThreadStart threadStart = new ThreadStart(httpPostProc);
			Thread thread = new Thread(threadStart);
			thread.Start();
		}
	}
}

///<summary>
///function：maintain a list of HttpProc, monitor their status. provide interfaces to request http server.
///</summary> 
public static class HttpLite
{
	public class HttpInterface : HttpProc.HttpStateCB
	{
		public bool m_removed = false;
		public string URL = null;
		public int requestID = 0;
		private HttpDoneCallback m_httpCallback = null;
		public HttpDoneCallback HttpCallBack
		{
			set
			{
				m_httpCallback = value;
			}
			get
			{
				return m_httpCallback;
			}
		}
        private byte[] m_httpData;

		private HttpProc m_http = new HttpProc();
		public HttpProc.HttpState m_httpState= HttpProc.HttpState.E_HTTP_STATE_IDLE;
		public void httpStateNotify(HttpProc.HttpState state)
		{
			if(state == HttpProc.HttpState.E_HTTP_STATE_DOWNLOAD_FINISHED)
			{
				m_httpData = m_http.getHttpData();
			}
			m_httpState = state;
		}
		
		public void finish()
		{
			m_httpCallback(m_httpData, m_httpState);
		}
		
		public void failed()
		{
			m_httpCallback(null, m_httpState);
		}
		
		public void request(string url, byte[] postData)
		{
			URL = url;

            if (postData != null && postData.Length > 0)
            {
                m_http.httpPostRequest(url, this, postData);
            }
            else
            {
                m_http.httpRequest(url, this);
            }
		}
	}

	private static List<HttpInterface> m_https = new List<HttpInterface>();
	private static List<HttpInterface> m_httpAddList = new List<HttpInterface>();
	private static List<HttpInterface> m_httpRemoveList = new List<HttpInterface>();
	private static int m_httpRequestNum = 0;

	public static void Init()
	{
		GameMain.RegisterUpdate(httpCheck);
	}

	public static void HttpRequest(string url, HttpDoneCallback callback)
	{
		HttpRequest(url, callback, null);
	}

    public static void HttpRequest<T>(string url, HttpDoneCallback callback, T instance)
    {
        byte[] msg = Protocol.ProtoBufSerialize<T>(instance);

        HttpRequest(url, callback, msg);
    }

	public static void HttpRequest(string url, HttpDoneCallback callback, byte[] postData)
	{
		#if UNITY_EDITOR
		Debug.Log("httpRequest, url=" + url + ", postData=" + postData);
		#endif

        for (int i = 0, max = m_httpAddList.Count; i < max; ++i)
        {
            if (m_httpAddList[i].HttpCallBack == callback && m_httpAddList[i].URL.Equals(url))
            {
#if UNITY_EDITOR
                Debug.LogWarning("same httpRequest have been requested!, url=" + url);
#endif
                return;
            }
        }

        for (int i = 0, max = m_https.Count; i < max; ++i)
        {
            if (m_https[i].HttpCallBack == callback && m_https[i].URL.Equals(url))
            {
#if UNITY_EDITOR
                Debug.LogWarning("same httpRequest have been requested!, url=" + url);
#endif
                return;
            }
        }

		m_httpRequestNum++;
		HttpInterface Ihttp = new HttpInterface();
		Ihttp.HttpCallBack = callback;
		Ihttp.requestID = m_httpRequestNum;
		m_httpAddList.Add(Ihttp);
		Ihttp.request(url, postData);
	}

	private static void httpCheck(float deltaTime)
	{
		for(int i = 0, max = m_httpAddList.Count; i < max; ++i)
		{
			m_https.Add(m_httpAddList[i]);
		}
		m_httpAddList.Clear();
		
		for(int i = 0, max = m_https.Count; i < max; ++i)
		{
			HttpInterface http = m_https[i];
			if(http.m_removed)
			{
				m_httpRemoveList.Add(http);
			}
			else if(http.m_httpState == HttpProc.HttpState.E_HTTP_STATE_DOWNLOAD_FINISHED)
			{
				http.finish();
				m_httpRemoveList.Add(http);
			}
			else if(http.m_httpState == HttpProc.HttpState.E_HTTP_STATE_ERROR)
			{
				http.failed();
				m_httpRemoveList.Add(http);
			}
		}
		
		for(int i = 0, max = m_httpRemoveList.Count; i < max; ++i)
		{
			m_https.Remove (m_httpRemoveList[i]);
			m_httpRequestNum--;
		}
		m_httpRemoveList.Clear();
	}
	
	public static void ClearHttpRequest()
	{
		#if UNITY_EDITOR
		Debug.Log(string.Format("http request num = {0}", m_httpRequestNum));
		#endif

		foreach(HttpInterface http in m_https)
		{
			http.m_removed = true;
		}
		
		m_httpRequestNum = 0;
	}

	public static void CancelHttpRequest(HttpDoneCallback callback)
	{
		for(int i = 0, max = m_httpAddList.Count; i < max; ++i)
		{
			if(m_httpAddList[i].HttpCallBack == callback)
			{
				m_httpAddList[i].m_removed = true;

			}
		}
		
		for(int i = 0, max = m_https.Count; i < max; ++i)
		{
			if(m_https[i].HttpCallBack == callback)
			{
				m_https[i].m_removed = true;
			}
		}
	}

	public static void CancelHttpRequest(string url, HttpDoneCallback callback)
	{
		for(int i = 0, max = m_httpAddList.Count; i < max; ++i)
		{
			if(m_httpAddList[i].HttpCallBack == callback && m_httpAddList[i].URL == url)
			{
				m_httpAddList[i].m_removed = true;
				
			}
		}
		
		for(int i = 0, max = m_https.Count; i < max; ++i)
		{
			if(m_https[i].HttpCallBack == callback && m_https[i].URL == url)
			{
				m_https[i].m_removed = true;
			}
		}
	}
}
