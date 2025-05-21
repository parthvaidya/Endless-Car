using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleFactory : MonoBehaviour
{
    public static VehicleFactory Instance;

    [SerializeField] private GameObject[] vehiclePrefabs;
    [SerializeField] private float[] laneXPositions = new float[] { -7f, 0.5f, 7f };

    private void Awake()
    {
        Instance = this;
    }

    public GameObject SpawnRandomVehicle(Vector3 basePosition, Transform parent = null)
    {
        if (vehiclePrefabs.Length == 0)
        {
            Debug.LogWarning("No vehicle prefabs assigned in VehicleFactory.");
            return null;
        }

        GameObject prefab = vehiclePrefabs[Random.Range(0, vehiclePrefabs.Length)];
        float randomLaneX = laneXPositions[Random.Range(0, laneXPositions.Length)];
        float randomZOffset = Random.Range(-30f, 35f);

        Vector3 spawnPos = new Vector3(randomLaneX, basePosition.y + 0.5f, basePosition.z + randomZOffset);
        GameObject vehicle = Instantiate(prefab, spawnPos, Quaternion.identity);

        if (parent != null)
            vehicle.transform.SetParent(parent);

        return vehicle;
    }
}
