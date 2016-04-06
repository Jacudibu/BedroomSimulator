using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Child : NetworkObject
{
    public float height = 1.5f;
    public GameObject sockPrefab;
    
    int totalSocks = 20;

	override public void Start ()
    {
        base.Start(); 

        if (!hasAuthority)
        {
            GetComponent<Camera>().enabled = false;
            GetComponent<AudioListener>().enabled = false;
        }
        else
        {
            // Spawn Socks
            for (int i = 0; i < totalSocks; i++)
            {
                Vector3 pos = new Vector3();
                pos.x = Random.Range(-9f, 9f);
                pos.y = 0.3f;
                pos.z = Random.Range(-9f, 9f);
                
                GameObject sock = (GameObject)GameObject.Instantiate(sockPrefab, pos, Random.rotation);
                NetworkServer.Spawn(sock);
            }
        }
    }

    override public void Update ()
    {
        base.Update();

        //if (!hasAuthority)
        //    return;
       
        // Check if we are looking on the bad guy!
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 100f))
        {
            if (hit.collider.CompareTag("Player"))
            {
                Debug.Log("I know what you did last summer!");
            }
        }
	}

    [ClientRpc] public void RpcSockWasSnatched()
    {
        Debug.Log("I cry everytime");
        totalSocks--;

        if (totalSocks <= 2)
        {
            Debug.Log("Q_Q");
        }
    }
}
