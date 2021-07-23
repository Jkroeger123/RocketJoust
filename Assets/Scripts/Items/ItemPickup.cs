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
        //calls attempt set item. Also checks if it is possible before destroying pickup
        if (other.gameObject.GetComponent<PlayerItemManager>().CanSetItem(itemPrefab, gameObject)) {
            gameObject.SetActive(false);
        }
    }
}
