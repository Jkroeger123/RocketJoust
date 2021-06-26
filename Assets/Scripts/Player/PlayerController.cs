using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using TMPro;
using Unity.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D _rb;
    private PlayerInputController _input;

    public float _rotationSpeed = 250f; 
    public float velocityPerThrust = 1f, thrusterMaxVelocity = 10f; 
    public float rocketMaxVelocity = 30f, thoomTime = 0.5f, thoomSlowdownDuration = 1f;
    private float maxVelocity;

    private bool isThooming = false;

    private void Awake()
    {
        _rb = gameObject.GetComponent<Rigidbody2D>();
        _input = gameObject.GetComponent<PlayerInputController>();
        _input.onRocketPress += OnRocket;
        _input.onThrusterPress += OnThruster;
        maxVelocity = thrusterMaxVelocity;
    }

    
    private void Update()
    {
        if(_input.rotateLeftPressed) RotateLeft();
        if(_input.rotateRightPressed) RotateRight();
        //clamp velocity to max velocity
        _rb.velocity = Vector2.ClampMagnitude(_rb.velocity, maxVelocity);
    }

    private void OnThruster()
    {
        //little poof
        //tiny velocity increase on every press
        //low velocity cieling
        //if (_rb.velocity )
        _rb.velocity += (Vector2)transform.up * velocityPerThrust;
    }

    private void OnRocket() {
        if (isThooming) return;
        //big thoom
        //immmediately hits max velocity
        //launches attack
        SetIsThooming(true);
        maxVelocity = rocketMaxVelocity;
        StartCoroutine(Thoom());
    }

    private void RotateRight () {
        _rb.rotation -= _rotationSpeed * Time.deltaTime;
    }

    private void RotateLeft () {
        _rb.rotation += _rotationSpeed * Time.deltaTime;
    }

    private IEnumerator Thoom() {
        _rb.velocity = transform.up * rocketMaxVelocity;
        yield return new WaitForSeconds(thoomTime);
        Tween tween = DOTween.To(() => 
                maxVelocity, value => maxVelocity = value, thrusterMaxVelocity, thoomSlowdownDuration)
            .OnComplete(() => SetIsThooming(false));
    }
    
    private void SetIsThooming (bool b) {
        isThooming = b;
    }

    private void OnDestroy () {
        _input.onRocketPress -= OnRocket;
        _input.onThrusterPress -= OnThruster;
    }
}
