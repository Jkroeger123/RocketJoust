using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemManager : MonoBehaviour {
    public GameObject activeItem;
    private PlayerInputController _input;
    public GameObject itemSpawner;

    void Start() {
        _input = gameObject.GetComponent<PlayerInputController>();
        _input.ONUseItem += UseItem;
    }
    
    void Update()
    {
        
    }

    public bool CanSetItem (GameObject itemPrefab) {
        if (activeItem != null) return false;
        SetItem(itemPrefab);
        return true;
    }

    private void SetItem (GameObject itemPrefab) {
        activeItem = Instantiate(itemPrefab, itemSpawner.transform);
        activeItem.transform.parent = gameObject.transform;
        activeItem.SetActive(false);
    }

    private void UseItem () {
        if (activeItem == null) return;
        activeItem.gameObject.GetComponent<IUseable>().Use(gameObject);
        activeItem = null;
    }
}
