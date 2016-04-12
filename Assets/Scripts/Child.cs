using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Child : NetworkObject
{
    public float height = 1.5f;
    public GameObject sockPrefab;

    int totalSocks = 20;
    public static int currentSocks = 20;

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
                pos.x = Random.Range(-8f, 8f);
                pos.y = 0.1f;
                pos.z = Random.Range(-8f, 8f);
                
                GameObject sock = (GameObject)GameObject.Instantiate(sockPrefab, pos, Quaternion.Euler(0f, Random.Range(0f, 360f), 0f));
                NetworkServer.Spawn(sock);
            }
        }

        currentSocks = totalSocks;
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
        currentSocks--;
        FindObjectOfType<Monster>().EatSock();

        if (currentSocks <= 2)
        {
            // #TODO:
            Debug.Log("Q_Q");

            if (currentSocks == 1)
                GameManager.singleton.GameOver(false);
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
