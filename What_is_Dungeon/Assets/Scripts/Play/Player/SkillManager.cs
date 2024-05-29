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
    public TextMeshProUGUI[] hideSkillTimeTexts; // TextMeshProUGUI 배열로 수정
    public Image[] hideSkillImage;
    public bool[] isHideSkill = { false, false, false, false };
    private float[] skillTimes = { 5, 5, 8, 12 };
    private float[] getSkillTimes = { 0, 0, 0, 0 };

    public float gizmoRadius = 5.0f; // 기즈모 반경을 설정합니다.
    public string[] enemyTags = { "Wave1", "Wave2", "Wave3", "Boss" };

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

        //GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy"); 
        // 임시로 이걸 다 넣어 놓긴 했는데 이건 스킬 레벨 불러오는 switch문 말고 스킬 실행 하는 함수에 넣을 예정

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
        //Ice룬의 freez 스킬 관련 코드 추가
        //오브젝트 멈추게 만들고 멈추고 있는 시간 동안 도트 데미지 넣을 예정
        //이건 플레이어 앞으로 ray 쏴서 충돌한 오브젝트 태그 확인하고 같은 태그 가진 모든 오브젝트에 효과 적용하는 식으로 구현 예정
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
        //Wind룬의 tornado 스킬 관련 코드 추가
        //오브젝트 전체적으로 뒤로 밀어내며 일시적인 데미지 줄 예정
        //Ice와 마찬가지로 웨이브 통째로 적용 예정
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
        //해당 스킬 관련 코드 추가
        //일단 처음 스킬 맞은 오브젝트에서 선언 된 distance 값 만큼의 거리 내에 다른 오브젝트 있으면 스킬 튕기는 식으로 구현 할 예정
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
