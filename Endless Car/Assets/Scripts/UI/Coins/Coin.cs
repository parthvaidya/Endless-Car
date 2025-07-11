using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameObject.SetActive(false);
            CoinUIManager.Instance.AddCoin(); // Update UI
            //GameManager.Instance.CoinCollected();
            SoundManager.Instance.PlayCoin();
        }
    }
}
