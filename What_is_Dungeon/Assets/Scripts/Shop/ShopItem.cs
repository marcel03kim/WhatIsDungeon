using UnityEngine;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{
    public GameObject itemPrefab; // ������ ������
    public int itemCost; // ������ ����
    public Button purchaseButton; // ���� ��ư

    private Inventory inventory; // �κ��丮 ����

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
