using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class Mouse : MonoBehaviour
{
    //movement parameters
    public float speed;
    public Vector2 direction;

    private AudioSource audioSource;
    private Child child;
    private Rect activeArea;

    void Start()
    {
        //set up component references
        audioSource = GetComponent<AudioSource>();
        child = FindObjectOfType<Child>();

        activeArea = child.spawnArea;

        //start making noises
        StartCoroutine(NoiseCoroutine());
	}

    void Update()
    {
        //movement
        transform.Translate(speed * new Vector3(direction.x, 0, direction.y) * Time.deltaTime, Space.World);

        if (isOutside())
        {
            child.UnspawnMouse(this.gameObject);
        }
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

    private bool isOutside()
    {
        Vector3 pos = transform.position;

        return (pos.x < activeArea.xMin) || (pos.x > activeArea.xMax) || (pos.z < activeArea.yMin) || (pos.z > activeArea.yMax); 
    }
}
