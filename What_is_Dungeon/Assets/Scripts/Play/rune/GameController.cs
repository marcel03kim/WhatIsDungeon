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
    public float manaIncreaseInterval = 1.0f; // 1�ʸ��� mana ����

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
            if (Input.GetMouseButtonDown(0)) // ���콺 ���� ��
            {
                SendRayCast();
            }

            if (Input.GetMouseButton(0) && carryingRune) // ��� �̵���ų ��
            {
                OnruneSelected();
            }

            if (Input.GetMouseButtonUp(0)) // ���콺 ��ư�� ���� ��
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
            var slot = hit.transform.GetComponent<SlotManager>();          //Raycast�� ���� ���� Slotĭ
            if (slot.state == SlotManager.SLOTSTATE.FULL && carryingRune == null)
            {
                string runePath = "Prefabs/rune_Grabbed_" + slot.runeObject.tag + slot.runeObject.level.ToString("000");
                var runeGo = (GameObject)Instantiate(Resources.Load<GameObject>(runePath));     //������ ����

                runeGo.transform.SetParent(this.transform);
                runeGo.transform.localPosition = Vector3.zero;
                runeGo.transform.localScale = Vector3.one * 2;

                carryingRune = runeGo.GetComponent<RuneManager>();         //���� ���� �Է�
                carryingRune.InitDummy(slot.id, slot.runeObject.level, slot.runeObject.tag);

                slot.runeGrabbed();
            }
            else if (slot.state == SlotManager.SLOTSTATE.EMPTY && carryingRune != null)
            {//�� ���Կ� ������ ��ġ
                slot.Createrune(carryingRune.runeLevel, carryingRune.runeTag);       //��� �ִ°� ���� ��ġ�� ����
                Destroy(carryingRune.gameObject);           //��� �ִ°� �ı�
            }
            else if (slot.state == SlotManager.SLOTSTATE.FULL && carryingRune != null)
            {//Checking �� ����
                if (slot.runeObject.level == carryingRune.runeLevel
                    && slot.runeObject.tag == carryingRune.runeTag)
                {
                    OnruneMergedWithTarget(slot.id);    //���� �Լ� ȣ��
                }
                else
                {
                    OnruneCarryFail();  //������ ��ġ ����
                }
            }
        }
        else
        {
            if (!carryingRune) return;
            OnruneCarryFail();  //������ ��ġ ����
        }
        
    }


    void OnruneSelected()
    {   //�������� �����ϰ� ���콺 ��ġ�� �̵� 
        _target = Camera.main.ScreenToWorldPoint(Input.mousePosition);  //��ǥ��ȯ
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
            Destroy(slot.runeObject.gameObject);            //slot�� �ִ� ��ü �ı�
            slot.Createrune(carryingRune.runeLevel + 1, carryingRune.runeTag);       //���Կ� ���� ��ȣ ��ü ����
            Destroy(carryingRune.gameObject);               //��� �ִ� ��ü �ı�
        }
        else
        {
            OnruneCarryFail();
        }
    }

    void OnruneCarryFail()
    {//������ ��ġ ���� �� ����
        var slot = GetSlotById(carryingRune.slotId);        //���� ��ġ Ȯ��
        slot.Createrune(carryingRune.runeLevel, carryingRune.runeTag);               //�ش� ���Կ� �ٽ� ����
        Destroy(carryingRune.gameObject);                   //��� �ִ� ��ü �ı�
    }
    void PlaceRandomrune(string runeTag)
    {//������ ���Կ� ������ ��ġ
        if (AllSlotsOccupied())
        {
            return;
        }

        var rand = UnityEngine.Random.Range(0, slots.Length); //����Ƽ �����Լ��� �����ͼ� 0 ~ �迭 ũ�� ���� ��
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
                    Destroy(manaSlot.transform.GetChild(0).gameObject); // ������ ���� ������ ����
                    manaSlot.ChangeStateTo(Slot.MANASLOTSTATE.EMPTY); // ���� ���¸� EMPTY�� ����
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
    {//��� ������ ä���� �ִ��� Ȯ��
        foreach(var slot in slots)                       //foreach���� ���ؼ� Slots �迭�� �˻���
        {
            if (slot.state == SlotManager.SLOTSTATE.EMPTY)       //����ִ��� Ȯ��
            {
                return false;
            }
        }
        return true;
    }
    bool AllManaSlotsOccupied()
    {//��� ������ ä���� �ִ��� Ȯ��
        foreach(var slot in manaSlots)                       //foreach���� ���ؼ� Slots �迭�� �˻���
        {
            if (slot.state == Slot.MANASLOTSTATE.EMPTY)       //����ִ��� Ȯ��
            {
                return false;
            }
        }
        return true;
    }
    SlotManager GetSlotById(int id)
    {//���� ID�� ��ųʸ����� Slot Ŭ������ ����
        return slotDictionary[id];
    }
    
    
}
