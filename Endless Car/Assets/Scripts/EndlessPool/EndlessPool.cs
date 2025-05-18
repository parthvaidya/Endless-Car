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

                sections[i].transform.position = new Vector3(lastSectionPosition.x, 0, lastSectionPosition.z + sectionLength * sections.Length); //change to 0  -100 if required
                sections[i].SetActive(true);

            }
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
