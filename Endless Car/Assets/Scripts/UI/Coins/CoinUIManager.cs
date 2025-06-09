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

    //[SerializeField] private TextMeshProUGUI coinText;

    //private void OnEnable()
    //{
    //    GameManager.Instance.OnCoinCollected += UpdateUI;
    //}

    //private void OnDisable()
    //{
    //    GameManager.Instance.OnCoinCollected -= UpdateUI;
    //}

    //private void UpdateUI(int totalCoins)
    //{
    //    coinText.text = "Coins: " + totalCoins;
    //}
}
