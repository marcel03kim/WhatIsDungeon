using UnityEngine;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{
    public GameObject itemPrefab; // 아이템 프리팹
    public int itemCost; // 아이템 가격
    public Button purchaseButton; // 구매 버튼

    private Inventory inventory; // 인벤토리 참조

    void Start()
    {
        inventory = FindObjectOfType<Inventory>();
        purchaseButton.onClick.AddListener(BuyItem);
    }

    void BuyItem()
    {
        if (CoinManager.instance.coin >= itemCost)
        {
            CoinManager.instance.coin -= itemCost;
            inventory.AddItem(itemPrefab);
        }
    }
}
