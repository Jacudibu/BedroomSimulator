using UnityEngine;
using System.Collections;

public class SetMainCamera : MonoBehaviour
{
	void Start ()
    {
        // Currently we just disable all other cameras
        if(GetComponent<NetworkObject>()!=null)
        if (!GetComponent<NetworkObject>().hasAuthority)
            GetComponentInChildren<Camera>().gameObject.SetActive(false);
	}
}
