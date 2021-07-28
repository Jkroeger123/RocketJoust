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
    public List<Sprite> sprites;
    
    private float _timer = 0.05f;
    private Rigidbody2D _rb;

    private void Start()
    {
        GetComponent<SpriteRenderer>().sprite = sprites[Random.Range(0, sprites.Count)];
        _rb = GetComponent<Rigidbody2D>();
        _rb.velocity = new Vector2(Random.Range(0f, 1f), Random.Range(-0, 1f))* (Random.Range(3f,8f));
    }

    private void StickPlayer(GameObject player)
    {
        //Disable movement, Initialize Mash suicide
        PlayerMashHandler mash = player.AddComponent<PlayerMashHandler>();
        mash.InitializeMash();
        mash.ShowGum();
        Destroy(gameObject);
    }

    private void Update()
    {
        _rb.velocity = Vector2.ClampMagnitude(_rb.velocity, speed);

        if (_rb.velocity.magnitude >= stickThreshold)
        {
            if (_timer <= 0)
            {
                CreatePhantomImage(gameObject);
                _timer = 0.05f;
            }
        }
        else
        {
            GetComponent<SpriteRenderer>().color = Color.white;
        }

        _timer -= Time.deltaTime;
    }

    private void CreatePhantomImage(GameObject item)
    {
        GameObject g = new GameObject("PhantomImage");
        g.transform.position = item.transform.position;
        g.transform.rotation = item.transform.rotation;

        SpriteRenderer spriteRenderer = g.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = item.GetComponent<SpriteRenderer>().sprite;

        g.AddComponent<PhantomImage>();
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
 
        if (other.gameObject.GetComponent<PlayerController>().isThooming || _rb.velocity.magnitude >= stickThreshold)
        {
            StickPlayer(other.gameObject);
        }
    }
}
