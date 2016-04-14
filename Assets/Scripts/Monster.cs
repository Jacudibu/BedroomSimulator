using UnityEngine;
using System.Collections;

public class Monster : NetworkObject
{
    public bool spotted = false;
    public bool eating = false;
    public float speed = 5f;

    AudioSource audioSource;

    public AudioClip enterRoomSound;
    public AudioClip heartbeatSound;
    public AudioClip eatSound;

    private SkinnedMeshRenderer rend;
    private Material mat;
    private Animator animator;

    Vector3 lastPos;

    public override void Start ()
    {
        base.Start();

        lastPos = transform.position;

        animator = GetComponentInChildren<Animator>();
        rend = GetComponentInChildren<SkinnedMeshRenderer>();
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

        animator.SetFloat("Speed", (transform.position - lastPos).magnitude);
        lastPos = transform.position;

        if (!hasAuthority || eating)
            return;

        Vector3 movement = Vector3.zero;
        movement += Vector3.right * Input.GetAxis("Horizontal") * Time.deltaTime * speed;
        movement += Vector3.forward * Input.GetAxis("Vertical") * Time.deltaTime * speed;

        if (movement == Vector3.zero)
            return;

        GetComponent<Rigidbody>().MovePosition(transform.position + movement);
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(movement), Time.deltaTime * 5);
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
        yield return SetVisibility(0.4f, 6f);
        yield return SetVisibility(0f, 6f);
    }

    IEnumerator EatSockCoroutine()
    {
        eating = true;
        yield return SetVisibility(1f, 5f);

        audioSource.PlayOneShot(eatSound);
        animator.SetTrigger("Bite");
        yield return new WaitForSeconds(0.8f);

        yield return SetVisibility(0f, 5f);
        eating = false;

    }

    IEnumerator SetVisibility(float targetAlpha, float speed = 3f)
    {
        if (mat == null)
            mat = rend.material;

        Color start = mat.color;
        Color target = start;

        Color startOutline = mat.GetColor("_OutlineColor");
        Color targetOutline = Color.black;

        target.a = targetAlpha;
        targetOutline.a = targetAlpha;

        if (hasAuthority) // are we the monster?
            target.a += 0.5f;

        float progress = 0;

        while (progress < 1)
        {
            mat.color = Color.Lerp(start, target, progress);
            mat.SetColor("_OutlineColor", Color.Lerp(startOutline, targetOutline, progress));
            progress += Time.deltaTime * speed;

            yield return new WaitForEndOfFrame();
        }

        mat.color = target;
        mat.SetColor("_OutlineColor", targetOutline);
    }

    IEnumerator PlayHeartbeats()
    {
        int beatcounter = 0;
        while (true)
        {
            yield return new WaitForSeconds(1f);
            audioSource.PlayOneShot(heartbeatSound);

            beatcounter++;

            if (beatcounter > Child.currentSocks * 1.2f)
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

    public void Spotted()
    {
        spotted = true;

        StopAllCoroutines();

        StartCoroutine(SetVisibility(1f));
        StartCoroutine(SpottedCoroutine());
    }

    IEnumerator SpottedCoroutine()
    {
        float progress = 0f;
        syncRotation = false;
        syncPosition = false;

        eating = true;

        while (progress < 1f)
        {
            transform.Rotate(0f, Time.deltaTime * 1000f * progress, 0f);
            transform.localScale = new Vector3(1f - (progress * progress), 1f, 1f - (progress * progress));

            progress += Time.deltaTime * 0.2f;
            yield return new WaitForEndOfFrame();
        }

        transform.localScale = new Vector3(0f, 1f, 0f);
        GameManager.singleton.GameOver(true);
    }
}
