using UnityEngine;
using UnityEngine.UI;

public class StageButtons : MonoBehaviour
{
    public Button[] stageButtons; // 스테이지 버튼들

    void Start()
    {
        foreach (Button button in stageButtons)
        {
            button.interactable = false; // 기본적으로 비활성화
        }
    }

    public void ActivateButton(int slotIndex, GameObject item)
    {
        if (slotIndex >= 0 && slotIndex < stageButtons.Length)
        {
            stageButtons[slotIndex].interactable = true;
            stageButtons[slotIndex].onClick.AddListener(() => UseItem(item, slotIndex));
        }
    }

    void UseItem(GameObject item, int slotIndex)
    {
        // 아이템 사용 로직
        // 아이템 사용 후 제거
        Inventory inventory = FindObjectOfType<Inventory>();
        inventory.RemoveItem(item);
        stageButtons[slotIndex].interactable = false;
    }
}
