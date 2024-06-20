using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    public TMP_Text manaText;                       // 현재 마나의 양을 표시할 TMP
    public Transform manaIconBG;

    public GameObject manaPrefab;                   // 마나 이미지 원본 프리팹
    private List<GameObject> manaObjects = new List<GameObject>();           // 지금까지 생성한 마나를 저장하는 리스트

    private int manaAmount = 0;                     // 현재 마나 양
    private int maxManaAmount = 10;                 // 최대 마나 양
    private float manaAdditionTimer = 0f;


    private void Start()
    {

        slotDictionary = new Dictionary<int, SlotManager>();
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].id = i;
            slotDictionary.Add(i, slots[i]);
        }

        
    }


    void Update()
    {
        if (Time.time > 1f)
        {
            manaAdditionTimer += Time.deltaTime;

        }
        if (manaAdditionTimer >= 1f)
        {
            AddMana(1);
            ChangeManaText();
            manaAdditionTimer -= 1f; // 1초를 초과한 시간을 차감
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
            var slot = hit.transform.GetComponent<SlotManager>(); // Raycast를 통해 나온 Slot칸
            if (slot.state == SlotManager.SLOTSTATE.FULL && carryingRune == null)
            {
                string runePath = "Prefabs/rune_Grabbed_" + slot.runeObject.tag + slot.runeObject.level.ToString("000");
                var runeGo = (GameObject)Instantiate(Resources.Load<GameObject>(runePath)); // 아이템 생성

                runeGo.transform.SetParent(this.transform);
                runeGo.transform.localScale = Vector3.one * 2;

                carryingRune = runeGo.GetComponent<RuneManager>(); // 슬롯 정보 입력
                carryingRune.InitDummy(slot.id, slot.runeObject.level, slot.runeObject.tag);

                slot.runeGrabbed();

                // 룬을 마우스 위치로 이동
                MoveRuneToMousePosition(carryingRune);
            }
            else if (slot.state == SlotManager.SLOTSTATE.EMPTY && carryingRune != null)
            {
                // 빈 슬롯에 아이템 배치
                slot.Createrune(carryingRune.runeLevel, carryingRune.runeTag); // 잡고 있는 것 슬롯 위치에 생성
                Destroy(carryingRune.gameObject); // 잡고 있는 것 파괴
            }
            else if (slot.state == SlotManager.SLOTSTATE.FULL && carryingRune != null)
            {
                // Checking 후 병합
                if (slot.runeObject.level == carryingRune.runeLevel && slot.runeObject.tag == carryingRune.runeTag)
                {
                    OnruneMergedWithTarget(slot.id); // 병합 함수 호출
                }
                else
                {
                    OnruneCarryFail(); // 아이템 배치 실패
                }
            }
        }
        else
        {
            if (!carryingRune) return;
            OnruneCarryFail(); // 아이템 배치 실패
        }
    }

    void MoveRuneToMousePosition(RuneManager rune)
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = 10f; // Set a distance from the camera to place the rune
        Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        rune.transform.position = worldMousePosition;
    }
   

    void OnruneSelected()
    {
        MoveRuneToMousePosition(carryingRune);
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

    public void AddMana(int amount)
    {       // 마나를 추가하는 함수
        if (amount < 0) amount *= -1;

        if (manaObjects.Count + amount > maxManaAmount)
        {
            manaAmount = maxManaAmount;
        }
        else
        {
            manaAmount += amount;
        }

        if (manaObjects.Count < maxManaAmount)
        {
            for (int i = 0; i < amount; i++)
            {
                var temp = Instantiate(manaPrefab, manaIconBG);
                manaObjects.Add(temp);
            }
        }
    }
    private void RemoveMana(int amount)
    {
        if (amount < 0) amount *= -1;

        if (manaAmount - amount <= 0)
        {
            amount = manaAmount; // 남아있는 모든 마나를 제거
            manaAmount = 0;
        }
        else
        {
            manaAmount -= amount;
        }

        int removeCount = Mathf.Min(amount, manaObjects.Count);

        for (int i = 0; i < removeCount; i++)
        {
            var go = manaObjects[0];
            manaObjects.RemoveAt(0);
            Destroy(go);
        }

        ChangeManaText(); // 마나가 변경된 후 텍스트 업데이트
    }
    private void ChangeManaText()
    {       // 마나를 표시하는 함수
       manaText.text = $"{manaAmount} / {maxManaAmount}";
    }


    public void CreateFireRune()
    {
        if (manaAmount >= 2)
        {
            PlaceRandomrune("Fire");
            RemoveMana(2);
            ChangeManaText();

        }
    }

    public void CreateIceRune()
    {
        if (manaAmount >= 3)
        {
            PlaceRandomrune("Ice");
            RemoveMana(3);
            ChangeManaText();
        }
    }

    public void CreateWindRune()
    {
        if (manaAmount >= 4)
        {
            PlaceRandomrune("Wind");
            RemoveMana(4);
            ChangeManaText();
        }
    }

    public void CreateLightningRune()
    {
        if (manaAmount >= 6)
        {
            PlaceRandomrune("Lightning");
            RemoveMana(6);
            ChangeManaText();
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
