using UnityEngine;
using UnityEngine.UI;

public class ItemButton : MonoBehaviour
{
    public Image itemImage; // ������ �̹����� ǥ���� UI
    private GameObject itemPrefab; // ������ ������ ����
    private Inventory inventory; // �κ��丮 ����

    public void Initialize(GameObject itemPrefab, Inventory inventory)
    {
        this.itemPrefab = itemPrefab;
        this.inventory = inventory;
        itemImage.sprite = itemPrefab.GetComponent<SpriteRenderer>().sprite;
        GetComponent<Button>().onClick.AddListener(OnItemClicked);
    }

    void OnItemClicked()
    {
        PreparationManager prepManager = FindObjectOfType<PreparationManager>();
        prepManager.AddItemToSlot(itemPrefab);
        inventory.RemoveItem(itemPrefab);
        Destroy(gameObject); // �κ��丮 ��ư ����
    }
}
