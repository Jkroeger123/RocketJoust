using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PlayerFaceController : MonoBehaviour {
    
    public GameObject facePrefab;
    public float velocityThreshold;
    
    private float followSpeed = 0.4f;
    private Rigidbody2D rb;
    private GameObject face;
    
    private void Start() {
        face = Instantiate(facePrefab, transform.position, Quaternion.identity);
        rb = GetComponent<Rigidbody2D>();
    }

 
    private void Update() {

        followSpeed = Mathf.Approximately(rb.velocity.magnitude, GetComponent<PlayerController>().rocketMaxVelocity) ? (float)0.4: 1f;

        face.transform.position = Vector2.Lerp(face.transform.position, transform.position, followSpeed);
        face.transform.rotation = transform.rotation;
    } 


    private void OnDestroy () 
    {
        Destroy(face);
    }
}
