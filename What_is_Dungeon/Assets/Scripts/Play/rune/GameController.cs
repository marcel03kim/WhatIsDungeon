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

    public TMP_Text manaText;                       // ���� ������ ���� ǥ���� TMP
    public Transform manaIconBG;

    public GameObject manaPrefab;                   // ���� �̹��� ���� ������
    private List<GameObject> manaObjects = new List<GameObject>();           // ���ݱ��� ������ ������ �����ϴ� ����Ʈ

    private int manaAmount = 0;                     // ���� ���� ��
    private int maxManaAmount = 10;                 // �ִ� ���� ��
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
            manaAdditionTimer -= 1f; // 1�ʸ� �ʰ��� �ð��� ����
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
            var slot = hit.transform.GetComponent<SlotManager>(); // Raycast�� ���� ���� Slotĭ
            if (slot.state == SlotManager.SLOTSTATE.FULL && carryingRune == null)
            {
                string runePath = "Prefabs/rune_Grabbed_" + slot.runeObject.tag + slot.runeObject.level.ToString("000");
                var runeGo = (GameObject)Instantiate(Resources.Load<GameObject>(runePath)); // ������ ����

                runeGo.transform.SetParent(this.transform);
                runeGo.transform.localScale = Vector3.one * 2;

                carryingRune = runeGo.GetComponent<RuneManager>(); // ���� ���� �Է�
                carryingRune.InitDummy(slot.id, slot.runeObject.level, slot.runeObject.tag);

                slot.runeGrabbed();

                // ���� ���콺 ��ġ�� �̵�
                MoveRuneToMousePosition(carryingRune);
            }
            else if (slot.state == SlotManager.SLOTSTATE.EMPTY && carryingRune != null)
            {
                // �� ���Կ� ������ ��ġ
                slot.Createrune(carryingRune.runeLevel, carryingRune.runeTag); // ��� �ִ� �� ���� ��ġ�� ����
                Destroy(carryingRune.gameObject); // ��� �ִ� �� �ı�
            }
            else if (slot.state == SlotManager.SLOTSTATE.FULL && carryingRune != null)
            {
                // Checking �� ����
                if (slot.runeObject.level == carryingRune.runeLevel && slot.runeObject.tag == carryingRune.runeTag)
                {
                    OnruneMergedWithTarget(slot.id); // ���� �Լ� ȣ��
                }
                else
                {
                    OnruneCarryFail(); // ������ ��ġ ����
                }
            }
        }
        else
        {
            if (!carryingRune) return;
            OnruneCarryFail(); // ������ ��ġ ����
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

    public void AddMana(int amount)
    {       // ������ �߰��ϴ� �Լ�
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
            amount = manaAmount; // �����ִ� ��� ������ ����
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

        ChangeManaText(); // ������ ����� �� �ؽ�Ʈ ������Ʈ
    }
    private void ChangeManaText()
    {       // ������ ǥ���ϴ� �Լ�
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
