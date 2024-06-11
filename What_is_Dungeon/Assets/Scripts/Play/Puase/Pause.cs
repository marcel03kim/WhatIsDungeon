using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{

    private static Pause instance;
    public static Pause Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<Pause>();
                if (instance == null)
                {
                    GameObject tempObj = new GameObject("Pause");
                    instance = tempObj.AddComponent<Pause>();
                }

                DontDestroyOnLoad(instance);
            }
            return instance;
        }
    }

    public GameObject settingsCanvas;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void TogglePause()
    {
        if (settingsCanvas != null)
        {
            if (Time.timeScale == 1)
            {
                Time.timeScale = 0;
                settingsCanvas.SetActive(true);
            }
            else
            {
                Time.timeScale = 1;
                settingsCanvas.SetActive(false);
            }
        }
    }
}
