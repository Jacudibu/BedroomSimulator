using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public GameObject MonsterCamera;

    void Start ()
    {
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

    void OnDestroy()
    {
        UnityEngine.VR.VRSettings.enabled = false;
        UnityEngine.VR.VRSettings.loadedDevice = UnityEngine.VR.VRDeviceType.None;
    }
}
