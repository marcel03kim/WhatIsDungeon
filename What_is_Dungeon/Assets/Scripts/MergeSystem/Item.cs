using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public int id;
    public MergeSlot parentSlot;

    public void Init(int id, MergeSlot slot)
    {//������ ������ �Է��ϴ� �Լ�
        this.id = id;
        this.parentSlot = slot;
    }
}
