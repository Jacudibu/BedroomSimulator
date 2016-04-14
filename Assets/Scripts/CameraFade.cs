using UnityEngine;
using System.Collections;

[RequireComponent(typeof(GUITexture))]
public class CameraFade : MonoBehaviour
{
    public float fadeSpeed = 1.5f;
    bool running = false;

    private GUITexture texture;
    private float progress = 0f;

    public static CameraFade instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        texture = GetComponent<GUITexture>();
        texture.pixelInset = new Rect(0f, 0f, Screen.width, Screen.height);
    }

    void Update()
    {
        // #TODO: Remove these debug calls
        if (Input.GetKeyDown(KeyCode.I))
            FadeToClear();

        if (Input.GetKeyDown(KeyCode.O))
            FadeToBlack();
    }

    IEnumerator FadeFromTo(Color start, Color end)
    {
        running = true;

        progress = 0f;
        while (progress < 1f)
        {
            texture.color = Color.Lerp(start, end, progress);
            progress += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        texture.color = end;

        running = false;
    }

    public void FadeToClear()
    {
        if (!running)
            StartCoroutine(FadeFromTo(texture.color, Color.clear));
        else
            Debug.LogWarning("ScreenFade wasn't finished!");
    }


    public void FadeToBlack()
    {
        if (!running)
            StartCoroutine(FadeFromTo(texture.color, Color.black));
        else
            Debug.LogWarning("ScreenFade wasn't finished!");
    }

    public void FadeTo(Color color)
    {
        if (!running)
            StartCoroutine(FadeFromTo(texture.color, color));
        else
            Debug.LogWarning("ScreenFade wasn't finished!");
    }

    public void FadeAndSendMessageAfterwards(Color targetColor, string methodName, MonoBehaviour messageTarget)
    {
        if (!running)
            StartCoroutine(FadeAndSendMessageAfterwardsCoroutine(targetColor, methodName, messageTarget));
        else
            Debug.LogWarning("ScreenFade wasn't finished!");
    }

    IEnumerator FadeAndSendMessageAfterwardsCoroutine(Color targetColor, string methodName, MonoBehaviour messageTarget)
    {
        yield return FadeFromTo(texture.color, targetColor);

        if (messageTarget != null)
            messageTarget.SendMessage(methodName);
    }
}