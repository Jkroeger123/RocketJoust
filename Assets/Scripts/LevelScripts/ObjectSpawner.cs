using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    [SerializeField]
    GameObject[] SpawnableObjects;

    [SerializeField]
    int[] rarities;

    [SerializeField]
    float minDistanceBetweenObjects, maxDistanceBetweenObjects, maxHeightBetweenObjects;

    [SerializeField] [Range(0, 100)]
    float minPercent, maxPercent;

    [SerializeField]
    int maxObjectsSpawned = 30;

    float minHeight, maxHeight;

    GameObject lastSpawnedObject;

    //distance from center of sprite to end of sprite
    float distToEnd;

    float nextSpawnDist, nextSpawnHeight;

    void Start()
    {
        CalculateSpawnRange();
        lastSpawnedObject = Instantiate(GetRandomObject(), new Vector3(transform.position.x, transform.position.y, 0), Quaternion.identity, transform);
        CalculateNextSpawn();
    }

    GameObject GetRandomObject() 
    {
        if (rarities.Length == 0) { return SpawnableObjects[Random.Range(0, SpawnableObjects.Length)]; }

        int sumOfWeights = 0;

        for (int i = 0; i < rarities.Length; i++) 
        {
            sumOfWeights += rarities[i];
        }

        int rand = Random.Range(0, sumOfWeights);

        for (int i = 0; i < rarities.Length; i++)
        {
            rand -= rarities[i];

            if (rand < 0) 
            {
                return SpawnableObjects[i];
            }

        }

        return null;

    }

    void CalculateSpawnRange()
    {
        maxHeight = Camera.main.ScreenToWorldPoint(new Vector2(0, Screen.height * (maxPercent / 100))).y;
        minHeight = Camera.main.ScreenToWorldPoint(new Vector2(0, Screen.height * (minPercent / 100))).y;
    }

    void Update()
    {
        CheckObjectCap();
    }

    void SpawnObject(GameObject obj) 
    {
        CalculateNextSpawn();
        lastSpawnedObject = Instantiate(obj, new Vector3(nextSpawnDist, nextSpawnHeight, 0), Quaternion.identity, transform);
    }

    void CalculateNextSpawn() 
    {
        CalculateNextObjectDist();
        CalculateNextObjectHeight();
        CalculateObjectLength();
    }

    void CalculateObjectLength() 
    {
        BoxCollider2D collider = lastSpawnedObject.GetComponent<BoxCollider2D>();
        distToEnd = collider != null ? collider.size.x : 0;
    }

    void CalculateNextObjectDist() 
    {
        nextSpawnDist = lastSpawnedObject.transform.position.x + Random.Range(minDistanceBetweenObjects, maxDistanceBetweenObjects) + distToEnd;
    }

    void CalculateNextObjectHeight() 
    {
        //if our car is going to go above the screen, spawn below
        if (lastSpawnedObject.transform.position.y + maxHeightBetweenObjects > maxHeight)
        {
            nextSpawnHeight = Random.Range(lastSpawnedObject.transform.position.y - maxHeightBetweenObjects, lastSpawnedObject.transform.position.y);
        }
        //if our car is going below the screen, spawn above
        else if (lastSpawnedObject.transform.position.y -  maxHeightBetweenObjects < minHeight)
        {
            nextSpawnHeight = Random.Range(lastSpawnedObject.transform.position.y, lastSpawnedObject.transform.position.y + maxHeightBetweenObjects);
        }
        //otherwise, spawn inbetween min and max height
        else 
        {
            nextSpawnHeight = Random.Range(lastSpawnedObject.transform.position.y - maxHeightBetweenObjects, lastSpawnedObject.transform.position.y + maxHeightBetweenObjects);
        }
    }

    //only spawns the next object when there is less then a specified amount of objects spawned
    void CheckObjectCap()
    {
        if (transform.childCount <= maxObjectsSpawned) 
        {
            GameObject spwn = GetRandomObject();
            if (spwn != null) { SpawnObject(spwn); } 
        }
    }   

}
