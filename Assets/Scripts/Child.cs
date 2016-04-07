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

        if (!hasAuthority)
            return;

        transform.Rotate(Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0f);
	}

    [ClientRpc] public void RpcSockWasSnatched()
    {
        // #TODO:
        Debug.Log("I cry everytime");
        totalSocks--;

        if (totalSocks <= 2)
        {
            // #TODO:
            Debug.Log("Q_Q");

            if (totalSocks == 0)
                GameManager.singleton.GameOver();
        }
    }

    public void StartGame()
    {
        StartCoroutine(SetFlashlight(true, 1.5f));
    }

    IEnumerator SetFlashlight(bool status, float delay)
    {
        yield return new WaitForSeconds(delay);

        // #TODO: Play flashlight Sound here

        foreach (Light light in GetComponentsInChildren<Light>())
        {
            light.enabled = status;
        }
    }
}
