using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public RawImage[] rawImages; // RawImage들을 배열로 설정합니다.
    public float delay = 1.0f; // 각 RawImage가 켜지는 간격을 설정합니다.

    void Start()
    {
        StartCoroutine(DisplayRawImages());
    }

    IEnumerator DisplayRawImages()
    {
        // 모든 RawImage를 비활성화합니다.
        foreach (RawImage img in rawImages)
        {
            img.gameObject.SetActive(false);
        }

        // 순서대로 RawImage를 켭니다.
        foreach (RawImage img in rawImages)
        {
            img.gameObject.SetActive(true);
            yield return new WaitForSeconds(delay);
        }
    }
}
