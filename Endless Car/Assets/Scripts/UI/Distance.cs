using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Distance : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI distanceTravelled;
    [SerializeField] Car playerCarTransform;
    // Start is called before the first frame update
    void Start()
    {
        playerCarTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Car>();
    }

    // Update is called once per frame
    void Update()
    {
        distanceTravelled.text = playerCarTransform.DistanceTravelled.ToString("0000000") + " km";
    }
}
