using UnityEngine;
using UnityEngine.UI;

public class StageButtons : MonoBehaviour
{
    public Button[] stageButtons; // �������� ��ư��

    void Start()
    {
        foreach (Button button in stageButtons)
        {
            button.interactable = false; // �⺻������ ��Ȱ��ȭ
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
        // ������ ��� ����
        // ������ ��� �� ����
        Inventory inventory = FindObjectOfType<Inventory>();
        inventory.RemoveItem(item);
        stageButtons[slotIndex].interactable = false;
    }
}
