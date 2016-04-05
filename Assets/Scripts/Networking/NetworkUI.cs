using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NetworkUI : MonoBehaviour
{

    [SerializeField] InputField ipInput;

    void Start()
    {
        ipInput.onEndEdit.AddListener(SetIP);
    }

	// --------------------------
    // -- Button Functionality --
    // --------------------------

    public void SetIP(string ip)
    {
        SuperNetworkManager.singleton.networkAddress = ip;
    }

    public void StartServer()
    {
        SuperNetworkManager.singleton.StartServer();
    }

    public void StartHost()
    {
        SuperNetworkManager.singleton.StartHost();
    }

    public void StartClient()
    {
        SuperNetworkManager.singleton.StartClient();
    }

    public void Disconnect()
    {
        if (SuperNetworkManager.isConnected)
        {
            if (SuperNetworkManager.isHost)
                SuperNetworkManager.singleton.StopHost();
            else if (SuperNetworkManager.isClient)
                SuperNetworkManager.singleton.StopClient();
            else if (SuperNetworkManager.isServer)
                SuperNetworkManager.singleton.StopServer();
        }
    }
}
