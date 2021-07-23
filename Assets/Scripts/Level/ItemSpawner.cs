using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class ItemSpawner : MonoBehaviour
{
    public Vector4 bounds;
    public List<GameObject> items;
    public float spawnRate = 5f;
    public int maxItems = 10;

    private GameObject _itemContainer;
    private float _timer;

    private void Start()
    {
        _itemContainer = new GameObject("Items");
    }

    private void Update()
    {
        if (_timer <= 0 && _itemContainer.transform.childCount < maxItems)
        {
            SpawnItem();
            _timer = spawnRate;
        }

        _timer -= Time.deltaTime;
    }



    private void SpawnItem()
    {
        Vector2 spawnPosition = new Vector2(Random.Range(bounds.x, bounds.y), Random.Range(bounds.z, bounds.w));
        GameObject g = Instantiate(items[Random.Range(0, items.Count)], spawnPosition, Quaternion.identity);
        g.transform.parent = _itemContainer.transform;
    }
}
