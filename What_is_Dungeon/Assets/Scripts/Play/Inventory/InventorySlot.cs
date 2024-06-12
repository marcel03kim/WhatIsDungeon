using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    private GameObject currentItem; // ���� ���Կ� �ִ� ������
    public Image itemImage; // ���Կ� ǥ�õ� �̹���

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
