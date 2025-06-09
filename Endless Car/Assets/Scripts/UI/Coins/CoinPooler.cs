using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPooler : MonoBehaviour
{
    public static CoinPooler Instance;

    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private int poolSize = 50;

    private List<GameObject> coinPool = new List<GameObject>();

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject coin = Instantiate(coinPrefab);
            coin.SetActive(false);
            coinPool.Add(coin);
        }
    }

    public GameObject GetCoin()
    {
        foreach (var coin in coinPool)
        {
            if (!coin.activeInHierarchy)
                return coin;
        }

        
        GameObject newCoin = Instantiate(coinPrefab);
        newCoin.SetActive(false);
        coinPool.Add(newCoin);
        return newCoin;
    }
}
