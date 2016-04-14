using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Child : NetworkObject
{
    public float height = 1.5f;
    public GameObject sockPrefab;

    int totalSocks = 20;
    public static int currentSocks = 20;

    //mouse spawning
    public GameObject mousePrefab;
    public Rect spawnArea;

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
                do
                {
                    pos.x = Random.Range(-8f, 8f);
                    pos.y = 0f;
                    pos.z = Random.Range(-8f, 8f);
                } while (Physics.CheckSphere(pos, 0.5f, LayerMask.GetMask("HierNixSpawnen")));

                GameObject sock = (GameObject)GameObject.Instantiate(sockPrefab, pos, Quaternion.Euler(0f, Random.Range(0f, 360f), 0f));
                sock.GetComponentInChildren<Renderer>().material.color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1f);

                NetworkServer.Spawn(sock);
            }

            //Play snoring sound effect
            GetComponent<AudioSource>().Play();
        }

        currentSocks = totalSocks;
    }

    override public void Update ()
    {
        base.Update();

        if (!hasAuthority)
            return;

        if (!UnityEngine.VR.VRDevice.isPresent)
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
        //Stop playing snoring sound effect (but finish playing the last loop)
        GetComponent<AudioSource>().loop = false;

        StartCoroutine(SetFlashlight(true, 1.5f));

        StartCoroutine(SpawnMice());//start spawning mice at intervals
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

    IEnumerator SpawnMice()
    {
        yield return new WaitForSeconds(10.0f);

        GameObject spawnedMouse = null;

        while (hasAuthority)
        {
            float nextDelay = Random.Range(30.0f, 60.0f);

            if (spawnedMouse != null)
            {
                //delete the mouse
                NetworkServer.UnSpawn(spawnedMouse);
                GameObject.Destroy(spawnedMouse);
            }

            spawnedMouse = spawnMouse();

            yield return new WaitForSeconds(nextDelay);
        }
    }

    private GameObject spawnMouse()
    {
        if (!hasAuthority) return null;

        int side = Random.Range(0, 4);//which side of the room to spawn the mouse at
        float length = Random.value;//how far along the length of that side to spawn

        Vector2 spawnPos = Vector2.zero;
        Vector2 dir = Vector2.zero;
        float z_angle = 0;

        switch (side)
        {
            //left
            case 0:
                {
                    spawnPos.x = spawnArea.xMin;
                    spawnPos.y = spawnArea.height * length;

                    dir = new Vector2(1, 0);
                    z_angle = 90;
                    break;
                }
            //right
            case 1:
                {
                    spawnPos.x = spawnArea.xMax;
                    spawnPos.y = spawnArea.height * length;

                    dir = new Vector2(-1, 0);
                    z_angle = 270;
                    break;
                }
            //top
            case 2:
                {
                    spawnPos.x = spawnArea.width * length;
                    spawnPos.y = spawnArea.yMax;

                    dir = new Vector2(0, -1);
                    z_angle = 180;
                    break;
                }
            //bottom
            case 3:
                {
                    spawnPos.x = spawnArea.width * length;
                    spawnPos.y = spawnArea.yMin;

                    dir = new Vector2(0, 1);
                    z_angle = 0;
                    break;
                }
        }


        //actually spawn
        GameObject mouse = (GameObject)GameObject.Instantiate(mousePrefab, new Vector3(spawnPos.x, 0, spawnPos.y), Quaternion.identity);
        mouse.GetComponent<Mouse>().direction = dir;
        mouse.transform.eulerAngles = new Vector3(mouse.transform.eulerAngles.x, mouse.transform.eulerAngles.y, z_angle);

        NetworkServer.Spawn(mouse);

        return mouse;
    }
}
