using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public string thisScene;
    public GameObject Canvas_Fail;
    public GameObject Canvas_Pause;
    public GameObject Canvas_Clear;

    public GameObject wave1;
    public GameObject wave2;
    public GameObject wave3;
    public GameObject Boss;

    public GameObject Player;
    public GameObject star1;
    public GameObject star2;
    public GameObject star3;

    public Text timeText;
    public Text clearText;
    public Text recordText;
    public GameObject NewRecord;
    public float bestTime;
    public bool isGameClear;

    public int clearPoint;
    public int maxClearPoint;

    [SerializeField] private float playTime;


   

    // Start is called before the first frame update
    void Start()
    {
        maxClearPoint = 15;
        Player.GetComponent<PlayerController>();

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
            clearText.text = "Clear : " + clearPoint + "/" + maxClearPoint;
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


        if (clearPoint >= maxClearPoint)
        {
            isGameClear = true;
            Clear();
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

        switch (Player.GetComponent<PlayerController>().Hp)
        {
            case 3:
                star3.SetActive(true);
                star2.SetActive(true);
                star1.SetActive(true);
                break;
            case 2:
                star3.SetActive(false);
                star2.SetActive(true);
                star1.SetActive(true);
                break;
            case 1:
                star3.SetActive(false);
                star2.SetActive(false);
                star1.SetActive(true);
                break;
            case 0:
                star3.SetActive(false);
                star2.SetActive(false);
                star1.SetActive(false);
                break;
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

    public void Setting()
    {
        SceneController.Instance.Canvas_Setting.SetActive(true);

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
