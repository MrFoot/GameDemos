using UnityEngine;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System;
using System.Text;
using System.Threading;

public class SocketTest : MonoBehaviour {

    int port = 10001;
    Socket m_client1;
    Socket m_client2;
    Thread recvThread;
    bool flag = true;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyUp(KeyCode.Alpha1))
        {
            m_client1 = CreateClient();
            recvThread = new Thread(Recv);
            recvThread.IsBackground = true;
            recvThread.Start(m_client1);
        }
        else if (Input.GetKeyUp(KeyCode.Alpha2))
        {
            m_client2 = CreateClient();
        }
        else if (Input.GetKeyUp(KeyCode.Q))
        {
            Send(m_client1);
        }
        else if (Input.GetKeyUp(KeyCode.W))
        {
            Send(m_client2);
        }
	}

    Socket CreateClient()
    {
        Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        try
        {
            IPAddress ip = IPAddress.Parse("127.0.0.1");
            IPEndPoint point = new IPEndPoint(ip,port);

            socket.Connect(point);
        }
        catch (Exception e)
        {
            Debug.Log("客户端连接异常 : " + e.Message);
        }
        Debug.Log("LocalEndPoint = " + socket.LocalEndPoint + ".RemoteEndPoint = " + socket.RemoteEndPoint);

        return socket;
    }

    void Send(Socket client)
    {
        if (client != null)
        {
            string msg = "hello world!!!";

            byte[] buffer = Encoding.Unicode.GetBytes(msg);

            client.Send(buffer);

            Debug.Log("发出消息 : " + msg);
        }
    }

    void Recv(System.Object para)
    {
        int bufferSize = 8792;

        Socket remoteClient = (Socket)para;

        Debug.Log(string.Format("客户端已接入! local:{0}<------Client:{1}"
                , remoteClient.LocalEndPoint, remoteClient.RemoteEndPoint));

        while (remoteClient.Connected)
        {
            try
            {
                byte[] buffer = new byte[bufferSize];
                int byteRead = remoteClient.Receive(buffer);

                if (byteRead == 0)
                {
                    
                    //break;
                }
                else
                {
                    string msg = Encoding.Unicode.GetString(buffer, 0, byteRead);
                    Debug.Log(string.Format("接收数据：{0} From：[{1}]", msg, remoteClient.RemoteEndPoint));
                }

                    
            }
            catch(Exception e)
            {
                Debug.Log(e.Message);
                break;
            }
        }

        Debug.Log(string.Format("连接断开..."));
        remoteClient.Close();
    }

    void OnApplicationQuit()
    {
        Debug.Log("OnAppQuit");
        flag = false;
    }

}
