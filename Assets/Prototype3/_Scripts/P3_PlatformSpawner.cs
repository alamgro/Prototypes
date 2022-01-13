using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P3_PlatformSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] pfbsPlatform;
    [SerializeField] private int initPlatformAmount;
    [SerializeField] private float minSpawnHeight, maxSpawnHeight;
    [SerializeField] private GameObject environmentObject;
    
    private float lastPlatformHeight = 0f;
    private List<GameObject> poolPlatforms;

    void Start()
    {
        P3_GameManager.Instance.platformSpawner = this;

        poolPlatforms = new List<GameObject>();

        for (int i = 0; i < initPlatformAmount; i++)
        {
            SpawnPlatform();
        }
    }

    private void SpawnPlatform2()
    {
        //Choose random platform
        int randIndex = Random.Range(0, pfbsPlatform.Length);
        //Random position
        Vector2 spawnPosition;
        spawnPosition.x = Random.Range(-P3_GameManager.Instance.camHorizontalExtents, P3_GameManager.Instance.camHorizontalExtents);
        spawnPosition.y = lastPlatformHeight += Random.Range(minSpawnHeight, maxSpawnHeight);
        //Store last height where a platform was spawned
        lastPlatformHeight = spawnPosition.y;
        //Spawn object
        Instantiate(pfbsPlatform[randIndex], spawnPosition, Quaternion.identity);
    }

    public void SpawnPlatform()
    {
        //Choose random platform
        int randIndex = Random.Range(0, pfbsPlatform.Length);
        //Random position
        Vector2 spawnPosition;
        spawnPosition.x = Random.Range(-P3_GameManager.Instance.camHorizontalExtents, P3_GameManager.Instance.camHorizontalExtents);
        spawnPosition.y = lastPlatformHeight += Random.Range(P3_GameManager.Instance.CurrentMinSpawnHeight, maxSpawnHeight);
        //Store last height where a platform was spawned
        lastPlatformHeight = spawnPosition.y;

        //Use object pool to either instatiate or activate a platform
        for (int i = 0; i < poolPlatforms.Count; i++)
        {
            if (!poolPlatforms[i].activeSelf)
            {
                poolPlatforms[i].SetActive(true);
                poolPlatforms[i].transform.position = spawnPosition;
                return;
            }
        }

        //Instantiate object
        GameObject tempGO = Instantiate(pfbsPlatform[randIndex], spawnPosition, Quaternion.identity);
        if(environmentObject)
            tempGO.transform.SetParent(environmentObject.transform);

        //Add object to the object pool
        poolPlatforms.Add(tempGO);
    }

}
