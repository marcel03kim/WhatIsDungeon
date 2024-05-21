using UnityEngine;
using UnityEngine.EventSystems;

public class Rune : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public int index; // 룬 인덱스
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

        // 룬을 맨 위로 이동하여 드래그 중에 가장 위에 렌더링되도록 함
        transform.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // 드래그를 끝내면 슬롯에 들어갈 수 있는지 확인
        GameObject hoveredSlot = eventData.pointerEnter;
        if (hoveredSlot != null && hoveredSlot.GetComponent<SlotManager>() != null)
        {
            // 슬롯에 룬을 드롭
            transform.SetParent(hoveredSlot.transform);
            rectTransform.anchoredPosition = Vector2.zero;
        }
        else
        {
            // 슬롯 범위를 벗어나면 원래 자리로 돌아가기
            rectTransform.anchoredPosition = originalPosition;
            transform.SetParent(originalParent);
        }
        canvasGroup.blocksRaycasts = true;
    }
}