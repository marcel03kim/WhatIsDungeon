using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot : MonoBehaviour
{
    public enum MANASLOTSTATE  // ���� ���°�
    {
        EMPTY,
        FULL
    }

    public int id;  // ���� ��ȣ ID
    public MANASLOTSTATE state = MANASLOTSTATE.EMPTY;

    // ���� ���°��� ��ȯ �����ִ� �Լ�
    public void ChangeStateTo(MANASLOTSTATE targetState)
    {
        state = targetState;
    }

    public void CreateMana()
    {
        // ������ ��δ� (Assets/Resources/Prefabs/mana)
        string manaPath = "Prefabs/mana";
        var manaPrefab = Resources.Load<GameObject>(manaPath);       // Prefab �ε�

        if (manaPrefab == null)
        {
            return;
        }

        // Prefab �ν��Ͻ�ȭ
        var manaGo = Instantiate(manaPrefab);

        // �ν��Ͻ�ȭ�� ��ü�� ���� ������ �ڽ����� ����
        manaGo.transform.SetParent(this.transform);
        manaGo.transform.localPosition = Vector3.zero;
        manaGo.transform.localScale = Vector3.zero;

        // ���°��� FULL�� ����
        ChangeStateTo(MANASLOTSTATE.FULL);
    }
}
