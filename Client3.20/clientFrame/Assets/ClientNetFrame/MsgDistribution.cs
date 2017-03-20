using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
//消息分发
public class MsgDistribution
{
    //每帧处理消息的数量
    public int num = 15;
    //消息列表
    public List<ProtocolBase> msgList = new List<ProtocolBase>();
    //委托类型
    public delegate void Delegate(ProtocolBase proto);
    //时间监听表
    private Dictionary<string, Delegate> eventDic = new Dictionary<string, Delegate>();
    private Dictionary<string, Delegate> onceDic = new Dictionary<string, Delegate>();

    public void Update()
    {
        for (int i = 0; i < num; i++)
        {
            if (msgList.Count > 0)
            {
                DispatchMsgEvent(msgList[0]);
                lock (msgList)
                {
                    msgList.RemoveAt(0);
                }
            }
            else
            {
                break;
            }
        }
    }
    //消息分发
    public void DispatchMsgEvent(ProtocolBase protocol)
    {
        string name = protocol.GetName();
        Debug.Log("分发消息: " + name);
        if (eventDic.ContainsKey(name))
        {
            eventDic[name](protocol);
        }
        if (onceDic.ContainsKey(name))
        {
            onceDic[name](protocol);
            onceDic[name] = null;
            onceDic.Remove(name);
        }
    }
    //添加监听事件
    public void AddListener(string name,Delegate cb)
    {
        if (eventDic.ContainsKey(name))
            eventDic[name] += cb;
        else
            eventDic[name] = cb;
    }
    //添加单次监听事件
    public void AddOnceListener(string name,Delegate cb)
    {
        if (onceDic.ContainsKey(name))
            onceDic[name] += cb;
        else
            onceDic[name] = cb;

    }

    //删除监听事件
    public void DelListener(string name, Delegate cb)
    {
        if (eventDic.ContainsKey(name))
        {
            eventDic[name] -= cb;
            if (eventDic[name]==null)
                eventDic.Remove(name);
        }      
    }
    //删除单次监听事件
    public void DelOnceListener(string name, Delegate cb)
    {
        if (onceDic.ContainsKey(name))
        {
            onceDic[name] -= cb;
            if (onceDic[name] == null)
                onceDic.Remove(name);
        }      
    }
}
