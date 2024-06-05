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

    public GameObject wave1;
    public GameObject wave2;
    public GameObject wave3;
    public GameObject Boss;


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

        wave1.SetActive(false);
        wave2.SetActive(false);
        wave3.SetActive(false);
        Boss.SetActive(false);

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
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Canvas_Pause.SetActive(true);
            Pause();
        }
        if(playTime >= 4f)
        {
            wave1.SetActive(true);
        }
        if (playTime >= 15f)
        {
            wave2.SetActive(true);
        }

        if (playTime >= 30f)
        {
            wave3.SetActive(true);
        }
    }
    
   
    public void ReStart()
    {
        Time.timeScale = 1f;
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
        Time.timeScale = 1f;
        Loading.LoadScene("GameScene");
    }

    public void Pause()
    {
        Canvas_Pause.SetActive(true);
        thisScene = SceneManager.GetActiveScene().name;
        Time.timeScale = 0f;

        Rigidbody[] rigidbodies = FindObjectsOfType<Rigidbody>();
        foreach (Rigidbody rb in rigidbodies)
        {
            rb.isKinematic = true;
        }
    }

    public void Continue()
    {
        Canvas_Pause.SetActive(false);
        Time.timeScale = 1f;

        Rigidbody[] rigidbodies = FindObjectsOfType<Rigidbody>();
        foreach (Rigidbody rb in rigidbodies)
        {
            rb.isKinematic = false;
        }
    }
}
