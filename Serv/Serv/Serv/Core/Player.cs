using System;

public class Player
{
    public string id;
    //稍后实现连接类
    public Conn conn;
    //数据
    public PlayerData data;
    //临时数据
    public PlayerTempData tempData;
    //构造函数,给id赫尔conn赋值
    public Player(string id,Conn conn)
    {
        this.id = id;
        this.conn = conn;
        tempData = new PlayerTempData();
    }
    //发送
    public void Send(ProtocolBase proto)
    {
        if (proto == null)
            return;
        ServNet.instance.Send(conn,proto);
    }
    //踢下线
    public static bool KickOff(string id,ProtocolBase proto)
    {
        Conn[] conns = ServNet.instance.conns;
        //遍历连接池,找到要踢下线的玩家
        for (int i = 0; i < conns.Length; i++)
        {
            if (conns[i] == null)
                continue;
            if (!conns[i].isUse)
                continue;
            if (conns[i].player == null)
                continue;
            if (conns[i].player.id==id)
            {
                lock (conns[i].player)
                {
                    if (proto!=null)
                    {
                        conns[i].player.Send(proto);
                    }
                    //下线并保存数据
                    return conns[i].player.Logout();
                }
            }
        }
        return true;
    }
    //下线
    public bool Logout()
    {
        //事件处理(稍后实现)
        ServNet.instance.handlePlayerEvent.OnLogout(this);
        //保存
        if (!DataMgr.instance.Saveplayer(this))
        {
            return false;
        }
        //下线
        conn.player = null;
        conn.Close();
        return true;
    }
}

