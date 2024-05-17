using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;                             //UI 이미지에 접근하기 위한 코드
using UnityEngine.SceneManagement;                //Scene에 접근하기 위한 코드

public class Loading : MonoBehaviour
{
    public static string nextScene;             //다음에 불러 올 Scene을 위한 코드

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
    }

    IEnumerator LoadScene()
    {
        yield return null;                           //LoadingScene을 불러오기 위한 코드
        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);
        op.allowSceneActivation = false;                    //Scene 전환 코드
        float timer = 0.0f;

        while (!op.isDone)                              //ProgressBar 관련 코드
        {
            yield return null;
            timer += Time.deltaTime;

            if (op.progress < 0.9f)
            {
                progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, op.progress, timer);
                if (progressBar.fillAmount >= op.progress)
                {
                    timer = 0f;
                }
            }
            else
            {
                progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, 1f, timer);
                if (progressBar.fillAmount == 1.0f)
                {
                    yield return new WaitForSeconds(2.0f);    // 2초 동안 페이크 로딩
                    op.allowSceneActivation = true;          // 2초가 끝나면 Scene 전환
                    break;
                }
            }
        }
    }

}