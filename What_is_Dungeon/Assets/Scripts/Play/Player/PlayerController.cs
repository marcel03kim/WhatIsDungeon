using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int Hp = 3;
    public GameObject hp1;
    public GameObject hp2;
    public GameObject hp3;
    

    // Update is called once per frame
    void Update()
    {
        switch (Hp)
        {
            case 3:
                hp3.SetActive(true);
                hp2.SetActive(true);
                hp1.SetActive(true);
                break;
            case 2:
                hp3.SetActive(false);
                hp2.SetActive(true);
                hp1.SetActive(true);
                break;
            case 1:
                hp3.SetActive(false);
                hp2.SetActive(false);
                hp1.SetActive(true);
                break;
            case 0:
                hp3.SetActive(false);
                hp2.SetActive(false);
                hp1.SetActive(false);
                Die();
                break;
        }
            
    }

    void Die()
    {

    }
}
