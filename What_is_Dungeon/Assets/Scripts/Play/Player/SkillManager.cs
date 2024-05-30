using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillManager : MonoBehaviour
{
    public GameObject[] hideSkillButton;
    public GameObject[] textPros;
    public TextMeshProUGUI[] hideSkillTimeTexts; // TextMeshProUGUI 배열로 수정
    public Image[] hideSkillImage;
    public bool[] isHideSkill = { false, false, false, false };
    private float[] skillTimes = { 5, 5, 8, 12 };
    private float[] getSkillTimes = { 0, 0, 0, 0 };

    public float gizmoRadius = 5.0f; // 기즈모 반경을 설정합니다.
    public string[] enemyTags = { "Enemy", "Wave1", "Wave2", "Wave3", "Boss" };

    private List<Vector3> meteorPositions = new List<Vector3>(); // 메테오 위치를 저장할 리스트
    public float meteorRadius = 5.0f;

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
            else
            {
                Debug.LogError("hideSkillButton[" + i + "] is not set.");
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
                else
                {
                    Debug.LogError("No Button component found on hideSkillButton[" + skillNum + "]");
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
                Meteor(100, randomPosition, radius);
                break;
            case 1:
                Meteor(60, randomPosition, radius);
                break;
            case 0:
                Meteor(30, randomPosition, radius);
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
                enemy.hp -= damage;
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
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 50.0f);
        foreach (var hitCollider in hitColliders)
        {
            Enemy enemy = hitCollider.GetComponent<Enemy>();
            if (enemy != null)
            {
                StartCoroutine(FreezeEnemy(enemy, time, damage));
            }
        }
    }

    IEnumerator FreezeEnemy(Enemy enemy, float time, int damage)
    {
        if (!enemy.isFrozen) // 이미 얼어있는 상태인지 확인
        {
            enemy.isFrozen = true; // 적의 움직임을 멈추게 함

            float elapsed = 0f;
            while (elapsed < time)
            {
                elapsed += Time.deltaTime;
                enemy.hp -= damage * Time.deltaTime;

                yield return null;
            }

            enemy.isFrozen = false; // 적의 움직임을 다시 활성화
        }
    }

    public void WindSkill(int level)
    {
        switch (level)
        {
            case 2:
                Tornado(10f, 50);
                break;
            case 1:
                Tornado(7f, 30);
                break;
            case 0:
                Tornado(3f, 10);
                break;
        }
    }

    public void Tornado(float power, int damage)
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 10.0f); // 임시로 10.0f 반경을 설정
        foreach (var hitCollider in hitColliders)
        {
            Enemy enemy = hitCollider.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.GetComponent<Rigidbody>().AddForce(Vector3.back * power, ForceMode.Impulse); // 적을 뒤로 밀어냄
                enemy.hp -= damage;
            }
        }
    }

    public void LightningSkill(int level)
    {
        switch (level)
        {
            case 2:
                ChainLightning(150f, 50);
                break;
            case 1:
                ChainLightning(100f, 25);
                break;
            case 0:
                ChainLightning(60f, 10);
                break;
        }
    }

    public void ChainLightning(float distance, int damage)
    {
        // Lightning룬의 ChainLightning 스킬 관련 코드 추가
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
