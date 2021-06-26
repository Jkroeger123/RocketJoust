using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D _rb;
    private PlayerInputController _input;
    
    private float _rotationSpeed = 100f;

    private void Start()
    {
        _rb = gameObject.GetComponent<Rigidbody2D>();
        _input = gameObject.GetComponent<PlayerInputController>();
        
        //subscribe to button pressed
        _input.onRocketPress += OnRocket;
        _input.onThrusterPress += OnThruster;
    }

    
    private void Update()
    {
        if(_input.rotateLeftPressed) Debug.Log("Left Pressed");
        if(_input.rotateRightPressed) Debug.Log("Right Pressed");
    }

    private void OnThruster()
    {
        //TODO
        Debug.Log("Thruster Pressed");
    }

    private void OnRocket()
    {
        //TODO
        Debug.Log("Rocket Pressed");
    }
}
