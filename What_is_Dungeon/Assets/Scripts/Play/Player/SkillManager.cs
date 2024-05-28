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
    // Scene���� Ư�� �±׿� ������ ���� ������Ʈ�� �ִ��� Ȯ���ϴ� �޼���
    public bool CheckObject(string tag, int level)
    {
        // ���� Scene�� �ִ� ��� ���� ������Ʈ�� �����ɴϴ�.
        GameObject[] allObjects = SceneManager.GetActiveScene().GetRootGameObjects();

        // ������ ��� ���� ������Ʈ�� �ݺ��ϸ鼭 Ư�� �±׿� ������ ���� ������Ʈ�� �ִ��� Ȯ���մϴ�.
        foreach (GameObject obj in allObjects)
        {
            // �ش� ���� ������Ʈ�� �±װ� �����Ǿ� �ִ��� Ȯ���մϴ�.
            if (obj.CompareTag(tag))
            {
                // �ش� ���� ������Ʈ�� �ش� ������ ���� ������Ʈ�� �ִ��� Ȯ���մϴ�.
                RuneManager runeManager = obj.GetComponent<RuneManager>();
                if (runeManager != null && runeManager.runeTag == tag && runeManager.runeLevel == level)
                {
                    // �±׿� ������ ��ġ�ϴ� ������Ʈ�� ã�����Ƿ� true�� ��ȯ�մϴ�.
                    return true;
                }
            }
        }

        // ��� ���� ������Ʈ�� Ȯ�������� ��ġ�ϴ� ������Ʈ�� ã�� �������Ƿ� false�� ��ȯ�մϴ�.
        return false;
    }
}