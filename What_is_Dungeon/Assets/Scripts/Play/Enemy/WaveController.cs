using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveController : MonoBehaviour
{
    public GameObject wave1;
    public GameObject wave2;
    public GameObject wave3;

    // Start is called before the first frame update
    void Start()
    {
        wave1.SetActive(true); 
        wave2.SetActive(false);
        wave3.SetActive(false);
    }

    // Update is called once per frame
    private void Update()
    {
        if (Time.deltaTime >= 15f)
        {
            wave2.SetActive(true);
        }
        else if (Time.deltaTime >= 30f)
        {
            wave3.SetActive(true);
        }

        if(transform.childCount < 1)
        {
            gameObject.SetActive(false);
        }
    }
}
