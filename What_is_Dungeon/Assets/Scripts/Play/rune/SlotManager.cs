using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotManager : MonoBehaviour
{
    public enum SLOTSTATE       //���Ի��°�
    {
        EMPTY,
        FULL
    }

    public int id;                              //���� ��ȣ ID
    public Rune runeObject;                     //������ Ŀ���� Class ID
    public SLOTSTATE state = SLOTSTATE.EMPTY;   //Enum �� ����

    private void ChangeStateTo(SLOTSTATE targetState)
    {//�ش� ������ ���°��� ��ȯ �����ִ� �Լ�
        state = targetState;
    }

    public void runeGrabbed()
    {//RayCast�� ���ؼ� �������� ����� ��
        Destroy(runeObject.gameObject);         //���� �������� ����
        ChangeStateTo(SLOTSTATE.EMPTY);         //������ �� ����

    }   
    
    public void Createrune(int id, string tag)
    {
        
        //������ ��δ� (Assets/Resources/Prefabs/rune_000)
        // Resoueces.Load(path) path = "Prefabs/rune_000" �̷������� �ۼ��ؾ���.
        string runePath = "Prefabs/rune_" + tag + id.ToString("000");
                
        //var runeGo = (GameObject)Instantiate(Resources.Load(runePath));
        // �� ������ ���ҽ� �ε� �� Object Ÿ������ ��ȯ�ϱ� ������ GameObject ������ Null Ref. Exception �߻���.
        var runeGo = (GameObject)Instantiate(Resources.Load<GameObject>(runePath));

        runeGo.transform.SetParent(this.transform);
        runeGo.transform.localPosition = Vector3.zero;
        runeGo.transform.localScale = Vector3.one;
        //������ �� ������ �Է�
        runeObject = runeGo.GetComponent<Rune>();
        runeObject.Init(id, tag, this); //�Լ��� ���� �� �Է�(this -> Slot Class)

        ChangeStateTo(SLOTSTATE.FULL);

    }
}
