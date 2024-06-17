using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public RawImage[] rawImages; // RawImage���� �迭�� �����մϴ�.
    public float delay = 1.0f; // �� RawImage�� ������ ������ �����մϴ�.

    void Start()
    {
        StartCoroutine(DisplayRawImages());
    }

    IEnumerator DisplayRawImages()
    {
        // ��� RawImage�� ��Ȱ��ȭ�մϴ�.
        foreach (RawImage img in rawImages)
        {
            img.gameObject.SetActive(false);
        }

        // ������� RawImage�� �մϴ�.
        foreach (RawImage img in rawImages)
        {
            img.gameObject.SetActive(true);
            yield return new WaitForSeconds(delay);
        }
    }
}
