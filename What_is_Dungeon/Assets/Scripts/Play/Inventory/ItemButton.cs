using UnityEngine;
using UnityEngine.UI;

public class ItemButton : MonoBehaviour
{
    public Image itemImage; // 아이템 이미지를 표시할 UI
    private GameObject itemPrefab; // 아이템 프리팹 참조
    private Inventory inventory; // 인벤토리 참조

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
        Destroy(gameObject); // 인벤토리 버튼 제거
    }
}
