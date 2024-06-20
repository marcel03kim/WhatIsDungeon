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
    public float effectDuration = 3f; // 효과 지속 시간

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
            // 플레이어 컨트롤러에서 체력을 증가시킴
            player.GetComponent<PlayerController>().Hp += 1;

            // 효과 프리팹을 생성하고, effectDuration 시간 후에 제거
            GameObject effectInstance = Instantiate(effectPrefabs[0], character.transform.position, Quaternion.identity);
            effectInstance.transform.localScale *= 15f;
            Destroy(effectInstance, effectDuration);

            isHp = true; // 효과 적용 상태로 변경
        }
    }

    public void ManaPotion()
    {
        if (!isMana)
        {
            // 게임 매니저에서 마나 추가
            gameManager.GetComponent<GameController>().AddMana(5);

            // 효과 프리팹을 생성하고, effectDuration 시간 후에 제거
            GameObject effectInstance = Instantiate(effectPrefabs[1], character.transform.position, Quaternion.identity);
            effectInstance.transform.localScale *= 15f; 
            Destroy(effectInstance, effectDuration);

            isMana = true; // 효과 적용 상태로 변경
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
