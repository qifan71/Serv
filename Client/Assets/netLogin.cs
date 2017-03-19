using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine.UI;
using System.Linq;
public class netLogin : MonoBehaviour
{
    //服务器IP和端口
    public InputField hostInput;
    public InputField portInput;
    //显示客户端收到的消息
    public Text recvText;
    public string recvStr;
    //显示客户端IP和端口
    public Text clientText;
    //聊天输入框
    public InputField textInput;
    //Socket和接收缓冲区
    Socket socket;
    const int BUFFER_SIZE = 1024;
    public byte[] readBuff = new byte[BUFFER_SIZE];
    //沾包分包
    int buffCount = 0;
    byte[] lenBytes = new byte[sizeof(UInt32)];
    Int32 msgLength = 0;
    //协议
    ProtocolBase proto = new ProtocolBytes();  //协议

    public InputField idInput;
    public InputField pwInput;


    //因为只有主线程能够修改UI组件属性
    //因此在Update里更换文本
    void Update()
    {
        recvText.text = recvStr;
    }

    //连接
    public void Connetion()
    {
        //清理text
        recvText.text = "";
        //Socket
        socket = new Socket(AddressFamily.InterNetwork,
                         SocketType.Stream, ProtocolType.Tcp);
        //Connect
        string host = hostInput.text;
        int port = int.Parse(portInput.text);
        socket.Connect(host, port);
        clientText.text = "客户端地址 " + socket.LocalEndPoint.ToString();
        //Recv
        socket.BeginReceive(readBuff, 0, BUFFER_SIZE, SocketFlags.None, ReceiveCb, null);
    }

    //接收回调
    private void ReceiveCb(IAsyncResult ar)
    {
        try
        {
            //count是接收数据的大小
            int count = socket.EndReceive(ar);
            //数据处理
            buffCount += count;
            ProcessData();
            //继续接收	
            socket.BeginReceive(readBuff, 0, BUFFER_SIZE, SocketFlags.None, ReceiveCb, null);
        }
        catch (Exception e)
        {
            recvText.text += "链接已断开";
            socket.Close();
        }
    }


    private void ProcessData()
    {
        //小于长度字节
        if (buffCount < sizeof(Int32))
            return;
        //消息长度
        Array.Copy(readBuff, lenBytes, sizeof(Int32));
        msgLength = BitConverter.ToInt32(lenBytes, 0);
        if (buffCount < msgLength + sizeof(Int32))
            return;
        //处理消息
        ProtocolBase protocol = proto.Decode(readBuff, sizeof(Int32), msgLength);
        HandleMsg(protocol);
        //清除已处理的消息
        int count = buffCount - msgLength - sizeof(Int32);
        Array.Copy(readBuff, msgLength, readBuff, 0, count);
        buffCount = count;
        if (buffCount > 0)
        {
            ProcessData();
        }
    }


    private void HandleMsg(ProtocolBase protoBase)
    {
        ProtocolBytes proto = (ProtocolBytes)protoBase;
        //获取数值
        int start = 0;
        string protoName = proto.GetString(start, ref start);
        int ret = proto.GetInt(start, ref start);
        //显示
        Debug.Log("接收 " + proto.GetDesc());
        recvStr = "接收 " + proto.GetName() + " " + ret.ToString();
    }


    public void OnSendClick()
    {
        ProtocolBytes protocol = new ProtocolBytes();
        protocol.AddString("HeatBeat");
        Debug.Log("发送 " + protocol.GetDesc());
        Send(protocol);
    }

    public void Send(ProtocolBase protocol)
    {
        byte[] bytes = protocol.Encode();
        byte[] length = BitConverter.GetBytes(bytes.Length);
        byte[] sendbuff = length.Concat(bytes).ToArray();
        socket.Send(sendbuff);
    }

    public void OnLoginClick()
    {
        ProtocolBytes protocol = new ProtocolBytes();
        protocol.AddString("Login");
        protocol.AddString(idInput.text);
        protocol.AddString(pwInput.text);
        Debug.Log("发送 " + protocol.GetDesc());
        Send(protocol);
    }

    public void OnAddClick()
    {
        ProtocolBytes protocol = new ProtocolBytes();
        protocol.AddString("AddScore");
        Debug.Log("发送 " + protocol.GetDesc());
        Send(protocol);
    }

    public void OnGetClick()
    {
        ProtocolBytes protocol = new ProtocolBytes();
        protocol.AddString("GetScore");
        Debug.Log("发送 " + protocol.GetDesc());
        Send(protocol);
    }
}


