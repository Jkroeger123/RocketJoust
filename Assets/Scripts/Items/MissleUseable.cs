using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class MissleUseable : MonoBehaviour, IUseable {
    //TODO
    // potentially implement slowdown and speed up
    // implement deflection
    // implement on-hit effects and fx

    public Rigidbody2D _rb;
    public float missileSpeed = 100f;
    public float rotateSpeed = 5f;
    private GameObject user;
    private Collider2D missileCollider;
    private GameObject[] targets;

    public float angleThreshold;

    private GameObject currentTarget;

    void Start() {
        AcquireTarget();
    }

    // Update is called once per frame
    void FixedUpdate() {
        HomeInOnTarget();
        _rb.velocity = missileSpeed * transform.up;
    }
    public void Use (GameObject user) {
        this.user = user;
        gameObject.SetActive(true);
        _rb = gameObject.GetComponent<Rigidbody2D>();
        targets = GameObject.FindGameObjectsWithTag("Player");
    }

    private void OnCollisionEnter2D (Collision2D other) {
        if (other.gameObject == user) return;

        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.AddComponent<PlayerMashHandler>().InitializeMash();
        }

        Destroy(gameObject);
    }

    private void AcquireTarget () {
        foreach (GameObject target in targets) {
            if (target == user) continue;
            
            Vector2 playerToEnemyLine = (target.transform.position - user.transform.position);
            float angle = Vector2.Angle(transform.up, playerToEnemyLine.normalized);
           
            if (angle <= angleThreshold) {
                currentTarget = target;
                
            } else {
                currentTarget = null;
            }
        }
    }
    
    private void HomeInOnTarget () {
        if (currentTarget == null) return;
        Vector2 direction = ((Vector2)currentTarget.transform.position - _rb.position).normalized;
        float rotateAmount = Vector3.Cross(direction, transform.up).z;
        _rb.angularVelocity = -rotateAmount * rotateSpeed;
    }
}
