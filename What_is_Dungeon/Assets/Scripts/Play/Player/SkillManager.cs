using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SkillManager : MonoBehaviour
{
    void FireSkill()
    {

    }
    void IceSkill()
    {

    }
    void WIndSkill()
    {

    }
    void LightningSkill()
    {

    }
}
public class ObjectChecker : MonoBehaviour
{
    // Scene에서 특정 태그와 레벨을 가진 오브젝트가 있는지 확인하는 메서드
    public bool CheckObject(string tag, int level)
    {
        // 현재 Scene에 있는 모든 게임 오브젝트를 가져옵니다.
        GameObject[] allObjects = SceneManager.GetActiveScene().GetRootGameObjects();

        // 가져온 모든 게임 오브젝트를 반복하면서 특정 태그와 레벨을 가진 오브젝트가 있는지 확인합니다.
        foreach (GameObject obj in allObjects)
        {
            // 해당 게임 오브젝트에 태그가 부착되어 있는지 확인합니다.
            if (obj.CompareTag(tag))
            {
                // 해당 게임 오브젝트에 해당 레벨을 가진 컴포넌트가 있는지 확인합니다.
                RuneManager runeManager = obj.GetComponent<RuneManager>();
                if (runeManager != null && runeManager.runeTag == tag && runeManager.runeLevel == level)
                {
                    // 태그와 레벨이 일치하는 오브젝트를 찾았으므로 true를 반환합니다.
                    return true;
                }
            }
        }

        // 모든 게임 오브젝트를 확인했지만 일치하는 오브젝트를 찾지 못했으므로 false를 반환합니다.
        return false;
    }
}