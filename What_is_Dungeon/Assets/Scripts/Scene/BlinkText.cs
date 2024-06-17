using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BlinkText : MonoBehaviour
{
    Text flashingText;
    void Start()
    {
        flashingText = GetComponent<Text>();
        StartCoroutine(FadeText());
    }

    public IEnumerator FadeText()
    {
        while (true)
        {
            // Fade out
            for (float alpha = 1f; alpha >= 0f; alpha -= 0.01f)
            {
                SetAlpha(alpha);
                yield return new WaitForSeconds(0.01f);
            }

            // Fade in
            for (float alpha = 0f; alpha <= 1f; alpha += 0.01f)
            {
                SetAlpha(alpha);
                yield return new WaitForSeconds(0.01f);
            }
        }
    }

    void SetAlpha(float alpha)
    {
        if (flashingText != null)
        {
            Color color = flashingText.color;
            color.a = alpha;
            flashingText.color = color;
        }
    }
}
