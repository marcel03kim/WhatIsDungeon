using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rune : MonoBehaviour
{
    public int level;
    public string tag;
    public SlotManager parentSlot;

    private void Start()
    {
        // Renderer 컴포넌트 가져오기
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            // 가장 위에 렌더링하도록 sortingOrder 설정
            renderer.sortingOrder = 9999; 
        }
    }
    public void Init(int level, string tag, SlotManager slot)
    {//아이템 정보값 입력하는 함수
        this.level = level;
        this.tag = tag;
        this.parentSlot = slot;
    }
}
