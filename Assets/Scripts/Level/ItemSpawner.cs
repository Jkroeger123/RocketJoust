using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ItemSpawner : MonoBehaviour
{
    public Vector4 bounds;
    public List<GameObject> items;
    public float spawnRate = 10f;
    public int maxItems = 500000000;

    private List<GameObject> _currentItems;
    
    private float timer;

    private void Start()
    {
        _currentItems = new List<GameObject>();
    }

    private void Update()
    {
        if (timer <= 0 && _currentItems.Count <= maxItems)
        {
            SpawnItem();
            timer = spawnRate;
        }

        timer -= Time.deltaTime;
    }



    private void SpawnItem()
    {
        Vector2 spawnPosition = new Vector2(Random.Range(bounds.x, bounds.y), Random.Range(bounds.z, bounds.w));
        GameObject g = Instantiate(items[Random.Range(0, items.Count)], spawnPosition, Quaternion.identity);
        _currentItems.Add(g);
    }
}
