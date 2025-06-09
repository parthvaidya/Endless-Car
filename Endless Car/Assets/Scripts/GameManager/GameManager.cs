using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public event Action<int> OnCoinCollected;
    public event Action OnGameStarted;
    public event Action OnGameOver;
    public event Action OnCarExploded;

    private int totalCoins = 0;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void StartGame()
    {
        OnGameStarted?.Invoke();
    }

    public void EndGame()
    {
        OnGameOver?.Invoke();
    }

    public void CoinCollected()
    {
        totalCoins++;
        OnCoinCollected?.Invoke(totalCoins);
    }

    public void CarExploded()
    {
        OnCarExploded?.Invoke();
        EndGame();
    }

    public int GetTotalCoins() => totalCoins;
}
