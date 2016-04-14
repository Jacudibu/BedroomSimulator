using UnityEngine;
using System.Collections;

public class Ending : MonoBehaviour
{
    IEnumerator Start()
    {
        yield return new WaitForSeconds(5f);

        if (SuperNetworkManager.isHost)
            SuperNetworkManager.singleton.StopHost();
        else
            SuperNetworkManager.singleton.StopClient();
    }
}
