using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;            //싱글톤 코드
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject tempObj = GameObject.Find("GameManager");
                if (tempObj != null)
                {
                    instance = tempObj.GetComponent<GameManager>();
                }
                else
                {
                    instance = new GameObject("GameManager").AddComponent<GameManager>();
                }

                DontDestroyOnLoad(instance);
            }
            return instance;
        }
    }

    public GameObject Canvas_Stage;
    public GameObject Canvas_Main;

    
    public void Start()
    {
        Canvas_Main.SetActive(true);
        Canvas_Stage.SetActive(false);
    }
    public void GoToGame()
    {
        Loading.LoadScene("PlayScene");
    }
    public void GoToShop()
    {
        Loading.LoadScene("ShopScene");         //Loading.LoadScene을 사용해야 Scene과 Scene 사이에 로딩 씬이 로드 됨
        //SceneManager.LoadScene("GameScene");
    }
     public void GoToStage()
     {
        Canvas_Stage.SetActive(true);
        Canvas_Main.SetActive(false);
     }
    public void GoExit()
    {
        Application.Quit();
    }
}
