//using UnityEngine;
//using System.Collections;

//public class SocketServer : MonoBehaviour {

//    // Use this for initialization
//    void Start () {
	
//    }
	
//    // Update is called once per frame
//    void Update () {
	
//    }
//}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Runtime.InteropServices;


namespace SocketServer
{
    class Program
    {
        static bool s_flag = true;

        static void Main(string[] args)
        {
            int port = 10001;

            IPAddress ip = new IPAddress(new Byte[] { 127, 0, 0, 1 });
            TcpListener listener = new TcpListener(ip, port);
            listener.Start();
            Console.WriteLine("服务器侦听启动");

            while (true)
            {
                TcpClient remoteClient = listener.AcceptTcpClient();
                Thread thread = new Thread(SocketThread);
                thread.IsBackground = true;
                thread.Start(remoteClient);
            }
            
            //Console.ReadKey();
            s_flag = false;
            //Console.WriteLine("退出所有线程");
            //Console.ReadKey();
        }

        static void SocketThread(Object para)
        {
            int bufferSize = 8792;

            TcpClient remoteClient = (TcpClient)para;

            Console.WriteLine("客户端已接入! local:{0}<------Client:{1}"
                    , remoteClient.Client.LocalEndPoint, remoteClient.Client.RemoteEndPoint);

            NetworkStream streamToClient = remoteClient.GetStream();

            while (s_flag)
            {
                try
                {
                    byte[] buffer = new byte[bufferSize];
                    int byteRead = streamToClient.Read(buffer, 0, bufferSize);

                    if (byteRead == 0)
                    {
                        //Console.WriteLine("客户端连接断开...");
                        //break;
                    }
                    else
                    {
                        string msg = Encoding.Unicode.GetString(buffer, 0, byteRead);
                        Console.WriteLine("接收数据：{0} From：[{1}]", msg, remoteClient.Client.RemoteEndPoint);

                        if(msg == "hello world!!!")
                        {
                            s_flag = false;
                        }

                        Send(remoteClient);

                    }

                    
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                    break;
                }
            }

            Console.WriteLine("关闭连接");
            remoteClient.Close();
        }

        static void Send(TcpClient client)
        {
            if (client != null)
            {
                string msg = "hello world!!!";
                NetworkStream streamToServer = client.GetStream();
                byte[] buffer = Encoding.Unicode.GetBytes(msg);
                streamToServer.Write(buffer, 0, buffer.Length);
                Console.WriteLine("发出消息 : " + msg);
            }
        }


    }

    
}
