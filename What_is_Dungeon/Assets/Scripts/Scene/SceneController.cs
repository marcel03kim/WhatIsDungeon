using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    private static SceneController instance;            //½Ì±ÛÅæ ÄÚµå
    public static SceneController Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject tempObj = GameObject.Find("SceneController");
                if (tempObj != null)
                {
                    instance = tempObj.GetComponent<SceneController>();
                }
                else
                {
                    instance = new GameObject("SceneController").AddComponent<SceneController>();
                }

                DontDestroyOnLoad(instance);
            }
            return instance;
        }
    }

    public GameObject Canvas_Stage;
    public GameObject Canvas_Main;
    public GameObject Canvas_Book;
    public GameObject Canvas_Shop;



    public void Start()
    {
        Canvas_Main.SetActive(true);
        Canvas_Stage.SetActive(false);
        Canvas_Shop.SetActive(false);
        Canvas_Book.SetActive(false);
    }

    public void OpenBook()
    {
        Canvas_Book.SetActive(true);
        Canvas_Main.SetActive(false);
        Canvas_Shop.SetActive(false);
        Canvas_Stage.SetActive(false);
    }
    public void GoToGame()
    {
        Loading.LoadScene("Stage");
    }
    public void GoToShop()
    {
        Canvas_Shop.SetActive(true);
        Canvas_Stage.SetActive(false);
        Canvas_Main.SetActive(false);
        Canvas_Book.SetActive(false);
    }
    public void GoToStage()
    {
        Canvas_Stage.SetActive(true);
        Canvas_Main.SetActive(false);
        Canvas_Shop.SetActive(false);
        Canvas_Book.SetActive(false);
    }
    public void GoExit()
    {
        Application.Quit();
    }
}
