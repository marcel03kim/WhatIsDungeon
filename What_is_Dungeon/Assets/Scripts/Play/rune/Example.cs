using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Example : MonoBehaviour
{
    public List<ExItem> items = new List<ExItem>();
    public string[] foods = new string[3];

    void Start()
    {
        items.Add(new ExItem("Į", "���X ���X"));
        items.Add(new ExItem("����", "���°� �� �����̴ϱ�"));
        items.Add(new ExItem("����", "�ϵ� �ϵ�"));
        items.Add(new ExItem("������", "�Ϳ���"));
        items.Add(new ExItem("��", "���� ����"));
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log(FindItem("Į").itemName + FindItem("Į").itemText);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            Debug.Log(FindItem("����").itemName + FindItem("����").itemText);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            Debug.Log(FindItem("������").itemName + FindItem("������").itemText);
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log(FindItem("��").itemName + FindItem("��").itemText);
        }
    }

    ExItem FindItem(string itemName)
    {
        foreach (var item in items)
        {
            if(item.itemName == itemName)
            {
                return item;
            }
        }

        return null;
    }

}

[System.Serializable]
public class ExItem
{

    public string itemName;
    public string itemText;

    public ExItem(string itemName, string itemText)
    {
        this.itemName = itemName;
        this.itemText = itemText;
    }
}
