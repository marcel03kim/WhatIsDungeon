using UnityEngine;
using UnityEngine.EventSystems;

public class SlotManager : MonoBehaviour
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
    
}