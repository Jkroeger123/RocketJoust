using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using DG.Tweening;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class MissleUseable : MonoBehaviour, IUseable {
    //TODO
    // potentially implement slowdown and speed up
    // implement deflection
    // implement on-hit effects and fx
    // Sort target array by distance

    public Rigidbody2D _rb;
    public float missileSpeed = 100f;
    public float rotateSpeed = 5f;
    private GameObject user;
    private Collider2D missileCollider;
    private GameObject[] targets;

    public float angleThreshold;

    private GameObject currentTarget;

    void Start()
    {
        currentTarget = null;
        AcquireTarget();
        transform.parent = null;
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

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject == user) return;

        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.AddComponent<PlayerMashHandler>().InitializeMash();
        }

        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag("Beam")) return;
        
        Destroy(gameObject);
    }


    private void OnRedirect(GameObject redirector)
    {
        transform.DORotate(redirector.transform.rotation.eulerAngles, 0.3f, RotateMode.Fast).OnComplete(() =>
        {
            
            user = redirector.transform.parent.parent.gameObject;
            AcquireTarget();
        });
    }

    private void AcquireTarget () {
        foreach (GameObject target in targets) {
            if (target == user) continue;
            
            Vector2 playerToEnemyLine = (target.transform.position - user.transform.position);
            float angle = Vector2.Angle(transform.up, playerToEnemyLine.normalized);
           
            if (angle <= angleThreshold) {
                currentTarget = target;
                return;
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
