using UnityEngine;
using UnityEngine.UI;


public class runeManager : MonoBehaviour
{
    public static runeManager Instance; //ΩÃ±€≈Ê


    [System.Serializable]
    public struct RunePrefab
    {
        public string runeTag;
        public int index;
        public GameObject prefab;
    }

    public RunePrefab[] runePrefabs;

    private void Awake()    //ΩÃ±€≈Ê
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
    
    

    public GameObject GetRunePrefab(string runeTag, int index)
    {
        foreach (RunePrefab runePrefab in runePrefabs)
        {
            if (runePrefab.index == index && runePrefab.runeTag == runeTag)
            {
                return runePrefab.prefab;
            }
        }
        return null;
    }

    public GameObject CreateRune(string runeTag, int index, Transform parent)
    {
        GameObject runePrefab = GetRunePrefab(runeTag, index);
        if (runePrefab != null)
        {
            GameObject newRune = Instantiate(runePrefab, parent);
            newRune.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            return newRune;
        }
        else
        {
            Debug.LogWarning("Rune prefab not found for tag: " + runeTag + ", index: " + index);
            return null;
        }
    }

    public void buyRune(string runeTag)
    {
        Transform emptySlot = SlotManager.Instance.GetEmptySlot();
        if (emptySlot != null)
        {
            CreateRune(runeTag, 1, emptySlot);
        }
        else
        {
            Debug.LogWarning("No empty slot available");
        }
    }

    public GameObject GetHigherLevelRune(string runeTag, int index)
    {
        return GetRunePrefab(runeTag, index + 1);
    }
}