using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RegPanel : PanelBase
{
    private InputField idInput;
    private InputField pwInput;
    private Button closeBtn;
    private Button regBtn;
    #region[生命周期]
    public override void Init(params object[] args)
    {
        base.Init(args);
        skinPath = "RegPanel";
        layer = PanelLayer.Panel;
    }
    public override void OnShowing()
    {
        base.OnShowing();
        Transform skinTrans = skin.transform;
        idInput = skinTrans.FindChild("IDInput").GetComponent<InputField>();
        pwInput = skinTrans.FindChild("PWInput").GetComponent<InputField>();
        closeBtn = skinTrans.FindChild("CloseBtn").GetComponent<Button>();
        regBtn = skinTrans.FindChild("RegBtn").GetComponent<Button>();

        closeBtn.onClick.AddListener(OnCloseClick);
        regBtn.onClick.AddListener(OnRegClick);
    }
    #endregion

    public void OnRegClick()
    {
        //用户名 密码为空
        if (idInput.text == "" || pwInput.text == "")
        {
            Debug.Log("用户名 密码不能为空");
            return;
        }
        //如果尚未连接,则发起连接
        if (NetMgr.srvConn.status != Connection.Status.Connected)
        {
            string host = "127.0.0.1";
            int port = 1234;
            NetMgr.srvConn.proto = new ProtocolBytes();
            NetMgr.srvConn.Connect(host, port);
        }
        //发送
        ProtocolBytes protocol = new ProtocolBytes();
        protocol.AddString("Register");
        protocol.AddString(idInput.text);
        protocol.AddString(pwInput.text);
        Debug.Log("发送: " + protocol.GetDesc());
        //发送Login协议,并注册OnLoginBack
        NetMgr.srvConn.Send(protocol, OnLoginBack);
    }

    public void OnLoginBack(ProtocolBase protocol)
    {
        ProtocolBytes proto = (ProtocolBytes)protocol;
        int start = 0;
        string proName = proto.GetString(start, ref start);
        int ret = proto.GetInt(start, ref start);
        if (ret == 0)
        {
            Debug.Log("注册成功");
            PanelMgr.instance.OpenPanel<LoginPanel>("");
            Close();
        }
        else
        {
            Debug.Log("注册失败");
        }
    }

    public void OnCloseClick()
    {
        PanelMgr.instance.OpenPanel<LoginPanel>("");
        Close();
    }
}
