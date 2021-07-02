using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Gumball : MonoBehaviour
{
    public float speed = 5000f;
    public float stickThreshold = 150f;
    
    private Rigidbody2D _rb;

    
    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rb.velocity = new Vector2(Random.Range(0f, 1f), Random.Range(-0, 1f))* (Random.Range(3f,8f));
    }

    private void StickPlayer(GameObject player)
    {
        //Disable movement, Initialize Mash suicide
        player.AddComponent<PlayerMashHandler>().InitializeMash();
        Destroy(gameObject);
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Beam")) return;

        Vector2 dir = (transform.position - other.transform.parent.position).normalized;

        _rb.velocity = dir * speed;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (!other.transform.CompareTag("Player")) return;
 
        if (other.gameObject.GetComponent<Rigidbody2D>().velocity.magnitude + _rb.velocity.magnitude >= stickThreshold)
        {
            StickPlayer(other.gameObject);
        }
    }
}
