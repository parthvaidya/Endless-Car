using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleFactory : MonoBehaviour
{
    //public static VehicleFactory Instance;

    //[SerializeField] private GameObject[] vehiclePrefabs;
    //[SerializeField] private float[] laneXPositions = new float[] { -7f, 0.5f, 7f };

    //private void Awake()
    //{
    //    Instance = this;
    //}

    //public GameObject SpawnRandomVehicle(Vector3 basePosition, Transform parent = null)
    //{
    //    if (vehiclePrefabs.Length == 0)
    //    {
    //        Debug.LogWarning("No vehicle prefabs assigned in VehicleFactory.");
    //        return null;
    //    }

    //    GameObject prefab = vehiclePrefabs[Random.Range(0, vehiclePrefabs.Length)];
    //    float randomLaneX = laneXPositions[Random.Range(0, laneXPositions.Length)];
    //    float randomZOffset = Random.Range(-30f, 35f);

    //    Vector3 spawnPos = new Vector3(randomLaneX, basePosition.y + 0.5f, basePosition.z + randomZOffset);
    //    GameObject vehicle = Instantiate(prefab, spawnPos, Quaternion.identity);

    //    if (parent != null)
    //        vehicle.transform.SetParent(parent);

    //    return vehicle;
    //}

    public static VehicleFactory Instance;

    [SerializeField] private GameObject[] vehiclePrefabs;
    [SerializeField] private float[] laneXPositions = new float[] { -7f, 0.5f, 7f };
    [SerializeField] private float minZDistanceBetweenVehicles = 15f;

    // Dictionary: laneX -> list of recent Z positions
    private Dictionary<float, List<float>> laneZPositions = new Dictionary<float, List<float>>();

    private void Awake()
    {
        Instance = this;

        // Initialize dictionary
        foreach (float laneX in laneXPositions)
        {
            laneZPositions[laneX] = new List<float>();
        }
    }

    public GameObject SpawnRandomVehicle(Vector3 basePosition, Transform parent = null)
    {
        if (vehiclePrefabs.Length == 0)
        {
            Debug.LogWarning("No vehicle prefabs assigned in VehicleFactory.");
            return null;
        }

        // Try up to 10 times to find a valid spawn lane
        for (int attempt = 0; attempt < 10; attempt++)
        {
            float randomLaneX = laneXPositions[Random.Range(0, laneXPositions.Length)];
            float randomZOffset = Random.Range(-30f, 35f);
            float spawnZ = basePosition.z + randomZOffset;

            // Check if too close to another vehicle in the same lane
            bool tooClose = false;
            foreach (float z in laneZPositions[randomLaneX])
            {
                if (Mathf.Abs(z - spawnZ) < minZDistanceBetweenVehicles)
                {
                    tooClose = true;
                    break;
                }
            }

            if (tooClose)
                continue;

            // Valid spawn position
            Vector3 spawnPos = new Vector3(randomLaneX, basePosition.y + 0.5f, spawnZ);
            GameObject prefab = vehiclePrefabs[Random.Range(0, vehiclePrefabs.Length)];
            GameObject vehicle = Instantiate(prefab, spawnPos, Quaternion.identity);

            if (parent != null)
                vehicle.transform.SetParent(parent);

            // Track spawn position
            laneZPositions[randomLaneX].Add(spawnZ);

            // Optional cleanup: only keep last 10 spawns per lane
            if (laneZPositions[randomLaneX].Count > 10)
                laneZPositions[randomLaneX].RemoveAt(0);

            return vehicle;
        }

        // Failed to spawn after attempts
        Debug.Log("No valid lane found to spawn vehicle without overlapping.");
        return null;
    }
}
