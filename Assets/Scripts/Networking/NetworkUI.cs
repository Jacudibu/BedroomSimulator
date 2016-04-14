using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NetworkUI : MonoBehaviour
{
    public bool useBroadcast;
    SuperNetworkDiscovery networkDiscovery;

    [SerializeField] InputField ipInput;

    void Start()
    {
        ipInput.onEndEdit.AddListener(SetIP);

        ipInput.gameObject.SetActive(!useBroadcast);
    }

    void OnLevelWasLoaded(int id)
    {
        // On return to main menu, stop broadcasting
        if (id == 0 && useBroadcast)
        {
            networkDiscovery = FindObjectOfType<SuperNetworkDiscovery>();
            if (networkDiscovery.running)
                networkDiscovery.StopBroadcast();
        }
    }

	// --------------------------
    // -- Button Functionality --
    // --------------------------

    public void SetIP(string ip)
    {
        SuperNetworkManager.singleton.networkAddress = ip;
    }

    public void StartServer()
    {
        SuperNetworkManager.singleton.StartServer();
    }

    public void StartHost()
    {
        SuperNetworkManager.singleton.StartHost();

        if (networkDiscovery == null)
        {
            networkDiscovery = FindObjectOfType<SuperNetworkDiscovery>();
        }

        networkDiscovery.Initialize();
        networkDiscovery.StartAsServer();
    }

    public void StartClient()
    {
        if (!useBroadcast)
            SuperNetworkManager.singleton.StartClient();
        else
        {
            if (networkDiscovery == null)
            {
                networkDiscovery = FindObjectOfType<SuperNetworkDiscovery>();
            }
            networkDiscovery.Initialize();
            networkDiscovery.StartAsClient();
        }
    }

    public void Disconnect()
    {
        if (SuperNetworkManager.isConnected)
        {
            if (SuperNetworkManager.isHost)
                SuperNetworkManager.singleton.StopHost();
            else if (SuperNetworkManager.isClient)
                SuperNetworkManager.singleton.StopClient();
            else if (SuperNetworkManager.isServer)
                SuperNetworkManager.singleton.StopServer();
        }
    }

    public void FadeAndStartHost()
    {
        CameraFade.instance.FadeAndSendMessageAfterwards(Color.black, "StartHost", this);
        StartCoroutine(FadeOutButtons());
    }

    public void FadeAndStartClient()
    {
        CameraFade.instance.FadeAndSendMessageAfterwards(Color.black, "StartClient", this);
        StartCoroutine(FadeOutButtons());
    }

    // Thats super inefficient, but... since 
    IEnumerator FadeOutButtons()
    {
        float progress = 0f;

        // First of all, disable buttos!
        foreach (Button button in GetComponentsInChildren<Button>())
        {
            button.interactable = false;
        }

        // Now just lerp their color's alpha to 0.
        while (progress < 1f)
        {
            foreach (Image img in GetComponentsInChildren<Image>())
            {
                img.color = Color.Lerp(img.color, new Color(img.color.r, img.color.g, img.color.b, 0f), progress);
            }

            foreach (Text txt in GetComponentsInChildren<Text>())
            {
                txt.color = Color.Lerp(txt.color, new Color(txt.color.r, txt.color.g, txt.color.b, 0f), progress);
            }

            progress += Time.deltaTime * 3f;
            yield return new WaitForEndOfFrame();
        }
    }
}
