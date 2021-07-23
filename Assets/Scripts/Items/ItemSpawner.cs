using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class ItemSpawner : MonoBehaviour
{
    //TODO: make this dynamic for level size
    public Vector4 bounds;
    public List<ItemEntry> items;
    public float minSpawnRate = 5f;
    public float maxSpawnRate = 10f;

    private BattleManager _battleManager;
    private GameObject _itemContainer;
    private float _timer;
    private Dictionary<ItemEntry, List<GameObject>> _curItemsSpawned;

    private void Start()
    {
        _battleManager = FindObjectOfType<BattleManager>();
        _itemContainer = new GameObject("Items");
        _timer =  GetNextSpawnRate();
        _curItemsSpawned = new Dictionary<ItemEntry, List<GameObject>>();
        InitializeCurItemsSpawned();
    }

    private void InitializeCurItemsSpawned()
    {
        foreach (ItemEntry itemEntry in items)
        {
            _curItemsSpawned.Add(itemEntry, new List<GameObject>());
        }
    }

    //TODO: make this more effecient
    private void UpdateCurrentItemsSpawned()
    {
        for (int i = 0; i < items.Count; i++)
        {
            for (int j = 0; j < _curItemsSpawned[items[i]].Count; j++)
            {
                if (_curItemsSpawned[items[i]][j] == null)
                {
                    _curItemsSpawned[items[i]].Remove(_curItemsSpawned[items[i]][j]);
                }
            }
        }
    }
    
    
    private float GetNextSpawnRate() => Random.Range(minSpawnRate, maxSpawnRate);
    
    private void Update()
    {
        if (_timer <= 0)
        {
            SpawnItem();
            _timer = GetNextSpawnRate();
        }

        _timer -= Time.deltaTime;
        UpdateCurrentItemsSpawned();
    }
    

    private void SpawnItem()
    {
        int randItem = Random.Range(0, items.Count);

        int maxItems = (int) (items[randItem].maxPerPlayer * _battleManager.GetPlayers().Count);

        if (_curItemsSpawned[items[randItem]].Count > (maxItems - 1)) return;

        Vector2 spawnPosition = new Vector2(Random.Range(bounds.x, bounds.y), Random.Range(bounds.z, bounds.w));
        
        GameObject g = Instantiate(items[randItem].prefab, 
            spawnPosition, Quaternion.identity);
        
        _curItemsSpawned[items[randItem]].Add(g);
        
        g.transform.parent = _itemContainer.transform;
    }
}

[System.Serializable]
public class ItemEntry
{
    public GameObject prefab;
    public float maxPerPlayer;
}
