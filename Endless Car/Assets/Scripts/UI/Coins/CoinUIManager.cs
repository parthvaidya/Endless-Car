using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoinUIManager : MonoBehaviour
{
    public static CoinUIManager Instance;

    [SerializeField] private TextMeshProUGUI coinText;

    private int coinCount = 0;

    private void Awake()
    {
        Instance = this;
    }

    public void AddCoin()
    {
        coinCount++;
        coinText.text = "Coins: " + coinCount;
    }
}
