using UnityEngine;
using System.Collections;

public class ConeTrigger : MonoBehaviour
{
    float timer = 0f;
    bool visible = false;

    void Update()
    {
        if (visible)
            timer += Time.deltaTime;
        else
            timer -= Time.deltaTime;

        timer = Mathf.Clamp01(timer);
    }

    void TriggerDetected(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Monster kevin = other.GetComponent<Monster>();
            visible = kevin.eating;

            if (timer > 0.5f && !kevin.spotted)
                kevin.Spotted();
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

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            visible = false;
    }
}
