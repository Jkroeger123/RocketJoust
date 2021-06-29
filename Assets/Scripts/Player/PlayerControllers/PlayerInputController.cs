using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputController : MonoBehaviour
{
    private PlayerInput _playerInput;

    private InputAction rotateRight;
    private InputAction rotateRightSuper;
    private InputAction rotateLeft;
    private InputAction rotateLeftSuper;
    private InputAction thrusterPress;
    private InputAction rocketPress;

    [NonSerialized]
    public bool rotateRightPressed;
    
    [NonSerialized]
    public bool rotateLeftPressed;

    [NonSerialized] 
    public bool rotateRightSuperPressed;
    
    [NonSerialized] 
    public bool rotateLeftSuperPressed;

    public delegate void OnPress();

    public event OnPress ONThrusterPress;
    public event OnPress ONRocketPress;
    
    private void Awake()
    {
        _playerInput = GetComponentInParent<PlayerInput>();
        
        _playerInput.SwitchCurrentActionMap("Gameplay");
        
        rotateRight = _playerInput.currentActionMap.FindAction("RotateRight");
        rotateLeft = _playerInput.currentActionMap.FindAction("RotateLeft");
        thrusterPress = _playerInput.currentActionMap.FindAction("Thruster");
        rocketPress = _playerInput.currentActionMap.FindAction("Rocket");
        rotateRightSuper = _playerInput.currentActionMap.FindAction("RotateRightSuper");
        rotateLeftSuper = _playerInput.currentActionMap.FindAction("RotateLeftSuper");
        
        rotateRight.started += context => rotateRightPressed = true;
        rotateRight.canceled += context => rotateRightPressed = false;
        
        rotateRightSuper.started += context => rotateRightSuperPressed = true;
        rotateRightSuper.canceled += context => rotateRightSuperPressed = false;
        
        rotateLeftSuper.started += context => rotateLeftSuperPressed = true;
        rotateLeftSuper.canceled += context => rotateLeftSuperPressed = false;
        
        rotateLeft.started += context => rotateLeftPressed = true;
        rotateLeft.canceled += context => rotateLeftPressed = false;

        thrusterPress.performed += context => ONThrusterPress?.Invoke();
        rocketPress.performed += context => ONRocketPress?.Invoke();
    }



}
