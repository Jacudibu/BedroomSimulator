using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class WeirdProjectile : NetworkBehaviour
{
    public float damage = 10f;
    public float initialVelocity = 1000f;

    // This will be called on all connected clients.
    [ClientRpc] public void RpcInitialize(Quaternion rotation)
    {
        transform.rotation = rotation;
        GetComponent<Rigidbody>().AddForce(transform.forward * initialVelocity);
    }

    // This should only be called if the Server is not hosting (so isClient should be false)
    [Server] public void LocalInitialize(Quaternion rotation)
    {
        transform.rotation = rotation;
        GetComponent<Rigidbody>().AddForce(transform.forward * initialVelocity);
    }

    [Server] void OnCollisionEnter(Collision collision)
    {
        NetworkServer.Destroy(gameObject);

    //    if (collision.gameObject.GetComponent<NetworkPlayer>() != null)
   //         collision.gameObject.GetComponent<NetworkPlayer>().TakeDamage(damage);
    }
}
