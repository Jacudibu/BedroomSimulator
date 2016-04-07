using UnityEngine;
using System.Collections;

public class ConeTrigger : MonoBehaviour
{
    void TriggerDetected(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Monster kevin = other.GetComponent<Monster>();
            if (kevin.eating)
            {
                Debug.Log("You start seeing ... things!");
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        TriggerDetected(other);
    }

    void OnTriggerStay(Collider other)
    {
        TriggerDetected(other);
    }
}
