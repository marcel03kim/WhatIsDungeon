using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameController : MonoBehaviour
{
    public SlotManager[] slots;
    private Vector3 _target;
    private RuneManager carryingRune;
    public Dictionary<int, SlotManager> slotDictionary;
    public SkillManager skillManager;
    public Slot[] manaSlots;
    public Dictionary<int, Slot> manaSlotDictionary;

    public int currentMana;
    public int maxMana;
    public int minMana;

    private int currentManaSlotIndex;
    private float manaIncreaseTimer;
    public float manaIncreaseInterval = 1.0f; // 1초마다 mana 증가

    public Text manaText;

    private void Start()
    {
        maxMana = 10;
        minMana = 0;
        currentManaSlotIndex = 10;

        slotDictionary = new Dictionary<int, SlotManager>();
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].id = i;
            slotDictionary.Add(i, slots[i]);
        }

        currentManaSlotIndex = 0;

        manaSlotDictionary = new Dictionary<int, Slot>();

        for (int i = 0; i < manaSlots.Length; i++)
        {
            manaSlots[i].id = i;
            manaSlotDictionary.Add(i, manaSlots[i]);
        }

        manaIncreaseTimer = 0.0f;
    }


    void Update()
    {
        manaIncreaseTimer += Time.deltaTime;

        if(currentMana < 11)
        {
            if (manaIncreaseTimer >= manaIncreaseInterval)
            {
                manaIncreaseTimer = 0.0f;
                currentMana++;
                manaText.text = currentMana + " / " + maxMana;
                PlaceMana();
            }
        }
        

        if (Time.timeScale > 0)
        {
            if (Input.GetMouseButtonDown(0)) // 마우스 누를 때
            {
                SendRayCast();
            }

            if (Input.GetMouseButton(0) && carryingRune) // 잡고 이동시킬 때
            {
                OnruneSelected();
            }

            if (Input.GetMouseButtonUp(0)) // 마우스 버튼을 놓을 때
            {
                SendRayCast();
            }
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
        if (carryingRune.runeLevel < 2)
        {
            var slot = GetSlotById(targetSlotId);
            Destroy(slot.runeObject.gameObject);            //slot에 있는 물체 파괴
            slot.Createrune(carryingRune.runeLevel + 1, carryingRune.runeTag);       //슬롯에 다음 번호 물체 생성
            Destroy(carryingRune.gameObject);               //잡고 있는 물체 파괴
        }
        else
        {
            OnruneCarryFail();
        }
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

    void PlaceMana()
    {
        if (AllManaSlotsOccupied())
        {
            return;
        }

        if(currentMana < 11)
        {
            var manaSlot = manaSlots[currentManaSlotIndex];
            while (manaSlot.state == Slot.MANASLOTSTATE.FULL)
            {
                currentManaSlotIndex = (currentManaSlotIndex + 1) % manaSlots.Length;
                manaSlot = manaSlots[currentManaSlotIndex];
            }
            manaSlot.GetComponent<Slot>().CreateMana();
            currentManaSlotIndex = (currentManaSlotIndex + 1) % manaSlots.Length;
        }
    }


    public void DecreaseMana(int amount)
    {
        currentMana -= amount;
        RemoveMana(amount);
    }


    void RemoveMana(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            foreach (var manaSlot in manaSlots)
            {
                if (manaSlot.state == Slot.MANASLOTSTATE.FULL)
                {
                    Destroy(manaSlot.transform.GetChild(0).gameObject); // 생성된 마나 프리팹 삭제
                    manaSlot.ChangeStateTo(Slot.MANASLOTSTATE.EMPTY); // 슬롯 상태를 EMPTY로 변경
                    break;
                }
            }
        }
    }


    public void CreateFireRune()
    {
        if (currentMana >= 1)
        {
            PlaceRandomrune("Fire");
            DecreaseMana(1);
        }
    }

    public void CreateIceRune()
    {
        if (currentMana >= 2)
        {
            PlaceRandomrune("Ice");
            DecreaseMana(2);
        }
    }

    public void CreateWindRune()
    {
        if (currentMana >= 4)
        {
            PlaceRandomrune("Wind");
            DecreaseMana(4);
        }
    }

    public void CreateLightningRune()
    {
        if (currentMana >= 6)
        {
            PlaceRandomrune("Lightning");
            DecreaseMana(6);
        }
    }

    public void ActiveFireSkill()
    {
        foreach (SlotManager slot in slots)
        {
            if (slot.state == SlotManager.SLOTSTATE.FULL && slot.runeObject.tag == "Fire")
            {
                switch (slot.runeObject.level)
                {
                    case 2:
                        skillManager.GetComponent<SkillManager>().FireSkill(2);
                        break;
                    case 1:
                        skillManager.GetComponent<SkillManager>().FireSkill(1);
                        break;
                    case 0:
                        skillManager.GetComponent<SkillManager>().FireSkill(0);
                        break;
                }
            }
        }
        
    }
    public void ActiveIceSkill()
    {
        foreach (SlotManager slot in slots)
        {
            if (slot.state == SlotManager.SLOTSTATE.FULL && slot.runeObject.tag == "Ice")
            {
                switch (slot.runeObject.level)
                {
                    case 2:
                        skillManager.GetComponent<SkillManager>().IceSkill(2);
                        break;
                    case 1:
                        skillManager.GetComponent<SkillManager>().IceSkill(1);
                        break;
                    case 0:
                        skillManager.GetComponent<SkillManager>().IceSkill(0);
                        break;
                }
            }
        }
        
    }
    public void ActiveWindSkill()
    {
        foreach (SlotManager slot in slots)
        {
            if (slot.state == SlotManager.SLOTSTATE.FULL && slot.runeObject.tag == "Wind")
            {
                switch (slot.runeObject.level)
                {
                    case 2:
                        skillManager.GetComponent<SkillManager>().WindSkill(2);
                        break;
                    case 1:
                        skillManager.GetComponent<SkillManager>().WindSkill(1);
                        break;
                    case 0:
                        skillManager.GetComponent<SkillManager>().WindSkill(0);
                        break;
                }
            }
        }
    }
    public void ActiveLightningSkill()
    {
        
        foreach (SlotManager slot in slots)
        {
            if (slot.state == SlotManager.SLOTSTATE.FULL && slot.runeObject.tag == "Lightning")
            {
                switch (slot.runeObject.level)
                {
                    case 2:
                        skillManager.GetComponent<SkillManager>().LightningSkill(2);
                        break;
                    case 1:
                        skillManager.GetComponent<SkillManager>().LightningSkill(1);
                        break;
                    case 0:
                        skillManager.GetComponent<SkillManager>().LightningSkill(0);
                        break;
                }
            }
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
    bool AllManaSlotsOccupied()
    {//모든 슬롯이 채워져 있는지 확인
        foreach(var slot in manaSlots)                       //foreach문을 통해서 Slots 배열을 검사후
        {
            if (slot.state == Slot.MANASLOTSTATE.EMPTY)       //비어있는지 확인
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
