using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class Mouse : MonoBehaviour
{
    //movement parameters
    public float speed;
    public Vector2 direction;

    AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        //start making noises
        StartCoroutine("NoiseCoroutine");
	}

    void Update()
    {
        //movement
        transform.Translate(speed * new Vector3(direction.x, 0, direction.y) * Time.deltaTime, Space.World);
	}

    IEnumerator NoiseCoroutine()
    {
        //makes a noise at randomized intervals
        while (true)
        {
            float nextDelay = Random.Range(1.5f, 3.5f);

            audioSource.Play();

            yield return new WaitForSeconds(nextDelay);
        }
    }
}
