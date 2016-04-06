using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class NetworkObject : NetworkBehaviour
{
    public bool syncPosition;
    public bool syncRotation;
    public bool syncRigidbody;

    [SyncVar]
    public int ownerConnectionID = -42;

    [Tooltip("The Speed at which non-authority Objects lerp to their Networking targetPosition.")]
    [Range(0f, 1f)]
    [SerializeField]
    private float networkLerpSpeed = 0.33f;

    [Tooltip("Amount of Updates per Seconds. \nSet this to 0 if you only need one Update after creation.\n[Not working for Rigidbodys since these are event-driven]")]
    [Range(0, 60)]
    [SerializeField]
    private int networkUpdatesPerSecond = 20;

    private float networkUpdateYieldValue;

    [SyncVar]
    private Vector3 targetPosition;

    [SyncVar]
    private Quaternion targetRotation;

    protected Rigidbody rb;

    private bool updateRigidbody;

    [SyncVar]
    private Vector3 targetVelocity;

    [SyncVar]
    private Vector3 targetAngularVelocity;

    virtual public void Awake()
    {
        rb = GetComponent<Rigidbody>();

    }

    virtual public void Start()
    {
        if (networkUpdatesPerSecond != 0)
            networkUpdateYieldValue = 1f / (float)networkUpdatesPerSecond;

        if (!hasAuthority)
            return;

        CmdSetOwnerConnectionID(SuperNetworkManager.connectionID);
        NetworkEventManager.OnConnectionIDChanged += SetOwnerConnectionID;

        if (syncPosition || syncRotation || syncRigidbody)
            StartCoroutine(SyncValues());
    }

    // ----------------------------------------
    // -- Basic Functions that may be Useful --
    // ----------------------------------------

    void SetOwnerConnectionID()
    { 
        if (SuperNetworkManager.isConnected)
            CmdSetOwnerConnectionID(SuperNetworkManager.connectionID);
    }

    [Command] void CmdSetOwnerConnectionID(int id)
    {
        ownerConnectionID = id;
    }

    // -------------------------------
    // -- Synchronization Functions --
    // -------------------------------

    virtual public void Update()
    {
        // Synch data with stuff thats on the server.
        if (!hasAuthority)
        {
            // Lerp to target Position & Rotation.
            if (syncPosition)
            {
                transform.position = Vector3.Lerp(transform.position, targetPosition, networkLerpSpeed);
            }

            if (syncRotation)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, networkLerpSpeed);
            }
        }
    }

    virtual public void FixedUpdate()
    {
        if (!hasAuthority)
        {
            if (updateRigidbody)
            {
                rb.velocity = targetVelocity;
                rb.angularVelocity = targetAngularVelocity;
                updateRigidbody = false;
            }
        }
    }

    private IEnumerator SyncValues()
    {
        if (networkUpdatesPerSecond == 0)
        {
            // Only update once.
            if (syncPosition)
                CmdSyncPosition(transform.position);
            if (syncRotation)
                CmdSyncRotation(transform.rotation);
            if (syncRigidbody)
                CmdSyncRigidbody(rb.velocity, rb.angularVelocity);

            yield return new WaitForEndOfFrame();
        }
        else
        {
            while (true)
            {
                if (syncPosition)
                    CmdSyncPosition(transform.position);
                if (syncRotation)
                    CmdSyncRotation(transform.rotation);
                if (syncRigidbody)
                    CmdSyncRigidbody(rb.velocity, rb.angularVelocity);

                yield return new WaitForSeconds(networkUpdateYieldValue);
            }
        }
    }
    
    [ClientRpc] private void RpcApplyRigidbodyVelocities()
    {
        updateRigidbody = true;
    }

    [Command] private void CmdSyncPosition(Vector3 target)
    {
        targetPosition = target;
    }

    [Command] private void CmdSyncRotation(Quaternion target)
    {
        targetRotation = target;
    }

    [Command] private void CmdSyncRigidbody(Vector3 velocity, Vector3 angularVelocity)
    {
        targetVelocity = velocity;
        targetAngularVelocity = angularVelocity;

        RpcApplyRigidbodyVelocities();

        if (!isClient) // RPCs will only be called on clients, so make sure we sync it on Server-Only Mode anyways.
        {
            updateRigidbody = true;
        }
    }
}
