using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class SuperNetworkManager : NetworkManager
{
    private static SuperNetworkManager _singleton;
    private static bool _isServer;
    private static bool _isClient;
    private static bool _isConnected;
    private static int _connectionID;
    private static NetworkClient _networkClient;

    private bool didInit;

    // --------------------
    // -- Static Getters --
    // --------------------

    new public static SuperNetworkManager singleton { get { if (_singleton == null) _singleton = GameObject.FindObjectOfType<SuperNetworkManager>();  return _singleton; } }

    public static bool isServer { get { return _isServer; } }
    public static bool isHost { get { return _isServer && _isClient; } }
    public static bool isClient { get { return _isClient; } }
    public static bool isConnected { get { return _isConnected; } }
    public static int connectionID { get { return _connectionID; } }
    public static NetworkClient networkClient { get { return _networkClient; } }

    [SerializeField] GameObject prefab_player_monster;
    [SerializeField] GameObject prefab_player_child;

    // ---------------
    // -- Singleton --
    // ---------------

    // ----------------------
    // -- Network Messages --
    // ----------------------

    void OnReceiveConnectionID(NetworkMessage msg)
    {
        _connectionID = msg.ReadMessage<MessageWithInt>().value;
        NetworkEventManager.FireConnectionIDChanged();
    }

    // ----------------------
    // -- Client Functions --
    // ----------------------

    public override void OnStartClient(NetworkClient client)
    {
        base.OnStartClient(client);

        client.RegisterHandler((short)NetworkMessageType.personalConnectionID, OnReceiveConnectionID);

        _networkClient = client;

        _isClient = true;
    }

    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);

        _isConnected = true;
    }

    public override void OnClientDisconnect(NetworkConnection conn)
    {
        base.OnClientDisconnect(conn);

        _isConnected = false;
    }

    public override void OnStopClient()
    {
        base.OnStopClient();

        _isClient = false;
    }

    public override void OnClientError(NetworkConnection conn, int errorCode)
    {
        base.OnClientError(conn, errorCode);

        // TODO: Handle errorCodes.
    }

    // ----------------------
    // -- Server Functions --
    // ----------------------

    public override void OnStartServer()
    {
        base.OnStartServer();

        _isServer = true;
        _isConnected = true;


    }

    public override void OnStopServer()
    {
        base.OnStopServer();

        _isServer = false;
        _isConnected = false;
    }

    public override void OnStartHost()
    {
        base.OnStartHost();

        _isServer = true;
        _isClient = true;
        _isConnected = true;

    }

    public override void OnStopHost()
    {
        base.OnStopHost();

        _isServer = false;
        _isClient = false;
        _isConnected = false;
    }

    public override void OnServerConnect(NetworkConnection conn)
    {
        base.OnServerConnect(conn);


        // Send the connectionID to the Client.
        MessageWithInt msg = new MessageWithInt();
        msg.value = conn.connectionId;
        NetworkServer.SendToClient(conn.connectionId, (short)NetworkMessageType.personalConnectionID, msg);

        if (!didInit) // Start Host calls this twice, but we only want to spawn something once!
            StartCoroutine(SpawnPlayerObject(conn));
    }

    IEnumerator SpawnPlayerObject(NetworkConnection conn)
    {
        didInit = true;
        yield return new WaitForSeconds(0.2f);

        GameObject player;
        if (conn.connectionId == connectionID)
            player = (GameObject)GameObject.Instantiate(prefab_player_child);
        else
        {
            Vector3 pos = new Vector3();
            pos.x = Random.Range(-9f, 9f);
            pos.y = prefab_player_monster.transform.position.y;
            pos.z = Random.Range(-9f, 9f);

            player = (GameObject)GameObject.Instantiate(prefab_player_monster, pos, prefab_player_monster.transform.rotation);
        }

        NetworkServer.SpawnWithClientAuthority(player, conn);

        yield return new WaitForSeconds(0.2f);
        didInit = false;
    }

    public override void OnServerDisconnect(NetworkConnection conn)
    {
        base.OnServerDisconnect(conn);
    }

    public override void OnServerError(NetworkConnection conn, int errorCode)
    {
        base.OnServerError(conn, errorCode);

        // TODO: Handle errorCodes.
    }
}
