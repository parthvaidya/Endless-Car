using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarObstaclePooler : MonoBehaviour
{
    public static CarObstaclePooler Instance;

    [SerializeField] GameObject carPrefab;
    [SerializeField] int poolSize = 10;

    Queue<GameObject> carPool = new Queue<GameObject>();

    void Awake()
    {
        Instance = this;

        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(carPrefab);
            obj.SetActive(false);
            carPool.Enqueue(obj);
        }
    }

    public GameObject GetCar()
    {
        if (carPool.Count == 0)
        {
            GameObject newCar = Instantiate(carPrefab);
            return newCar;
        }

        GameObject car = carPool.Dequeue();
        car.SetActive(true);
        return car;
    }

    public void ReturnCar(GameObject car)
    {
        car.SetActive(false);
        carPool.Enqueue(car);
    }
}
