using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IDropHandler
{
    // Start is called before the first frame update
    public void OnDrop(PointerEventData eventData)
    {
        GameObject droppedRune = eventData.pointerDrag;
        if (droppedRune != null)
        {
            Rune rune = droppedRune.GetComponent<Rune>();
            if (rune != null && transform.childCount == 0)
            {
                droppedRune.transform.SetParent(transform);
                droppedRune.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            }
            else if (rune != null && transform.childCount == 1)
            {
                MergeRunes(rune);
            }
            else
            {
                Debug.Log("슬롯 오류입니다");
            }
        }
    }

    private void MergeRunes(Rune rune)
    {
        string runeTag = rune.runeTag;
        int index = rune.index;

        if (index == 1)
        {
            GameObject higherLevelRunePrefab = runeManager.Instance.GetHigherLevelRune(runeTag, 1);
            if (higherLevelRunePrefab != null)
            {
                Destroy(rune.gameObject);
                GameObject mergedRune = Instantiate(higherLevelRunePrefab, transform);
                mergedRune.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            }
            else
            {
                Debug.LogWarning("Higher level rune prefab not found for tag: " + runeTag + ", index: 2");
            }
        }
        else if (index == 2)
        {
            GameObject higherLevelRunePrefab = runeManager.Instance.GetHigherLevelRune(runeTag, 2);
            if (higherLevelRunePrefab != null)
            {
                Destroy(rune.gameObject);
                GameObject mergedRune = Instantiate(higherLevelRunePrefab, transform);
                mergedRune.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            }
            else
            {
                Debug.LogWarning("Higher level rune prefab not found for tag: " + runeTag + ", index: 3");
            }
        }
    }
}
