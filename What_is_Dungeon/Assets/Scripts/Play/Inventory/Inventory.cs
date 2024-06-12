using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public GameObject itemButtonPrefab; // ������ ��ư ������
    public Transform inventoryPanel; // �κ��丮 UI �г� (Grid Layout Group�� �����)

    private List<GameObject> items = new List<GameObject>(); // �κ��丮�� �ִ� ������ ����Ʈ

    public void AddItem(GameObject itemPrefab)
    {
        items.Add(itemPrefab);
        CreateInventoryButton(itemPrefab);
    }

    void CreateInventoryButton(GameObject itemPrefab)
    {
        GameObject newItemButton = Instantiate(itemButtonPrefab, inventoryPanel);
        newItemButton.GetComponent<ItemButton>().Initialize(itemPrefab, this);
    }

    public void RemoveItem(GameObject itemPrefab)
    {
        items.Remove(itemPrefab);
        // �κ��丮 UI ���� ���� �߰� �ʿ�
    }
}
