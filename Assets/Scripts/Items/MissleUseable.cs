using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissleUseable : MonoBehaviour, IUseable {
    // Start is called before the first frame update

    public Rigidbody2D _rb;
    public float missileSpeed = 100f;
    private GameObject userItemSpawner;
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        
    }
    public void Use (GameObject userItemSpawner) {
        //Instead of instantiating, refactor item system so that a prefab of the item
        //is parented to the player on pickup
        //use will then activate the object and the object will do what it do
    }
    
}
