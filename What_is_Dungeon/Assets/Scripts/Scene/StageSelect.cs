using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSelect : MonoBehaviour
{
    public GameObject Forest;
    public GameObject Cave;
    public GameObject Ocean;
    public GameObject Lava;

    // Start is called before the first frame update
    void Start()
    {
        Forest.SetActive(false); 
        Cave.SetActive(false); 
        Ocean.SetActive(false); 
        Lava.SetActive(false);
    }
    
    public void SetLava()
    {
        Lava.SetActive(true);
        Forest.SetActive(false);
        Ocean.SetActive(false);
        Cave.SetActive(false);
    }

    public void SetForest()
    {
        Forest.SetActive(true);
        Cave.SetActive(false);
        Ocean.SetActive(false);
        Lava.SetActive(false);
    }


    public void SetCave()
    {
       Cave.SetActive(true);
        Forest.SetActive(false);
        Ocean.SetActive(false);
        Lava.SetActive(false);
    }

    public void SetOcean()
    {
        Ocean.SetActive(true);
        Forest.SetActive(false);
        Cave.SetActive(false);
        Lava.SetActive(false);
    }
}
