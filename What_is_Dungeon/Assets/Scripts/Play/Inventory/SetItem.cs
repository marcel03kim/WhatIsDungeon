using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetItem : MonoBehaviour
{
    public GameObject player;
    public GameObject character;
    public GameObject gameManager;
    public bool isEnemySlow;
    public bool isHp;
    public bool isMana;
    public float SlowTime;
    public GameObject[] effectPrefabs; 
    public float effectDuration = 3f; // ȿ�� ���� �ð�

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
        if (!isHp)
        {
            // �÷��̾� ��Ʈ�ѷ����� ü���� ������Ŵ
            player.GetComponent<PlayerController>().Hp += 1;

            // ȿ�� �������� �����ϰ�, effectDuration �ð� �Ŀ� ����
            GameObject effectInstance = Instantiate(effectPrefabs[0], character.transform.position, Quaternion.identity);
            effectInstance.transform.localScale *= 15f;
            Destroy(effectInstance, effectDuration);

            isHp = true; // ȿ�� ���� ���·� ����
        }
    }

    public void ManaPotion()
    {
        if (!isMana)
        {
            // ���� �Ŵ������� ���� �߰�
            gameManager.GetComponent<GameController>().AddMana(5);

            // ȿ�� �������� �����ϰ�, effectDuration �ð� �Ŀ� ����
            GameObject effectInstance = Instantiate(effectPrefabs[1], character.transform.position, Quaternion.identity);
            effectInstance.transform.localScale *= 15f; 
            Destroy(effectInstance, effectDuration);

            isMana = true; // ȿ�� ���� ���·� ����
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
