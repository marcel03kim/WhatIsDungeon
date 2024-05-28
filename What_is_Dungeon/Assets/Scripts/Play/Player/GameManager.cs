using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public string thisScene;
    public GameObject Canvas_Fail;
    public GameObject Canvas_Pause;
    public GameObject Canvas_Clear;

    public Text timeText;
    public Text recordText;
    public GameObject NewRecord;
    public float bestTime;
    public bool isGameClear;

    

    [SerializeField] private float playTime;

    // Start is called before the first frame update
    void Start()
    {
        isGameClear = false;
        playTime = 0;

        Canvas_Pause.SetActive(false);
        Canvas_Fail.SetActive(false);
        Canvas_Clear.SetActive(false);

        bestTime = PlayerPrefs.GetFloat("BestTime");
    }

    // Update is called once per frame
    private void Update()
    {
        if (!isGameClear)
        {
            playTime += Time.deltaTime;
            timeText.text = "Time : " + (int)playTime;
        }
    }
    public void Stage()
    {
        Loading.LoadScene("Stage");
    }
    public void ReStart()
    {
        Loading.LoadScene(thisScene);
    }
    public void Clear()
    {
        isGameClear = true;
        bestTime = PlayerPrefs.GetFloat("BestTime");
        Canvas_Clear.SetActive(true);
        if (playTime < bestTime)
        {
            bestTime = playTime;
            PlayerPrefs.SetFloat("BestTime", bestTime);
            NewRecord.SetActive(true);
        }
        else
        {
            NewRecord.SetActive(false);
        }

        recordText.text = "BestTime : " + (int)playTime;
    }
    public void Fail()
    {
        Canvas_Fail.SetActive(true);
    }

    public void Main()
    {
        Start();
        Loading.LoadScene("Main_scene");
    }

    public void Pause()
    {
        Canvas_Pause.SetActive(true);
        thisScene = SceneManager.GetActiveScene().name;
        Time.timeScale = 0f;
    }

    public void Continue()
    {
        Canvas_Pause.SetActive(false);
        Time.timeScale = 1f;
    }
}
