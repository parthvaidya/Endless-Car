using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndlessPool : MonoBehaviour
{
    [SerializeField] GameObject[] sectionPrefab;
    GameObject[] sectionPool = new GameObject[20];
    GameObject[] sections = new GameObject[20];
    Transform playerCarTransform;
    WaitForSeconds waitfor100ms = new WaitForSeconds(0.1f);

    //[SerializeField] private int coinsPerSection = 3;

    const float sectionLength = 26;
    // Start is called before the first frame update
    void Start()
    {
        playerCarTransform = GameObject.FindGameObjectWithTag("Player").transform;
        int prefabIndex = 0;

        for (int i = 0; i < sectionPool.Length; i++)
        {
            sectionPool[i] = Instantiate(sectionPrefab[prefabIndex]);
            sectionPool[i].SetActive(false);

            prefabIndex++;

            if (prefabIndex > sectionPrefab.Length - 1)
            {
                prefabIndex = 0;
            }
        }

        for (int i = 0; i < sections.Length; i++)
        {
            GameObject randomSection = GetRandomSectionFromPool();

            randomSection.transform.position = new Vector3(sectionPool[i].transform.position.x, 0, i * sectionLength);
            randomSection.SetActive(true);

            sections[i] = randomSection;
        }

        StartCoroutine(UpdateLessOften());
    }

    IEnumerator UpdateLessOften()
    {
        while (true)
        {
            UpdatePosition();
            yield return waitfor100ms;
            
        }
    }

    void UpdatePosition()
    {
        for (int i = 0; i < sections.Length; i++)
        {
            if (sections[i].transform.position.z - playerCarTransform.position.z < -sectionLength)
            {
                Vector3 lastSectionPosition = sections[i].transform.position;
                sections[i].SetActive(false);

                sections[i] = GetRandomSectionFromPool();

                sections[i].transform.position = new Vector3(lastSectionPosition.x, 0, lastSectionPosition.z + sectionLength * sections.Length);
                sections[i].SetActive(true);
                EnableCollidersInChildren(sections[i]);

                SpawnCoinsInSection(sections[i]);
                SpawnCarsInSection(sections[i]);

            }
        }

    
    }


    void SpawnCoinsInSection(GameObject section)
    {
        Transform road = section.transform.Find("Road");
        if (road == null) return;

        List<Transform> lanes = new List<Transform>();
        foreach (Transform lane in road)
        {
            lanes.Add(lane);
        }

        int coinsToSpawn = Random.Range(1, 4); // 1–3 coins per section randomly

        for (int i = 0; i < coinsToSpawn; i++)
        {
            if (Random.value > 0.5f) continue;

            Transform randomLane = lanes[Random.Range(0, lanes.Count)];

            float randomXOffset = Random.Range(-5f, 5f); // horizontal variation
            float randomZOffset = Random.Range(-20f, 20f);   // depth variation

            Vector3 spawnPos = randomLane.position + new Vector3(randomXOffset, 1f, randomZOffset);

            GameObject coin = CoinPooler.Instance.GetCoin();
            coin.transform.position = spawnPos;
            coin.transform.SetParent(section.transform);
            coin.SetActive(true);
        }
    }

    //void SpawnCarsInSection(GameObject section)
    //{
    //    Transform road = section.transform.Find("Road");
    //    if (road == null) return;

    //    List<Transform> lanes = new List<Transform>();
    //    foreach (Transform lane in road)
    //    {
    //        lanes.Add(lane);
    //    }

    //    // 30% chance to spawn a car in this section
    //    if (Random.value > 0.3f) return;

    //    Transform randomLane = lanes[Random.Range(0, lanes.Count)];

    //    float randomZOffset = Random.Range(-20f, 20f);
    //    float randomXOffset = Random.Range(-8f, 8f);

    //    Vector3 spawnPos = randomLane.position + new Vector3(randomXOffset, 0.5f, randomZOffset);

    //    GameObject car = CarObstaclePooler.Instance.GetCar();
    //    car.transform.position = spawnPos;
    //    car.transform.SetParent(section.transform);
    //    car.SetActive(true);
    //}

    void SpawnCarsInSection(GameObject section)
    {
        Transform road = section.transform.Find("Road");
        if (road == null) return;

        // 30% chance to spawn a car in this section
        if (Random.value > 0.3f) return;

        // We pass the base road position to the factory, and it handles X/Z offsets
        VehicleFactory.Instance.SpawnRandomVehicle(road.position, section.transform);
    }
    void EnableCollidersInChildren(GameObject section)
    {
        Collider[] colliders = section.GetComponentsInChildren<Collider>(true);
        foreach (Collider col in colliders)
        {
            col.enabled = true;
            col.gameObject.SetActive(true); // Just in case the child GameObject is disabled
        }
    }

    GameObject GetRandomSectionFromPool()
    {
        int randomIndex = Random.Range(0, sectionPool.Length);

        bool isNewSectionFound = false;

        while (!isNewSectionFound)
        {
            if (!sectionPool[randomIndex].activeInHierarchy)
                isNewSectionFound = true;
            else
            {
                randomIndex++;

                if (randomIndex > sectionPool.Length - 1)
                    randomIndex = 0;
            }
        }

        return sectionPool[randomIndex];
    }
}
