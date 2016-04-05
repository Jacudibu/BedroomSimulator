using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class CloseAllWindows : NetworkBehaviour
{
    public KeyCode key = KeyCode.Escape;
	
	void Update ()
    {
        if (hasAuthority && Input.GetKeyDown(key))
        {
            CmdCloseAll();
        }
	}

    [ClientRpc] void RpcCloseAll()
    {
        Application.Quit();
    }

    [Command] void CmdCloseAll()
    {
        RpcCloseAll();
    }
}
