using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.UI;


public class GameController : MonoBehaviour
{   
    public SlotManager[] slots;                                //���� ��Ʈ�ѷ������� Slot �迭�� ����

    private Vector3 _target;
    private RuneManager carryingRune;                      //��� �ִ� ������ ���� �� ����

    public Dictionary<int, SlotManager> slotDictionary;       //Slot id, Slot class �����ϱ� ���� �ڷᱸ��

    public float coin;
    public Text coinText;             //coin ���� 
    public GameObject cantBuy;

    private void Start()
    {
        cantBuy.SetActive(false);
        slotDictionary = new Dictionary<int, SlotManager>();   //�ʱ�ȭ

        for (int i = 0; i < slots.Length; i++)
        {                                               //�� ������ ID�� �����ϰ� ��ųʸ��� �߰�
            slots[i].id = i;
            slotDictionary.Add(i, slots[i]);
        }
    }

    void Update()
    {
        coin += Time.deltaTime * 4;
        coinText.text = ": " + ((int)coin).ToString();

        if (Input.GetMouseButtonDown(0)) //���콺 ���� ��
        {
            SendRayCast();
        }

        if (Input.GetMouseButton(0) && carryingRune)    //��� �̵���ų ��
        {
            OnruneSelected();
        }

        if (Input.GetMouseButtonUp(0))  //���콺 ��ư�� ������
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
        var slot = GetSlotById(targetSlotId);
        Destroy(slot.runeObject.gameObject);            //slot�� �ִ� ��ü �ı�
        slot.Createrune(carryingRune.runeLevel + 1, carryingRune.runeTag);       //���Կ� ���� ��ȣ ��ü ����
        Destroy(carryingRune.gameObject);               //��� �ִ� ��ü �ı�
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
    SlotManager GetSlotById(int id)
    {//���� ID�� ��ųʸ����� Slot Ŭ������ ����
        return slotDictionary[id];
    }
    
    
}
