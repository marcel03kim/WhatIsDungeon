using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.UI;


public class GameController : MonoBehaviour
{   
    public SlotManager[] slots;                                //게임 컴트롤러에서는 Slot 배열을 관리

    private Vector3 _target;
    private RuneManager carryingRune;                      //잡고 있는 아이템 정보 값 관리

    public Dictionary<int, SlotManager> slotDictionary;       //Slot id, Slot class 관리하기 위한 자료구조

    public float coin;
    public Text coinText;             //coin 관리 
    public GameObject cantBuy;

    private void Start()
    {
        cantBuy.SetActive(false);
        slotDictionary = new Dictionary<int, SlotManager>();   //초기화

        for (int i = 0; i < slots.Length; i++)
        {                                               //각 슬롯의 ID를 설정하고 딕셔너리에 추가
            slots[i].id = i;
            slotDictionary.Add(i, slots[i]);
        }
    }

    void Update()
    {
        coin += Time.deltaTime * 4;
        coinText.text = ": " + ((int)coin).ToString();

        if (Input.GetMouseButtonDown(0)) //마우스 누를 때
        {
            SendRayCast();
        }

        if (Input.GetMouseButton(0) && carryingRune)    //잡고 이동시킬 때
        {
            OnruneSelected();
        }

        if (Input.GetMouseButtonUp(0))  //마우스 버튼을 놓을때
        {
            SendRayCast();
        }
    }
    void SendRayCast()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            var slot = hit.transform.GetComponent<SlotManager>();          //Raycast를 통해 나온 Slot칸
            if (slot.state == SlotManager.SLOTSTATE.FULL && carryingRune == null)
            {
                string runePath = "Prefabs/rune_Grabbed_" + slot.runeObject.tag + slot.runeObject.level.ToString("000");
                var runeGo = (GameObject)Instantiate(Resources.Load<GameObject>(runePath));     //아이템 생성

                runeGo.transform.SetParent(this.transform);
                runeGo.transform.localPosition = Vector3.zero;
                runeGo.transform.localScale = Vector3.one * 2;

                carryingRune = runeGo.GetComponent<RuneManager>();         //슬롯 정보 입력
                carryingRune.InitDummy(slot.id, slot.runeObject.level, slot.runeObject.tag);

                slot.runeGrabbed();
            }
            else if (slot.state == SlotManager.SLOTSTATE.EMPTY && carryingRune != null)
            {//빈 슬롯에 아이템 배치
                slot.Createrune(carryingRune.runeLevel, carryingRune.runeTag);       //잡고 있는것 슬롯 위치에 생성
                Destroy(carryingRune.gameObject);           //잡고 있는것 파괴
            }
            else if (slot.state == SlotManager.SLOTSTATE.FULL && carryingRune != null)
            {//Checking 후 병합
                if (slot.runeObject.level == carryingRune.runeLevel
                    && slot.runeObject.tag == carryingRune.runeTag)
                {
                    OnruneMergedWithTarget(slot.id);    //병합 함수 호출
                }
                else
                {
                    OnruneCarryFail();  //아이템 배치 실패
                }
            }
        }
        else
        {
            if (!carryingRune) return;
            OnruneCarryFail();  //아이템 배치 실패
        }
        
    }


    void OnruneSelected()
    {   //아이템을 선택하고 마우스 위치로 이동 
        _target = Camera.main.ScreenToWorldPoint(Input.mousePosition);  //좌표변환
        _target.z = 0;
        var delta = 10 * Time.deltaTime;
        delta *= Vector3.Distance(transform.position, _target);
        carryingRune.transform.position = Vector3.MoveTowards(carryingRune.transform.position, _target, delta);
    }
    
    void OnruneMergedWithTarget(int targetSlotId)
    {
        var slot = GetSlotById(targetSlotId);
        Destroy(slot.runeObject.gameObject);            //slot에 있는 물체 파괴
        slot.Createrune(carryingRune.runeLevel + 1, carryingRune.runeTag);       //슬롯에 다음 번호 물체 생성
        Destroy(carryingRune.gameObject);               //잡고 있는 물체 파괴
    }

    void OnruneCarryFail()
    {//아이템 배치 실패 시 실행
        var slot = GetSlotById(carryingRune.slotId);        //슬롯 위치 확인
        slot.Createrune(carryingRune.runeLevel, carryingRune.runeTag);               //해당 슬롯에 다시 생성
        Destroy(carryingRune.gameObject);                   //잡고 있는 물체 파괴
    }
    void PlaceRandomrune(string runeTag)
    {//랜덤한 슬롯에 아이템 배치
        if (AllSlotsOccupied())
        {
            return;
        }

        var rand = UnityEngine.Random.Range(0, slots.Length); //유니티 랜덤함수를 가져와서 0 ~ 배열 크기 사이 값
        var slot = GetSlotById(rand);
        while (slot.state == SlotManager.SLOTSTATE.FULL)
        {
            rand = UnityEngine.Random.Range(0, slots.Length);
            slot = GetSlotById(rand);
        }
        slot.GetComponent<SlotManager>().Createrune(0, runeTag);
    }

    public void CreateFireRune()
    {
        if (coin > 2)
        {
            PlaceRandomrune("Fire");
            coin -= 10;
        }
    }

    public void CreateIceRune()
    {
        if (coin > 2)
        {
            PlaceRandomrune("Ice");
            coin -= 10;
        }
    }

    public void CreateWindRune()
    {
        if (coin > 2)
        {
            PlaceRandomrune("Wind");
            coin -= 10;
        }
    }

    public void CreateLightningRune()
    {
        if (coin > 2)
        {
            PlaceRandomrune("Lightning");
            coin -= 10;
        }
    }

    bool AllSlotsOccupied()
    {//모든 슬롯이 채워져 있는지 확인
        foreach(var slot in slots)                       //foreach문을 통해서 Slots 배열을 검사후
        {
            if (slot.state == SlotManager.SLOTSTATE.EMPTY)       //비어있는지 확인
            {
                return false;
            }
        }
        return true;
    }
    SlotManager GetSlotById(int id)
    {//슬롯 ID로 딕셔너리에서 Slot 클래스를 리턴
        return slotDictionary[id];
    }
    
    
}
