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
        // Renderer ������Ʈ ��������
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            // ���� ���� �������ϵ��� sortingOrder ����
            renderer.sortingOrder = 9999; 
        }
    }
    public void Init(int level, string tag, SlotManager slot)
    {//������ ������ �Է��ϴ� �Լ�
        this.level = level;
        this.tag = tag;
        this.parentSlot = slot;
    }
}
