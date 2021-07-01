using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject itemPrefab;

    private void OnCollisionEnter2D (Collision2D other) {
        if (!other.gameObject.CompareTag("Player")) return;
        other.gameObject.GetComponent<PlayerItemManager>().AttemptSetItem(itemPrefab);
        Destroy(gameObject);
    }
}
