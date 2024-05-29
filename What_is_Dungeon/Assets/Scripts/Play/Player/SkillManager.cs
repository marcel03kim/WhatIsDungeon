using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class SkillManager : MonoBehaviour
{
    public GameObject[] hideSkillButton;
    public GameObject[] textPros;
    public TextMeshProUGUI[] hideSkillTimeTexts; // TextMeshProUGUI �迭�� ����
    public Image[] hideSkillImage;
    public bool[] isHideSkill = { false, false, false, false };
    private float[] skillTimes = { 5, 5, 8, 12 };
    private float[] getSkillTimes = { 0, 0, 0, 0 };

    public float gizmoRadius = 5.0f; // ����� �ݰ��� �����մϴ�.
    public string[] enemyTags = { "Wave1", "Wave2", "Wave3", "Boss" };

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

        //GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy"); 
        // �ӽ÷� �̰� �� �־� ���� �ߴµ� �̰� ��ų ���� �ҷ����� switch�� ���� ��ų ���� �ϴ� �Լ��� ���� ����

    public void FireSkill(int level)
    {
        GameObject[] enemies = FindGameObjectsWithTags(enemyTags);
        float radius = 5.0f;
        foreach (var enemy in enemies)
        {
            Vector3 enemyPosition = enemy.transform.position;

            switch (level)
            {
                case 2:
                    Meteor(100, enemyPosition, radius);
                    break;
                case 1:
                    Meteor(60, enemyPosition, radius);
                    break;
                case 0:
                    Meteor(30, enemyPosition, radius);
                    break;
            }
        }
    }

    public void Meteor(int damage, Vector3 center, float radius)
    {
        Collider[] hitColliders = Physics.OverlapSphere(center, radius);
        foreach (var hitCollider in hitColliders)
        {
            hitCollider.GetComponent<Enemy>().hp -= damage;
        }
    }
    public void IceSkill(int level)
    {
        GameObject[] enemies = FindGameObjectsWithTags(enemyTags);

        switch (level)
        {
            case 2:
                freez(5f, 10);
                break;
            case 1:
                freez(3f, 10);
                break;
            case 0:
                freez(1.5f, 10);
                break;
        }
    }

    public void freez(float time, int damage)
    {
        //Ice���� freez ��ų ���� �ڵ� �߰�
        //������Ʈ ���߰� ����� ���߰� �ִ� �ð� ���� ��Ʈ ������ ���� ����
        //�̰� �÷��̾� ������ ray ���� �浹�� ������Ʈ �±� Ȯ���ϰ� ���� �±� ���� ��� ������Ʈ�� ȿ�� �����ϴ� ������ ���� ����
    }
    public void WindSkill(int level)
    {
        GameObject[] enemies = FindGameObjectsWithTags(enemyTags);

        switch (level)
        {
            case 2:
                tornado(10f, 50);
                break;
            case 1:
                tornado(7f, 30);
                break;
            case 0:
                tornado(3f, 10);
                break;
        }
    }

    public void tornado(float power, int damage)
    {
        //Wind���� tornado ��ų ���� �ڵ� �߰�
        //������Ʈ ��ü������ �ڷ� �о�� �Ͻ����� ������ �� ����
        //Ice�� ���������� ���̺� ��°�� ���� ����
    }

    public void LightningSkill(int level)
    {
        GameObject[] enemies = FindGameObjectsWithTags(enemyTags);

        switch (level)
        {
            case 2:
                chainLightning(150f, 50);
                break;
            case 1:
                chainLightning(100f, 25);
                break;
            case 0:
                chainLightning(60f, 10);
                break;
        }
    }

    public void chainLightning(float distance, int damage)
    {
        //�ش� ��ų ���� �ڵ� �߰�
        //�ϴ� ó�� ��ų ���� ������Ʈ���� ���� �� distance �� ��ŭ�� �Ÿ� ���� �ٸ� ������Ʈ ������ ��ų ƨ��� ������ ���� �� ����
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        GameObject[] enemies = FindGameObjectsWithTags(enemyTags);
        foreach (var enemy in enemies)
        {
            Vector3 enemyPosition = enemy.transform.position;
            Gizmos.DrawWireSphere(enemyPosition, gizmoRadius);
        }
    }
}
