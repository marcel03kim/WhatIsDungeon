using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController Instance;            //½Ì±ÛÅæ ÄÚµå
    
    public GameObject Canvas_Inventory;
    public GameObject Canvas_Stage;
    public GameObject Canvas_Forest;
    public GameObject Canvas_Cave;
    public GameObject Canvas_Ocean;
    public GameObject Canvas_Lava;
    public GameObject Canvas_Main;
    public GameObject Canvas_Book;
    public GameObject Canvas_Setting;


    
    public void Start()
    {
        Canvas_Main.SetActive(true);
        Canvas_Stage.SetActive(false);
        Canvas_Inventory.SetActive(false);
        Canvas_Book.SetActive(false);
        Canvas_Setting.SetActive(false);
        Canvas_Setting.SetActive(false);
    }

    public void OpenBook()
    {
        Canvas_Book.SetActive(true);
        Canvas_Main.SetActive(false);
        Canvas_Inventory.SetActive(false);
        Canvas_Stage.SetActive(false);
    }
    public void GoToGame()
    {
        Loading.LoadScene("Demo");
    }

    public void GoToStageScene()
    {
        Loading.LoadScene("Stage");
    }
    public void GoToShop()
    {
        Loading.LoadScene("ShopScene");
    }
    public void GoToMain()
    {
        Start();
    }
    
    public void GoToGameScene()
    {
        Loading.LoadScene("GameScene");
    }

    
    public void GoToStage()
    {
        Canvas_Stage.SetActive(true);
        Canvas_Main.SetActive(false);
        Canvas_Inventory.SetActive(false);
        Canvas_Book.SetActive(false);
    }
    public void GoToInventory()
    {
        Canvas_Stage.SetActive(false);
        Canvas_Main.SetActive(false);
        Canvas_Inventory.SetActive(true);
        Canvas_Book.SetActive(false);
    }

    
    public void ToggleSetting()
    {
        Canvas_Setting.SetActive(true);
    }
    
    public void OutSetting()
    {
        Canvas_Setting.SetActive(false);

    }

    public void GoExit()
    {
        Application.Quit();
    }

}
