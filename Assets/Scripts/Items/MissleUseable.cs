using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using DG.Tweening;
using MoreMountains.Tools;
using Sirenix.Utilities;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class MissleUseable : MonoBehaviour, IUseable {
    //TODO
    // potentially implement slowdown and speed up
    // implement on-hit effects and fx

    public GameObject eraserPrefab;
    public GameObject explosionParticle;
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
        GameObject o = Instantiate(eraserPrefab, transform.GetChild(1).position, transform.rotation);
        o.transform.position = new Vector3(transform.position.x, transform.position.y, 0f);
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

        StartCoroutine(IgnoreCollisionForSeconds(0.5f, GetComponent<Collider2D>(), user.gameObject.GetComponent<Collider2D>()));
        
        _rb = gameObject.GetComponent<Rigidbody2D>();
        
        targets = GameObject.FindGameObjectsWithTag("Player");
        
        targets.Sort((o, o1) => 
            Vector2.Distance(o.transform.position, transform.position).
                CompareTo(Vector2.Distance(o1.transform.position, transform.position)));
    }

    private IEnumerator IgnoreCollisionForSeconds(float t, Collider2D c1, Collider2D c2)
    {

        if (c1 == null || c2 == null) yield break;
        Physics2D.IgnoreCollision(c1, c2, true);
        yield return new WaitForSeconds(t);
        
        if(c1 == null || c2 == null) yield break;
        Physics2D.IgnoreCollision(c1, c2, false);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {

        if (other.gameObject.CompareTag("Chunk")) return;
        
        if (other.gameObject == user) return;

        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.AddComponent<PlayerMashHandler>().InitializeMash();
        }

        Instantiate(explosionParticle, transform.position, Quaternion.identity);
        
        Destroy(transform.GetChild(1).gameObject, 3f);
        transform.GetChild(1).parent = null;
        
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag("Beam")) return;
        OnRedirect(other.gameObject.transform.parent.parent.gameObject);
    }


    private void OnRedirect(GameObject redirector)
    {
        transform.rotation = Quaternion.FromToRotation(Vector3.up, -redirector.transform.up);
        user = redirector;
        currentTarget = null;
        AcquireTarget();
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
