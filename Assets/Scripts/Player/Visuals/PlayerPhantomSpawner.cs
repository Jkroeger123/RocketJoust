using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPhantomSpawner : MonoBehaviour
{
    private float _spawnSpeed = 0.05f;
    
    private void Start()
    {
        PlayerController.ONBlast += OnBlast;
    }

    private void OnBlast(GameObject player)
    {
        if (player != gameObject) return;
        StartCoroutine(SpawnPhantom(player));
    }

    private IEnumerator SpawnPhantom(GameObject player)
    {
        PlayerController pc = player.GetComponent<PlayerController>();
        WaitForSeconds wait = new WaitForSeconds(_spawnSpeed);
        
        while (pc.isThooming)
        {
            CreatePhantomImage(player);
            yield return wait;
        }

    }

    private void CreatePhantomImage(GameObject player)
    {
        GameObject g = new GameObject("PhantomImage");
        g.transform.position = player.transform.position;
        g.transform.rotation = player.transform.rotation;

        SpriteRenderer spriteRenderer = g.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = player.GetComponent<SpriteRenderer>().sprite;

        g.AddComponent<PhantomImage>();
    }

    private void OnDestroy()
    {
        PlayerController.ONBlast -= OnBlast;
    }
}
