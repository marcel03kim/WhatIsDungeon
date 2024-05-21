using UnityEngine;
using UnityEngine.EventSystems;

public class SlotManager : MonoBehaviour, IDropHandler
{
    public static SlotManager Instance;

    public Transform[] slots;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public Transform GetEmptySlot()
    {
        foreach (Transform slot in slots)
        {
            if (slot.childCount == 0)
            {
                return slot;
            }
        }
        return null;
    }
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
                Debug.Log("���� �����Դϴ�");
            }
        }
    }

    private void MergeRunes(Rune rune)
    {
        string runeTag = rune.runeTag;
        int index = rune.index;

        if (index == 1)
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
                Debug.LogWarning("Higher level rune prefab not found for tag: " + runeTag + ", index: 2");
            }
        }
        else if (index == 2)
        {
            GameObject higherLevelRunePrefab = runeManager.Instance.GetHigherLevelRune(runeTag, 3);
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