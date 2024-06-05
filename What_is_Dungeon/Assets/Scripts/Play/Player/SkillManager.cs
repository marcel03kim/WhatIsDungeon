using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static UnityEngine.GraphicsBuffer;
using Unity.VisualScripting;

public class SkillManager : MonoBehaviour
{
    public GameObject[] hideSkillButton;
    public GameObject[] textPros;
    public TextMeshProUGUI[] hideSkillTimeTexts; // TextMeshProUGUI 배열로 수정
    public Image[] hideSkillImage;
    public bool[] isHideSkill = { false, false, false, false };
    private float[] skillTimes = { 5, 5, 8, 12 };
    private float[] getSkillTimes = { 0, 0, 0, 0 };

    public string[] enemyTags = { "Enemy", "Wave1", "Wave2", "Wave3", "Boss" };

    private List<Vector3> meteorPositions = new List<Vector3>(); // 메테오 위치를 저장할 리스트
    public float meteorRadius;

    public GameObject LightPrefab;

    private void Start()
    {
        hideSkillTimeTexts = new TextMeshProUGUI[textPros.Length]; // 배열 초기화

        for (int i = 0; i < textPros.Length; i++)
        {
            hideSkillTimeTexts[i] = textPros[i].GetComponent<TextMeshProUGUI>(); // GetComponent로 TextMeshProUGUI 가져오기
            if (hideSkillButton[i] != null)
            {
                hideSkillButton[i].SetActive(false);
            }
        }
    }

    private void Update()
    {
        hideSkillCheck();
    }

    public void hideSkillSetting(int skillNum)
    {
        if (skillNum < 0 || skillNum >= hideSkillButton.Length)
        {
            Debug.LogError("Invalid skill number: " + skillNum);
            return;
        }

        if (hideSkillButton[skillNum] == null)
        {
            Debug.LogError("hideSkillButton[" + skillNum + "] is not set.");
            return;
        }


        Button button = hideSkillButton[skillNum].GetComponent<Button>();
        if (button == null)
        {
            button = hideSkillButton[skillNum].AddComponent<Button>();
        }

        hideSkillButton[skillNum].SetActive(true);
        button.interactable = false; // 버튼 비활성화

        getSkillTimes[skillNum] = skillTimes[skillNum];
        isHideSkill[skillNum] = true;
    }

    private void hideSkillCheck()
    {
        for (int i = 0; i < isHideSkill.Length; i++)
        {
            if (isHideSkill[i])
            {
                StartCoroutine(skillTimeCheck(i));
            }
        }
    }

    IEnumerator skillTimeCheck(int skillNum)
    {
        yield return null;
        if (getSkillTimes[skillNum] > 0)
        {
            getSkillTimes[skillNum] -= Time.deltaTime;

            if (getSkillTimes[skillNum] < 0)
            {
                getSkillTimes[skillNum] = 0;
                isHideSkill[skillNum] = false;
                hideSkillButton[skillNum].SetActive(false);
                Button button = hideSkillButton[skillNum].GetComponent<Button>();

                if (button != null)
                {
                    button.interactable = true; // 버튼 활성화
                }
            }

            hideSkillTimeTexts[skillNum].text = getSkillTimes[skillNum].ToString("00");

            float time = getSkillTimes[skillNum] / skillTimes[skillNum];
            hideSkillImage[skillNum].fillAmount = time;
        }
    }


    public void FireSkill(int level)
    {
        Vector3 randomPosition = GetRandomEnemyPosition();
        if (randomPosition == Vector3.zero) return;

        meteorPositions.Add(randomPosition);

        float radius = meteorRadius;
        switch (level)
        {
            case 2:
                Meteor(100, randomPosition, 100f);
                break;
            case 1:
                Meteor(60, randomPosition, 50f);
                break;
            case 0:
                Meteor(30, randomPosition, 30f);
                break;
        }
    }

    public void Meteor(int damage, Vector3 center, float radius)
    {
        Collider[] hitColliders = Physics.OverlapSphere(center, radius);
        foreach (var hitCollider in hitColliders)
        {
            Enemy enemy = hitCollider.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.currentHp -= damage;
            }
        }
    }

    public void IceSkill(int level)
    {
        switch (level)
        {
            case 2:
                Freeze(5f, 10);
                break;
            case 1:
                Freeze(3f, 10);
                break;
            case 0:
                Freeze(1.5f, 10);
                break;
        }
    }

    public void Freeze(float time, int damage)
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 100.0f);
        foreach (var hitCollider in hitColliders)
        {
            Enemy enemy = hitCollider.GetComponent<Enemy>();
            if (enemy != null)
            {
                Rigidbody enemyRigidbody = enemy.GetComponent<Rigidbody>();
                if (enemyRigidbody != null)
                {
                    StartCoroutine(FreezeEnemy(enemy, time, damage));

                }
            }
        }
    }

    IEnumerator FreezeEnemy(Enemy enemy, float time, int damage)
    {
        if (!enemy.isStop) // 이미 얼어있는 상태인지 확인
        {
            enemy.isStop = true; // 적의 움직임을 멈추게 함

            float elapsed = 0f;
            while (elapsed < time)
            {
                elapsed += Time.deltaTime;
                enemy.currentHp -= damage * Time.deltaTime;

                yield return null;
            }

            enemy.isStop = false; // 적의 움직임을 다시 활성화
        }
    }

    public void WindSkill(int level)
    {
        switch (level)
        {
            case 2:
                Tornado(5f, 3f, 50);
                break;
            case 1:
                Tornado(2.5f, 1f, 30);
                break;
            case 0:
                Tornado(1f, 0.5f, 10);
                break;
        }
    }

    public void Tornado(float power, float time, int damage)
    {

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 100.0f);
        foreach (var hitCollider in hitColliders)
        {
            Enemy enemy = hitCollider.GetComponent<Enemy>();
            if (enemy != null)
            {
                Rigidbody enemyRigidbody = enemy.GetComponent<Rigidbody>();
                if (enemyRigidbody != null)
                {
                    StartCoroutine(KnockBackEnemy(enemy, time, damage));
                    enemyRigidbody.AddForce(Vector3.left * power, ForceMode.Impulse); // 적을 뒤로 밀어냄
                    enemy.currentHp -= damage;
                }
            }
        }
    }

    IEnumerator KnockBackEnemy(Enemy enemy, float time, int damage)
    {
        if (!enemy.isStop) // 이미 얼어있는 상태인지 확인
        {
            enemy.isStop = true; // 적의 움직임을 멈추게 함

            float elapsed = 0f;
            while (elapsed < time)
            {
                elapsed += Time.deltaTime;

                yield return null;
            }

            enemy.isStop = false; // 적의 움직임을 다시 활성화
        }
    }
    public void LightningSkill(int level)
    {
        switch (level)
        {
            case 2:
                ChainLightning(150f, 10f, 5f, 50);
                break;
            case 1:
                ChainLightning(100f, 10f, 3f, 25);
                break;
            case 0:
                ChainLightning(60f, 10f, 1f, 10);
                break;
        }
    }

    public void ChainLightning(float distance, float LightSpeed, float time, int damage)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, distance);
        foreach (Collider col in colliders)
        {
            if (col.CompareTag("Enemy"))
            {
                Vector3 direction = (col.transform.position - transform.position).normalized;
                transform.Translate(direction * LightSpeed * Time.deltaTime);

                // 충돌 시 연쇄 효과 발동
                chain(col.gameObject);

                break; // 하나의 적만 추적하도록 하기 위해 루프 중단
            }
        }

        void chain(GameObject target)
        {
            Destroy(target); // 충돌된 오브젝트 파괴

            Collider[] colliders = Physics.OverlapSphere(target.transform.position, distance);
            foreach (Collider col in colliders)
            {
                if (col.CompareTag("Enemy"))
                {
                    Vector3 spawnPosition = col.transform.position;
                    Destroy(col.gameObject); // 주변 적 파괴

                    // 새로운 오브젝트 생성
                    Instantiate(LightPrefab, spawnPosition, Quaternion.identity);
                }
            }
        }
    }

    
    private Vector3 GetRandomEnemyPosition()
    {
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        if (enemies.Length == 0) return Vector3.zero;

        Enemy randomEnemy = enemies[Random.Range(0, enemies.Length)];
        return randomEnemy.transform.position;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        foreach (var position in meteorPositions)
        {
            Gizmos.DrawWireSphere(position, meteorRadius);
        }
    }
    private GameObject[] FindGameObjectsWithTags(string[] tags)
    {
        List<GameObject> allObjects = new List<GameObject>();
        foreach (string tag in tags)
        {
            GameObject[] taggedObjects = GameObject.FindGameObjectsWithTag(tag);
            allObjects.AddRange(taggedObjects);
        }
        return allObjects.ToArray();
    }
}
