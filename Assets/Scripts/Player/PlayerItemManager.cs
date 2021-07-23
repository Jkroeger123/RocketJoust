using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemManager : MonoBehaviour {
    
    public GameObject itemSpawner;
    public GameObject activeItem;

    private GameObject _pickedUpItem;
    private PlayerInputController _input;

    public static event Action<GameObject> ONItemPickup; 

    void Start() {
        _input = gameObject.GetComponent<PlayerInputController>();
        _input.ONUseItem += UseItem;
    }

    public bool CanSetItem (GameObject itemPrefab, GameObject pickedUpItem) {
        if (activeItem != null) return false;
        
        SetItem(itemPrefab);
        _pickedUpItem = pickedUpItem;
        
        transform.parent.GetComponent<Player>().GetPlayerHUD().SetItemSprite(_pickedUpItem.GetComponent<SpriteRenderer>().sprite);
        
        ONItemPickup?.Invoke(gameObject);
        return true;
    }

    private void SetItem (GameObject itemPrefab) {
        activeItem = Instantiate(itemPrefab, itemSpawner.transform);
        activeItem.transform.parent = gameObject.transform;
        activeItem.SetActive(false);
    }

    private void UseItem () {
        if (activeItem == null) return;
        
        Destroy(_pickedUpItem);
        transform.parent.GetComponent<Player>().GetPlayerHUD().RemoveItem();

        activeItem.gameObject.GetComponent<IUseable>().Use(gameObject);
        activeItem = null;
    }
}
