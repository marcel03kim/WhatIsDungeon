using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot : MonoBehaviour
{
    public enum MANASLOTSTATE  // 슬롯 상태값
    {
        EMPTY,
        FULL
    }

    public int id;  // 슬롯 번호 ID
    public MANASLOTSTATE state = MANASLOTSTATE.EMPTY;

    // 슬롯 상태값을 변환 시켜주는 함수
    public void ChangeStateTo(MANASLOTSTATE targetState)
    {
        state = targetState;
    }

    public void CreateMana()
    {
        // 아이템 경로는 (Assets/Resources/Prefabs/mana)
        string manaPath = "Prefabs/mana";
        var manaPrefab = Resources.Load<GameObject>(manaPath);       // Prefab 로드

        if (manaPrefab == null)
        {
            return;
        }

        // Prefab 인스턴스화
        var manaGo = Instantiate(manaPrefab);

        // 인스턴스화된 객체를 현재 슬롯의 자식으로 설정
        manaGo.transform.SetParent(this.transform);
        manaGo.transform.localPosition = Vector3.zero;
        manaGo.transform.localScale = Vector3.zero;

        // 상태값을 FULL로 변경
        ChangeStateTo(MANASLOTSTATE.FULL);
    }
}
