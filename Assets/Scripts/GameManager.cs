using UnityEngine;
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
        MonsterCamera.SetActive(false);
        UnityEngine.VR.VRSettings.enabled = true;
        UnityEngine.VR.VRSettings.loadedDevice = UnityEngine.VR.VRDeviceType.Oculus;     
    }

    void InitClient()
    {
        MonsterCamera.SetActive(true);
        // Disable all "Main Cameras", the only main cam is the player with Oculus
        foreach (GameObject camObject in GameObject.FindGameObjectsWithTag("MainCamera"))
            camObject.GetComponent<Camera>().enabled = false;
    }

    void OnDestroy()
    {
        UnityEngine.VR.VRSettings.enabled = false;
        UnityEngine.VR.VRSettings.loadedDevice = UnityEngine.VR.VRDeviceType.None;
    }
}
