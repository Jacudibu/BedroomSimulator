using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public GameObject MonsterCamera;

    public static GameManager singleton;

    void Start ()
    {
        singleton = this;

	    if (SuperNetworkManager.isServer)
        {
            InitHost();
        }
        else
        {
            InitClient();
        }
        
	}

    void InitHost()
    {

    }

    void InitClient()   
    {
        MonsterCamera.SetActive(true);
    }

    public void GameOver(bool childHasWon)
    {
        if (SuperNetworkManager.isServer)
            SuperNetworkManager.singleton.StopHost();
        else
            SuperNetworkManager.singleton.StopClient();
    }

    void OnDestroy()
    {
        UnityEngine.VR.VRSettings.enabled = false;
        UnityEngine.VR.VRSettings.loadedDevice = UnityEngine.VR.VRDeviceType.None;
    }
}
