using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rune : MonoBehaviour
{
    public int level;
    public new string tag;
    public SlotManager parentSlot;

    public void Init(int level, string tag, SlotManager slot)
    {//������ ������ �Է��ϴ� �Լ�
        this.level = level;
        this.tag = tag;
        this.parentSlot = slot;
    }
}
