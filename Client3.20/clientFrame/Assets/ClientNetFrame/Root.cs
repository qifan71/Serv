using UnityEngine;
using System.Collections;

public class Root : MonoBehaviour 
{

	// Use this for initialization
	void Start ()
    {
        PanelMgr.instance.OpenPanel<LoginPanel>("");
        Application.runInBackground = true;
	}
	
	// Update is called once per frame
	void Update () 
    {
        NetMgr.Update();
	}
}
