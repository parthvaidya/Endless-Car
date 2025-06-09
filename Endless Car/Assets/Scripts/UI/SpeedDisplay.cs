using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpeedDisplay : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI speedText;
    [SerializeField] Car playerCar;

    void Start()
    {
        if (playerCar == null)
        {
            playerCar = GameObject.FindGameObjectWithTag("Player").GetComponent<Car>();
        }
    }

    void Update()
    {
        float speed = playerCar.GetCurrentSpeed();
        speedText.text = speed.ToString("000") + " km/h";
    }
}
