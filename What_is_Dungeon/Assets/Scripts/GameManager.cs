using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance {  get; private set; }

    private void Awake()
    {
        if (instance )
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            transform.parent = null;
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    // Update is called once per frame
    void Update()
    {
        if(Input.anyKeyDown)
        {
            SceneManager.LoadScene("GameScene");
        }
    }
    public void GoToGame()
    {
        SceneManager.LoadScene("PlayScene");
    }
    public void GoToShop()
    {
        SceneManager.LoadScene("ShopScene");
    }
    public void GoExit()
    {
        Application.Quit();
    }
}
