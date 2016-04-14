using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class SuperNetworkDiscovery : NetworkDiscovery
{
    public override void OnReceivedBroadcast(string fromAddress, string data)
    {
        Debug.Log(fromAddress);

        NetworkManager.singleton.networkAddress = fromAddress;
        SuperNetworkManager.singleton.networkAddress = fromAddress;
        SuperNetworkManager.singleton.StartClient();
        StopBroadcast();
    }
}
