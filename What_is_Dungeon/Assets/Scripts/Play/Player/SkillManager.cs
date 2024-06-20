using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static UnityEngine.GraphicsBuffer;
using Unity.VisualScripting;
using Unity.Burst.CompilerServices;

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

    public GameObject[] effectPrefabs;
    public float destroyDelay = 2f;
    private GameObject effectInstance;

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
                Meteor(20, randomPosition, 30f);
                break;
            case 1:
                Meteor(14, randomPosition, 15f);
                break;
            case 0:
                Meteor(11, randomPosition, 5f);
                break;
        }
    }

    public void Meteor(int damage, Vector3 center, float radius)
    {
        Collider[] hitColliders = Physics.OverlapSphere(center, radius);

        // 모든 오버랩된 적에 대해 스킬 효과를 적용
        foreach (var hitCollider in hitColliders)
        {
            Enemy enemy = hitCollider.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.currentHp -= damage;

                // 이미 생성된 이펙트가 있다면 파괴합니다.
                if (effectInstance != null)
                {
                    Destroy(effectInstance);
                }

                // 이펙트를 enemy 위치에 생성합니다.
                effectInstance = Instantiate(effectPrefabs[0], enemy.transform.position, Quaternion.identity);
                effectInstance.transform.localScale *= radius; // radius에 따라 크기 조정
                Destroy(effectInstance, destroyDelay); // destroyDelay 초 후에 이펙트를 파괴합니다.
            }
        }
    }

    private void ResetEnemyColor()
    {
        // 모든 Enemy의 색상을 원래대로 되돌립니다.
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        foreach (var enemy in enemies)
        {
            Renderer enemyRenderer = enemy.GetComponent<Renderer>();
            if (enemyRenderer != null)
            {
                enemyRenderer.material.color = Color.white;
            }
        }
    }



    public void IceSkill(int level)
    {
        Vector3 randomPosition = GetRandomEnemyPosition();
        if (randomPosition == Vector3.zero) return;

        switch (level)
        {
            case 2:
                Freeze(randomPosition, 1.9f, 10);
                break;
            case 1:
                Freeze(randomPosition, 1.5f, 5);
                break;
            case 0:
                Freeze(randomPosition, 0.9f, 1);
                break;
        }
    }

    public void Freeze(Vector3 center, float time, int damage)
    {
        Collider[] hitColliders = Physics.OverlapSphere(center, 100.0f);

        // 모든 오버랩된 적에 대해 스킬 효과를 적용
        foreach (var hitCollider in hitColliders)
        {
            Enemy enemy = hitCollider.GetComponent<Enemy>();
            if (enemy != null)
            {
                StartCoroutine(FreezeEnemy(enemy, time, damage));

                // 이미 생성된 이펙트가 있다면 파괴합니다.
                if (effectInstance != null)
                {
                    Destroy(effectInstance);
                }

                // 이펙트를 enemy 위치에 생성합니다.
                effectInstance = Instantiate(effectPrefabs[1], enemy.transform.position, Quaternion.identity);
                effectInstance.transform.localScale *= 15f; // 크기를 15배로 키우기
                Destroy(effectInstance, time); // time 초 후에 이펙트를 파괴합니다.
            }
        }
    }



    IEnumerator FreezeEnemy(Enemy enemy, float time, int damage)
    {
        if (!enemy.isStop) // 이미 얼어있는 상태인지 확인
        {
            enemy.isStop = true; // 적의 움직임을 멈추게 함

            // 원하는 시간 동안 적을 얼리고
            float elapsed = 0f;
            while (elapsed < time)
            {
                elapsed += Time.deltaTime;
                enemy.currentHp -= damage * Time.deltaTime;

                // 적의 색상을 파란색으로 변경
                Renderer enemyRenderer = enemy.GetComponent<Renderer>();
                if (enemyRenderer != null)
                {
                    enemyRenderer.material.color = Color.blue;
                }

                yield return null;
            }

            enemy.isStop = false; // 적의 움직임을 다시 활성화

            // 적의 색상을 원래대로 되돌림
            Renderer originalRenderer = enemy.GetComponent<Renderer>();
            if (originalRenderer != null)
            {
                originalRenderer.material.color = Color.white;
            }
        }
    }


    public void WindSkill(int level)
    {
        Vector3 randomPosition = GetRandomEnemyPosition();
        if (randomPosition == Vector3.zero) return;

        switch (level)
        {
            case 2:
                Tornado(1.7f, 3f, 6);
                break;
            case 1:
                Tornado(1f, 1f, 5);
                break;
            case 0:
                Tornado(0.7f, 0.5f, 5);
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
                    Vector3 offsetPosition = enemy.transform.position + new Vector3(6.0f, 0f, 0f);
                    GameObject instance = Instantiate(effectPrefabs[2], offsetPosition, Quaternion.identity);
                    effectPrefabs[2].transform.localScale *= 1.5f;
                    Destroy(instance, destroyDelay); // Destroy in 2 seconds
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
                ChainLightning(150f, 10f, 6f, 7);
                break;
            case 1:
                ChainLightning(100f, 10f, 4.5f, 5);
                break;
            case 0:
                ChainLightning(60f, 10f, 1f, 1);
                break;
        }
    }

    public void ChainLightning(float distance, float LightSpeed, float time, int damage)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, distance);
        GameObject firstEnemy = null;

        // 가장 가까운 적을 찾음
        foreach (Collider col in colliders)
        {
            if (col.CompareTag("Enemy"))
            {
                firstEnemy = col.gameObject;
                break;
            }
        }

        if (firstEnemy != null)
        {
            // 첫 번째 적에게 연쇄 효과 발동
            chain(firstEnemy);
        }

        void chain(GameObject target)
        {
            Destroy(target); // 충돌된 오브젝트 파괴

            // 주변의 모든 Enemy에게 데미지를 입힘
            Collider[] nearbyColliders = Physics.OverlapSphere(target.transform.position, distance);
            foreach (Collider col in nearbyColliders)
            {
                if (col.CompareTag("Enemy"))
                {
                    col.GetComponent<Enemy>().currentHp -= damage;
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
