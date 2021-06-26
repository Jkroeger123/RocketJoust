using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputController : MonoBehaviour
{
    private PlayerInput _playerInput;

    private InputAction rotateRight;
    private InputAction rotateLeft;
    private InputAction thrusterPress;
    private InputAction rocketPress;

    [NonSerialized]
    public bool rotateRightPressed;
    
    [NonSerialized]
    public bool rotateLeftPressed;

    public delegate void OnPress();

    public event OnPress onThrusterPress;
    public event OnPress onRocketPress;
    
    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();

        rotateRight = _playerInput.currentActionMap.FindAction("RotateRight");
        rotateLeft = _playerInput.currentActionMap.FindAction("RotateLeft");
        thrusterPress = _playerInput.currentActionMap.FindAction("Thruster");
        rocketPress = _playerInput.currentActionMap.FindAction("Rocket");

        rotateRight.started += context => rotateRightPressed = true;
        rotateRight.canceled += context => rotateRightPressed = false;
        
        rotateLeft.started += context => rotateLeftPressed = true;
        rotateLeft.canceled += context => rotateLeftPressed = false;

        thrusterPress.performed += context => onThrusterPress?.Invoke();
        rocketPress.performed += context => onRocketPress?.Invoke();
    }
    
    

}
