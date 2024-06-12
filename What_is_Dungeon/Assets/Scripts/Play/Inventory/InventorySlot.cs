using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    private GameObject currentItem; // 현재 슬롯에 있는 아이템
    public Image itemImage; // 슬롯에 표시될 이미지

    public void SetItem(GameObject itemPrefab)
    {
        currentItem = itemPrefab;
        itemImage.sprite = itemPrefab.GetComponent<SpriteRenderer>().sprite;
    }

    public GameObject GetItem()
    {
        return currentItem;
    }

    public void ClearItem()
    {
        currentItem = null;
        itemImage.sprite = null;
    }
}
