using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillManager : MonoBehaviour
{
    public GameObject[] hideSkillButton;
    public GameObject[] textPros;
    public TextMeshProUGUI[] hideSkillTimeTexts;
    public Image[] hideSkillImage;
    public bool[] isHideSkill = { false, false, false, false };
    private float[] skillTimes = { 5, 5, 8, 12 };
    private float[] getSkillTimes = { 0, 0, 0, 0 };

    public float gizmoRadius = 5.0f;
    public string[] enemyTags = { "Enemy", "Wave1", "Wave2", "Wave3", "Boss" };
    private Enemy[] enemies;

    private void Start()
    {
        hideSkillTimeTexts = new TextMeshProUGUI[textPros.Length];

        for (int i = 0; i < textPros.Length; i++)
        {
            hideSkillTimeTexts[i] = textPros[i].GetComponent<TextMeshProUGUI>();
            if (hideSkillButton[i] != null)
            {
                hideSkillButton[i].SetActive(false);
            }
            else
            {
                Debug.LogError("hideSkillButton[" + i + "] is not set.");
            }
        }

        enemies = FindObjectsOfType<Enemy>();
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
        button.interactable = false;

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
                    button.interactable = true;
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
        // Ice룬의 Freeze 스킬 관련 코드 추가
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
        // Wind룬의 Tornado 스킬 관련 코드 추가
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

    private void ActivateAllChildren(GameObject parent)
    {
        foreach (Transform child in parent.transform)
        {
            child.gameObject.SetActive(true);
            ActivateAllChildren(child.gameObject);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        if (enemies == null)
        {
            return;
        }

        foreach (var enemy in enemies)
        {
            if (enemy != null)
            {
                Vector3 enemyPosition = enemy.transform.position;
                Gizmos.DrawWireSphere(enemyPosition, gizmoRadius);
            }
        }
    }

    public void ActivateWave1()
    {
        GameObject wave1 = GameObject.Find("Wave1");
        if (wave1 != null)
        {
            wave1.SetActive(true);
            ActivateAllChildren(wave1);
        }
    }
}
