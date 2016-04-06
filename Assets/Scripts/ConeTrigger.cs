using UnityEngine;
using System.Collections;

public class ConeTrigger : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // #TODO:
            Debug.Log("You start seeing ... things!");
        }
    }
}
