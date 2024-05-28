using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Example : MonoBehaviour
{
    public List<ExItem> items = new List<ExItem>();
    public string[] foods = new string[3];

    void Start()
    {
        items.Add(new ExItem("Ä®", "À¸›X À¸›X"));
        items.Add(new ExItem("¹æÆÐ", "¾ÆÇÂ°Ç µü Áú»öÀÌ´Ï±î"));
        items.Add(new ExItem("°¡À§", "½ÏµÏ ½ÏµÏ"));
        items.Add(new ExItem("Áö··ÀÌ", "±Í¿©¿ö"));
        items.Add(new ExItem("ÃÑ", "»§¾ß »§¾ß"));
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log(FindItem("Ä®").itemName + FindItem("Ä®").itemText);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            Debug.Log(FindItem("¹æÆÐ").itemName + FindItem("¹æÆÐ").itemText);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            Debug.Log(FindItem("Áö··ÀÌ").itemName + FindItem("Áö··ÀÌ").itemText);
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log(FindItem("ÃÑ").itemName + FindItem("ÃÑ").itemText);
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
