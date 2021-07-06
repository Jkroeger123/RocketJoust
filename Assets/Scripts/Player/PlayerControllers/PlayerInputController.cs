using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputController : MonoBehaviour
{
    private PlayerInput _playerInput;

    private InputAction _rotateRight;
    private InputAction _rotateRightSuper;
    private InputAction _rotateLeft;
    private InputAction _rotateLeftSuper;
    private InputAction _thrusterPress;
    private InputAction _rocketPress;
    private InputAction _useItem;

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
    public event OnPress ONUseItem;
    
    private void Awake()
    {
        _playerInput = GetComponentInParent<PlayerInput>();
        InitializeGameplayControls();
    }

    public void InitializeGameplayControls()
    {
        _playerInput.SwitchCurrentActionMap("Gameplay");
        
        _rotateRight = _playerInput.currentActionMap.FindAction("RotateRight");
        _rotateLeft = _playerInput.currentActionMap.FindAction("RotateLeft");
        _thrusterPress = _playerInput.currentActionMap.FindAction("Thruster");
        _rocketPress = _playerInput.currentActionMap.FindAction("Rocket");
        _rotateRightSuper = _playerInput.currentActionMap.FindAction("RotateRightSuper");
        _rotateLeftSuper = _playerInput.currentActionMap.FindAction("RotateLeftSuper");
        _useItem = _playerInput.currentActionMap.FindAction("UseItem");
        
        _rotateRight.started += context => rotateRightPressed = true;
        _rotateRight.canceled += context => rotateRightPressed = false;
        
        _rotateRightSuper.started += context => rotateRightSuperPressed = true;
        _rotateRightSuper.canceled += context => rotateRightSuperPressed = false;
        
        _rotateLeftSuper.started += context => rotateLeftSuperPressed = true;
        _rotateLeftSuper.canceled += context => rotateLeftSuperPressed = false;
        
        _rotateLeft.started += context => rotateLeftPressed = true;
        _rotateLeft.canceled += context => rotateLeftPressed = false;

        _thrusterPress.performed += context => ONThrusterPress?.Invoke();
        _rocketPress.performed += context => ONRocketPress?.Invoke();

        _useItem.performed += context => ONUseItem?.Invoke();
    }

    public void InitializeMashControls(Action<InputAction.CallbackContext> callback)
    {
        _playerInput.SwitchCurrentActionMap("Mash");
        
        InputAction mash = _playerInput.currentActionMap.FindAction("Mash");

        mash.performed += callback;
    }

    public void DestroyMashControls(Action<InputAction.CallbackContext> callback)
    {
        _playerInput.SwitchCurrentActionMap("Mash");
        
        InputAction mash = _playerInput.currentActionMap.FindAction("Mash");

        mash.performed -= callback;
    }

}
