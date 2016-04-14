using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Sock : MonoBehaviour
{
	void Start ()
    {
        GetComponentInChildren<Renderer>().material.color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1f);
    }

    void OnTriggerEnter(Collider other)
    {
        if (!SuperNetworkManager.isServer)
            return; // Thou shall not pass!

        if (other.CompareTag("Player"))
        {
            // Tell the game the sock was snatched and remove it
            FindObjectOfType<Child>().RpcSockWasSnatched();
            NetworkServer.Destroy(this.gameObject);
        }
    }
}
