using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuneManager : MonoBehaviour
{//���� ���� ���� ���� ������ �ִ� Ŭ����
    public int slotId;      //���� ��ȣ (Slot Ŭ���� ���� ��)
    public int runeLevel;      //������ ��ȣ
    public string runeTag;


    public void InitDummy(int slotId, int runeLevel, string runeTag)
    {//�μ��� ���� ������ Class�ʿ� �Է�
        this.slotId = slotId;
        this.runeLevel = runeLevel;
        this.runeTag = runeTag;
    }
}
