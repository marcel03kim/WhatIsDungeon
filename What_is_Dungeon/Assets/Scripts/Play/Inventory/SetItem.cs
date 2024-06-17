using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetItem : MonoBehaviour
{
    public GameObject player;
    public GameObject gameManager;
    public bool isEnemySlow;
    public bool isHp;
    public bool isMana;
    public float SlowTime;

    public void Start()
    {
        isHp = false;
        isMana = false;
        isEnemySlow = false;
        player.GetComponent<PlayerController>();
        gameManager.GetComponent<GameController>();
    }

    private void Update()
    {
        if (isEnemySlow && SlowTime < 3f)
        {
            SlowTime += Time.deltaTime;
        }

        if (isEnemySlow && SlowTime > 3f)
        {
            SlowTime = 0;
        }
    }
    public void HpPotion()
    {
        if(!isHp)
        {
            player.GetComponent<PlayerController>().Hp += 1;
            isHp = true;
        }
    }

    public void ManaPotion()
    {
        if(!isMana)
        {
            gameManager.GetComponent<GameController>().AddMana(5);
            isMana = true;
        }
    }

    public void Cookie()
    {
        if(!isEnemySlow)
        {
            FindObjectOfType<Enemy>().speed = 2f;
            isEnemySlow = true;
        }
    }
}
