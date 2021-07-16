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
        StartCoroutine(BackgroundTimer());
    }

    private IEnumerator BackgroundTimer()
    {
        while (true)
        {
            AddSprite();
            yield return new WaitForSeconds(5f);
        }
    }

    private void AddSprite()
    {
        if (_sprites.Count == 0)
        {
            _sprites.Add(Instantiate(sprite, transform.position, Quaternion.identity));
            return;
        }

        GameObject prevSprite = _sprites[_sprites.Count-1];

        Vector2 location = prevSprite.transform.position;
        location.x += prevSprite.GetComponent<SpriteRenderer>().bounds.size.x;

        GameObject g = Instantiate(sprite, location, Quaternion.identity);

        if (_sprites.Count % 2 == 1) g.GetComponent<SpriteRenderer>().flipX = true;
        
        _sprites.Add(g);
    }
}
