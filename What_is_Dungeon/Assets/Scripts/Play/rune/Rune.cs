using UnityEngine;
using UnityEngine.EventSystems;

public class Rune : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public int index; // �� �ε���
    public string runeTag;

    private CanvasGroup canvasGroup;
    private RectTransform rectTransform;
    private Vector2 originalPosition;
    private Transform originalParent;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        rectTransform = GetComponent<RectTransform>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalPosition = rectTransform.anchoredPosition;
        originalParent = transform.parent;
        canvasGroup.blocksRaycasts = false;

        // ���� �� ���� �̵��Ͽ� �巡�� �߿� ���� ���� �������ǵ��� ��
        transform.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // �巡�׸� ������ ���Կ� �� �� �ִ��� Ȯ��
        GameObject hoveredSlot = eventData.pointerEnter;
        if (hoveredSlot != null && hoveredSlot.GetComponent<SlotManager>() != null)
        {
            // ���Կ� ���� ���
            transform.SetParent(hoveredSlot.transform);
            rectTransform.anchoredPosition = Vector2.zero;
        }
        else
        {
            // ���� ������ ����� ���� �ڸ��� ���ư���
            rectTransform.anchoredPosition = originalPosition;
            transform.SetParent(originalParent);
        }
        canvasGroup.blocksRaycasts = true;
    }
}