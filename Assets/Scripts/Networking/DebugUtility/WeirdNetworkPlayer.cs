using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class WeirdNetworkPlayer : NetworkObject
{
    public static NetworkObject localPlayer;

    [Tooltip("Drag children which should only be enabled if the object has local authority here.")]
    public GameObject[] authorityOnlyChildren;

    [Tooltip("Drag children which should only be enabled if the object has no local authority here.")]
    public GameObject[] noAuthorityOnlyChildren;

    [SyncVar] public float lifeMax = 100f;
    [SyncVar] public float lifeCurrent = 100f;

    public GameObject bulletPrefab;

	override public void Start ()
    {
        base.Start();

        if (hasAuthority)
        {
            localPlayer = this;

            foreach (GameObject obj in authorityOnlyChildren)
            {
                obj.SetActive(true);
            }

            foreach (GameObject obj in noAuthorityOnlyChildren)
            {
                obj.SetActive(false);
            }
        }
        else
        {
            foreach (GameObject obj in authorityOnlyChildren)
            {
                obj.SetActive(false);
            }

            foreach (GameObject obj in noAuthorityOnlyChildren)
            {
                obj.SetActive(true);
            }
        }
	}
	
	override public void Update ()
    {
        base.Update();
	}

    [Server] virtual public void TakeDamage(float amount)
    {
        lifeCurrent -= amount;

        if (lifeCurrent <= 0)
            // Do something!
            NetworkServer.Destroy(gameObject);
    }

    [Server] virtual public void Heal(float amount)
    {
        lifeCurrent = Mathf.Clamp(lifeCurrent + amount, 0f, lifeMax);
    }

    [Command] virtual protected void CmdSpawnProjectile(Vector3 position, Quaternion rotation)
    {
        GameObject bullet = (GameObject)Instantiate(bulletPrefab, position, rotation);
        NetworkServer.Spawn(bullet);

        bullet.GetComponent<WeirdProjectile>().RpcInitialize(rotation);

        // If the server is not hosting, it won't execute RPCs.
        if (!SuperNetworkManager.isClient)
        {
            bullet.GetComponent<WeirdProjectile>().LocalInitialize(rotation);
        }
    }
}
