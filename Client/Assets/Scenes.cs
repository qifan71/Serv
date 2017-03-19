using UnityEngine;
using System.Collections;

public class Scenes : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    void OnGUI()
    {
        //4服务端基础测试
        if (GUI.Button(new Rect(0, Screen.height - 50, 100, 50), "4ServNet"))
        {
            Application.LoadLevel("4ServNet");
        }
        //7协议
        if (GUI.Button(new Rect(100, Screen.height - 50, 100, 50), "7proto"))
        {
            Application.LoadLevel("7proto");
        }
        //9登录
        if (GUI.Button(new Rect(200, Screen.height - 50, 100, 50), "9login"))
        {
            Application.LoadLevel("9login");
        }
    }
}
