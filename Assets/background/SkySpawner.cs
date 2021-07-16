using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkySpawner : MonoBehaviour
{
    public GameObject sprite;
    private List<GameObject> _sprites;

    private void Start()
    {
        _sprites = new List<GameObject>();
        AddSprite();
        AddSprite();
        AddSprite();    
    }

    private void RemoveBackground(GameObject g)
    {
        Destroy(g);
        AddSprite();
    }

    private void AddSprite()
    {
        if (_sprites.Count == 0)
        {
            GameObject obj = Instantiate(sprite, transform.position, Quaternion.identity);
            obj.GetComponent<SkyMove>().ONRemove += RemoveBackground;
            _sprites.Add(obj);
            return;
        }

        GameObject prevSprite = _sprites[_sprites.Count-1];

        Vector2 location = prevSprite.transform.position;
        location.x += prevSprite.GetComponent<SpriteRenderer>().bounds.size.x;

        GameObject g = Instantiate(sprite, location, Quaternion.identity);

        g.GetComponent<SkyMove>().ONRemove += RemoveBackground;
        
        if (_sprites.Count % 2 == 1) g.GetComponent<SpriteRenderer>().flipX = true;
        
        _sprites.Add(g);
    }
}
