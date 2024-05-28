using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotManager : MonoBehaviour
{
    public enum SLOTSTATE       //슬롯상태값
    {
        EMPTY,
        FULL
    }

    public int id;                              //슬롯 번호 ID
    public Rune runeObject;                     //선언한 커스텀 Class ID
    public SLOTSTATE state = SLOTSTATE.EMPTY;   //Enum 값 선언

    private void ChangeStateTo(SLOTSTATE targetState)
    {//해당 슬롯의 상태값을 변환 시켜주는 함수
        state = targetState;
    }

    public void runeGrabbed()
    {//RayCast를 통해서 아이템을 잡았을 때
        Destroy(runeObject.gameObject);         //기존 아이템을 삭제
        ChangeStateTo(SLOTSTATE.EMPTY);         //슬롯은 빈 상태

    }   
    
    public void Createrune(int id, string tag)
    {
        
        //아이템 경로는 (Assets/Resources/Prefabs/rune_000)
        // Resoueces.Load(path) path = "Prefabs/rune_000" 이런식으로 작성해야함.
        string runePath = "Prefabs/rune_" + tag + id.ToString("000");
                
        //var runeGo = (GameObject)Instantiate(Resources.Load(runePath));
        // 본 형식은 리소스 로드 시 Object 타입으로 반환하기 때문에 GameObject 생성시 Null Ref. Exception 발생함.
        var runeGo = (GameObject)Instantiate(Resources.Load<GameObject>(runePath));

        runeGo.transform.SetParent(this.transform);
        runeGo.transform.localPosition = Vector3.zero;
        runeGo.transform.localScale = Vector3.one;
        //아이템 값 정보를 입력
        runeObject = runeGo.GetComponent<Rune>();
        runeObject.Init(id, tag, this); //함수를 통한 값 입력(this -> Slot Class)

        ChangeStateTo(SLOTSTATE.FULL);

    }
}
