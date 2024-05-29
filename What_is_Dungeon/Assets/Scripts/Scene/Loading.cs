using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;                             //UI �̹����� �����ϱ� ���� �ڵ�
using UnityEngine.SceneManagement;                //Scene�� �����ϱ� ���� �ڵ�

public class Loading : MonoBehaviour
{
    public static string nextScene;             //������ �ҷ� �� Scene�� ���� �ڵ�

    [SerializeField] Image progressBar;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LoadScene());
    }

    public static void LoadScene(string sceneName)
    {
        nextScene = sceneName;
        SceneManager.LoadScene("LoadingScene");
        Debug.Log(sceneName);
    }

    IEnumerator LoadScene()
    {
        yield return null;                           //LoadingScene�� �ҷ����� ���� �ڵ�
        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);
        op.allowSceneActivation = false;                    //Scene ��ȯ �ڵ�
        float timer = 0.0f;
        float fillSpeed = 0.5f;

        while (!op.isDone)                              //ProgressBar ���� �ڵ�
        {
            yield return null;
            timer += Time.deltaTime;

            if (op.progress < 0.9f)
            {
                progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, op.progress, timer * fillSpeed);
                if (progressBar.fillAmount >= op.progress)
                {
                    timer = 0f;
                }
            }
            else
            {
                progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, 1f, timer * fillSpeed);
                if (progressBar.fillAmount == 1.0f)
                {
                    yield return new WaitForSeconds(2.0f);    // 2�� ���� ����ũ �ε�
                    op.allowSceneActivation = true;          // 2�ʰ� ������ Scene ��ȯ
                    break;
                }
            }
        }
    }

}