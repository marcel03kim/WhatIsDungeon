using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinManager : MonoBehaviour
{
    public static CoinManager instance;
    public static CoinManager Instance
    { get { return instance; } }
    public int coin;
    public Text coinText;

    public void Update()
    {
        coinText.text = " : " + coin;
    }
}
