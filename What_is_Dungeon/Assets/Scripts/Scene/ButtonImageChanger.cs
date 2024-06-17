using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonImageChanger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image buttonImage; // 버튼의 이미지를 참조
    public Sprite normalSprite; // 기본 상태의 이미지
    public Sprite hoverSprite; // 마우스가 올려졌을 때의 이미지
    public Sprite clickSprite; // 클릭 했을 때 이미지

    // 마우스가 버튼 위로 들어왔을 때 호출
    public void OnPointerEnter(PointerEventData eventData)
    {
        buttonImage.sprite = hoverSprite;
    }

    // 마우스가 버튼에서 나갔을 때 호출
    public void OnPointerExit(PointerEventData eventData)
    {
        buttonImage.sprite = normalSprite;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        buttonImage.sprite = clickSprite;
    }
}
