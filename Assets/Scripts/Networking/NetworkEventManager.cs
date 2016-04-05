using UnityEngine;
using System.Collections;

public class NetworkEventManager : MonoBehaviour
{
    public delegate void ConnectionIDChanged();
    public static event ConnectionIDChanged OnConnectionIDChanged;

    public static void FireConnectionIDChanged() { if (OnConnectionIDChanged != null) OnConnectionIDChanged(); }
}
