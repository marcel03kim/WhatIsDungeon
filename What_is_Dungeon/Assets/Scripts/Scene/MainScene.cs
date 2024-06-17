using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainScene : MonoBehaviour
{
    public bool isFirst;

    void Start()
    {
        if (PlayerPrefs.HasKey("isFirst"))
        {
            isFirst = PlayerPrefs.GetInt("isFirst") == 1;
        }
        else
        {
            isFirst = true;
            PlayerPrefs.SetInt("isFirst", isFirst ? 1 : 0);
        }
    }
    private void Update()
    {
        if(Input.anyKeyDown)
        {
            if(isFirst)
            {
                Loading.LoadScene("Tutorial");
            }
            else
            {
                Loading.LoadScene("GameScene");
            }
        }
    }
    void OnApplicationQuit()
    {
        PlayerPrefs.SetInt("isFirst", isFirst ? 1 : 0);
    }
    void OnDisable()
    {
        PlayerPrefs.SetInt("isFirst", isFirst ? 1 : 0);
    }
}
