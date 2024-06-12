using System.Collections.Generic;
using UnityEngine;

public class PreparationManager : MonoBehaviour
{
    public GameObject slotPrefab; // 슬롯 프리팹
    public Transform preparationPanel; // 준비 창 UI 패널 (Grid Layout Group이 적용된)
    private List<GameObject> prepSlots = new List<GameObject>();

    void Start()
    {
        InitializeSlots();
    }

    void InitializeSlots()
    {
        // 준비 창 슬롯 초기화
        foreach (Transform child in preparationPanel)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < 5; i++) // 5개의 슬롯 예시
        {
            GameObject slot = Instantiate(slotPrefab, preparationPanel);
            prepSlots.Add(slot);
        }
    }

    public void AddItemToSlot(GameObject itemPrefab)
    {
        foreach (GameObject slot in prepSlots)
        {
            InventorySlot slotScript = slot.GetComponent<InventorySlot>();
            if (slotScript.GetItem() == null)
            {
                slotScript.SetItem(itemPrefab);
                // Stage Scene에서 버튼 활성화 로직 호출
                StageButtons stageManager = FindObjectOfType<StageButtons>();
                stageManager.ActivateButton(prepSlots.IndexOf(slot), itemPrefab);
                return;
            }
        }
    }

    public void ClearSlots()
    {
        foreach (GameObject slot in prepSlots)
        {
            InventorySlot slotScript = slot.GetComponent<InventorySlot>();
            slotScript.ClearItem();
        }
    }
}
