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
    public TextMeshProUGUI[] hideSkillTimeTexts; // TextMeshProUGUI �迭�� ����
    public Image[] hideSkillImage;
    public bool[] isHideSkill = { false, false, false, false };
    private float[] skillTimes = { 5, 5, 8, 12 };
    private float[] getSkillTimes = { 0, 0, 0, 0 };

    public string[] enemyTags = { "Enemy", "Wave1", "Wave2", "Wave3", "Boss" };

    private List<Vector3> meteorPositions = new List<Vector3>(); // ���׿� ��ġ�� ������ ����Ʈ
    public float meteorRadius;

    public GameObject[] effectPrefabs;
    public float destroyDelay = 2f;
    private GameObject effectInstance;

    private void Start()
    {
        hideSkillTimeTexts = new TextMeshProUGUI[textPros.Length]; // �迭 �ʱ�ȭ

        for (int i = 0; i < textPros.Length; i++)
        {
            hideSkillTimeTexts[i] = textPros[i].GetComponent<TextMeshProUGUI>(); // GetComponent�� TextMeshProUGUI ��������
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
        button.interactable = false; // ��ư ��Ȱ��ȭ

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
                    button.interactable = true; // ��ư Ȱ��ȭ
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

        // ��� �������� ���� ���� ��ų ȿ���� ����
        foreach (var hitCollider in hitColliders)
        {
            Enemy enemy = hitCollider.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.currentHp -= damage;

                // �̹� ������ ����Ʈ�� �ִٸ� �ı��մϴ�.
                if (effectInstance != null)
                {
                    Destroy(effectInstance);
                }

                // ����Ʈ�� enemy ��ġ�� �����մϴ�.
                effectInstance = Instantiate(effectPrefabs[0], enemy.transform.position, Quaternion.identity);
                effectInstance.transform.localScale *= radius; // radius�� ���� ũ�� ����
                Destroy(effectInstance, destroyDelay); // destroyDelay �� �Ŀ� ����Ʈ�� �ı��մϴ�.
            }
        }
    }

    private void ResetEnemyColor()
    {
        // ��� Enemy�� ������ ������� �ǵ����ϴ�.
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

        // ��� �������� ���� ���� ��ų ȿ���� ����
        foreach (var hitCollider in hitColliders)
        {
            Enemy enemy = hitCollider.GetComponent<Enemy>();
            if (enemy != null)
            {
                StartCoroutine(FreezeEnemy(enemy, time, damage));

                // �̹� ������ ����Ʈ�� �ִٸ� �ı��մϴ�.
                if (effectInstance != null)
                {
                    Destroy(effectInstance);
                }

                // ����Ʈ�� enemy ��ġ�� �����մϴ�.
                effectInstance = Instantiate(effectPrefabs[1], enemy.transform.position, Quaternion.identity);
                effectInstance.transform.localScale *= 15f; // ũ�⸦ 15��� Ű���
                Destroy(effectInstance, time); // time �� �Ŀ� ����Ʈ�� �ı��մϴ�.
            }
        }
    }



    IEnumerator FreezeEnemy(Enemy enemy, float time, int damage)
    {
        if (!enemy.isStop) // �̹� ����ִ� �������� Ȯ��
        {
            enemy.isStop = true; // ���� �������� ���߰� ��

            // ���ϴ� �ð� ���� ���� �󸮰�
            float elapsed = 0f;
            while (elapsed < time)
            {
                elapsed += Time.deltaTime;
                enemy.currentHp -= damage * Time.deltaTime;

                // ���� ������ �Ķ������� ����
                Renderer enemyRenderer = enemy.GetComponent<Renderer>();
                if (enemyRenderer != null)
                {
                    enemyRenderer.material.color = Color.blue;
                }

                yield return null;
            }

            enemy.isStop = false; // ���� �������� �ٽ� Ȱ��ȭ

            // ���� ������ ������� �ǵ���
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
                    enemyRigidbody.AddForce(Vector3.left * power, ForceMode.Impulse); // ���� �ڷ� �о
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
        if (!enemy.isStop) // �̹� ����ִ� �������� Ȯ��
        {
            enemy.isStop = true; // ���� �������� ���߰� ��

            float elapsed = 0f;
            while (elapsed < time)
            {
                elapsed += Time.deltaTime;

                yield return null;
            }

            enemy.isStop = false; // ���� �������� �ٽ� Ȱ��ȭ
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

        // ���� ����� ���� ã��
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
            // ù ��° ������ ���� ȿ�� �ߵ�
            chain(firstEnemy);
        }

        void chain(GameObject target)
        {
            Destroy(target); // �浹�� ������Ʈ �ı�

            // �ֺ��� ��� Enemy���� �������� ����
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
