using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;
 
public class TcpComm : MonoBehaviour
{
 
    public string ipaddress = "127.0.0.1";
    public int port = 8000;
    private Socket clientSocket;
    public InputField MessageInput;
    private Thread thread;
    private byte[] data=new byte[1024];// 数据容器
    private string message = "";
    public string receivedString = "";
	void Start () {
        ConnectToServer();
        print("Tcp started!");
	}
	
	void Update () {
        //只有在主线程才能更新UI
	  if (message!="" && message!=null)
	   {
           receivedString += message;
	        message = "";
	   }
	}
    /**
     * 连接服务器端函数
     * */
    void ConnectToServer()
    {
        clientSocket=new Socket(AddressFamily.InterNetwork,SocketType.Stream,ProtocolType.Tcp);
        //跟服务器连接
        clientSocket.Connect(new IPEndPoint(IPAddress.Parse(ipaddress),port));
        //客户端开启线程接收数据
        thread = new Thread(ReceiveMessage);
        thread.Start();
 
    }
 
    void ReceiveMessage()
    {
        while (true)
        {
            if (clientSocket.Connected == false)
            {
                break;
            }
        int length=clientSocket.Receive(data);
        message = Encoding.UTF8.GetString(data,0,length);
         print(message);
        }
     
    }
 
    public void SendMessageTcp(string message)
    {
        print("Send start.");
        byte[] data=Encoding.UTF8.GetBytes(message);
        clientSocket.Send(data);
    }
 
    public void OnSendButtonClick()
    {
        string value = MessageInput.text;
        SendMessage(value);
        MessageInput.text = " ";
    }
    /**
     * unity自带方法
     * 停止运行时会执行
     * */
     void OnDestroy()
    {
        //关闭连接，分接收功能和发送功能，both为两者均关闭
       clientSocket.Shutdown(SocketShutdown.Both);
       clientSocket.Close();
    }
}