using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonImageChanger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image buttonImage; // ��ư�� �̹����� ����
    public Sprite normalSprite; // �⺻ ������ �̹���
    public Sprite hoverSprite; // ���콺�� �÷����� ���� �̹���
    public Sprite clickSprite; // Ŭ�� ���� �� �̹���

    // ���콺�� ��ư ���� ������ �� ȣ��
    public void OnPointerEnter(PointerEventData eventData)
    {
        buttonImage.sprite = hoverSprite;
    }

    // ���콺�� ��ư���� ������ �� ȣ��
    public void OnPointerExit(PointerEventData eventData)
    {
        buttonImage.sprite = normalSprite;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        buttonImage.sprite = clickSprite;
    }
}
