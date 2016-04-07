using UnityEngine;
using System.Collections;

public class Monster : NetworkObject
{
    public bool eating = false;
    public float speed = 5f;

    AudioSource audioSource;

    public AudioClip enterRoomSound;
    public AudioClip heartbeatSound;
    public AudioClip eatSound;

    private MeshRenderer rend;
    private Material mat;

    public override void Start ()
    {
        base.Start();

        rend = GetComponent<MeshRenderer>();
        mat = rend.material;

        audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(enterRoomSound);

        // Tell the child it's about time to wake up
        StartCoroutine(WakeChildUp());
        StartCoroutine(SetVisibility(0f));
        StartCoroutine(PlayHeartbeats());
    }
	
	public override void Update ()
    {
        base.Update();

        if (!hasAuthority || eating)
            return;

        transform.Translate(Vector3.right * Input.GetAxis("Horizontal") * Time.deltaTime * speed);
        transform.Translate(Vector3.forward * Input.GetAxis("Vertical") * Time.deltaTime * speed);
	}

    public void EatSock()
    {
        StartCoroutine(EatSockCoroutine());
    }

    public void Flash()
    {
        if (!eating)
            StartCoroutine(FlashCoroutine());
    }

    IEnumerator FlashCoroutine()
    {
        yield return SetVisibility(0.4f, 10f);
        yield return SetVisibility(0f, 10f);
    }

    IEnumerator EatSockCoroutine()
    {
        eating = true;
        yield return SetVisibility(1f);

        audioSource.PlayOneShot(eatSound);
        // #TODO: Play eating Animation

        yield return SetVisibility(0f);
        eating = false;

    }

    IEnumerator SetVisibility(float targetAlpha, float speed = 3f)
    {
        Color start = mat.color;
        Color target = start;
        target.a = targetAlpha;

        if (hasAuthority) // are we the monster?
            target.a += 0.5f;

        float progress = 0;

        while (progress < 1)
        {
            mat.color = Color.Lerp(start, target, progress);
            progress += Time.deltaTime * speed;

            yield return new WaitForEndOfFrame();
        }

        mat.color = target;
    }

    IEnumerator PlayHeartbeats()
    {
        int beatcounter = 0;
        while (true)
        {
            yield return new WaitForSeconds(1f);
            audioSource.PlayOneShot(heartbeatSound);

            beatcounter++;

            if (beatcounter > Child.currentSocks * 1.5f)
            {
                Flash();
                beatcounter = 0;
            }
        }
    }

    IEnumerator WakeChildUp()
    {
        yield return new WaitForSeconds(2f);
        CameraFade.instance.FadeAndSendMessageAfterwards(Color.clear, "StartGame", FindObjectOfType<Child>());
    }
}
