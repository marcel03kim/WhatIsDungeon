using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public GameObject itemButtonPrefab; // 아이템 버튼 프리팹
    public Transform inventoryPanel; // 인벤토리 UI 패널 (Grid Layout Group이 적용된)

    private List<GameObject> items = new List<GameObject>(); // 인벤토리에 있는 아이템 리스트

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
        // 인벤토리 UI 갱신 로직 추가 필요
    }
}
