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

    public void AttemptSetItem (GameObject itemPrefab) {
        if (activeItem != null) return;
        SetItem(itemPrefab);
    }

    private void SetItem (GameObject itemPrefab) {
        activeItem = itemPrefab;
    }

    private void UseItem () {
        activeItem.gameObject.GetComponent<IUseable>().Use(itemSpawner);
    }
}
