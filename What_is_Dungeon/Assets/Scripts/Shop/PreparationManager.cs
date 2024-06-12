using System.Collections.Generic;
using UnityEngine;

public class PreparationManager : MonoBehaviour
{
    public GameObject slotPrefab; // ���� ������
    public Transform preparationPanel; // �غ� â UI �г� (Grid Layout Group�� �����)
    private List<GameObject> prepSlots = new List<GameObject>();

    void Start()
    {
        InitializeSlots();
    }

    void InitializeSlots()
    {
        // �غ� â ���� �ʱ�ȭ
        foreach (Transform child in preparationPanel)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < 5; i++) // 5���� ���� ����
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
                // Stage Scene���� ��ư Ȱ��ȭ ���� ȣ��
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
