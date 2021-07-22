using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputController : MonoBehaviour
{
    private PlayerInput _playerInput;

    //used for caching the action map to go back to after pause finishes
    private InputActionMap _previousActionMap;
    
    private InputAction _rotateRight;
    private InputAction _rotateRightSuper;
    private InputAction _rotateLeft;
    private InputAction _rotateLeftSuper;
    private InputAction _thrusterPress;
    private InputAction _rocketPress;
    private InputAction _useItem;
    private InputAction _pause;

    private InputAction _UiPause;
    private InputAction _leave;
    
    [NonSerialized]
    public bool rotateRightPressed;
    
    [NonSerialized]
    public bool rotateLeftPressed;

    [NonSerialized] 
    public bool rotateRightSuperPressed;
    
    [NonSerialized] 
    public bool rotateLeftSuperPressed;
    

    public event Action ONThrusterPress;
    public event Action ONRocketPress;
    public event Action ONUseItem;
    
    private void Awake()
    {
        _playerInput = GetComponentInParent<PlayerInput>();
        
        InitializeGameplayControls();
        InitializeUI();

        _playerInput.onDeviceLost += OnDeviceLost;
        PauseMenu.ONPauseSet += OnPauseSet;
    }

    private void Start()
    {
        EnableGameplayControls();
    }

    #region Gameplay

    private void InitializeGameplayControls()
    {

        _playerInput.SwitchCurrentActionMap("Gameplay");
        
        _rotateRight = _playerInput.currentActionMap.FindAction("RotateRight");
        _rotateLeft = _playerInput.currentActionMap.FindAction("RotateLeft");
        _thrusterPress = _playerInput.currentActionMap.FindAction("Thruster");
        _rocketPress = _playerInput.currentActionMap.FindAction("Rocket");
        _rotateRightSuper = _playerInput.currentActionMap.FindAction("RotateRightSuper");
        _rotateLeftSuper = _playerInput.currentActionMap.FindAction("RotateLeftSuper");
        _useItem = _playerInput.currentActionMap.FindAction("UseItem");
        _pause = _playerInput.currentActionMap.FindAction("Pause");
        
        _rotateRight.started += RotateRight;
        _rotateRight.canceled += RotateRightCancel;
        
        _rotateRightSuper.started += RotateRightSuper;
        _rotateRightSuper.canceled += RotateRightSuperCancel;
        
        _rotateLeftSuper.started += RotateLeftSuper;
        _rotateLeftSuper.canceled += RotateLeftSuperCancel;
        
        _rotateLeft.started += RotateLeft;
        _rotateLeft.canceled += RotateLeftCancel;

        _thrusterPress.performed += ThrusterPress;
        _rocketPress.performed += RocketPress;

        _useItem.performed += UseItem;

        _pause.performed += PauseInput;
    }

    public void EnableGameplayControls() => _playerInput.SwitchCurrentActionMap("Gameplay");
    
    public void DestroyGameplayControls()
    {
        
        _rotateRight.started -= RotateRight;
        _rotateRight.canceled -= RotateRightCancel;
        
        _rotateRightSuper.started -= RotateRightSuper;
        _rotateRightSuper.canceled -= RotateRightSuperCancel;
        
        _rotateLeftSuper.started -= RotateLeftSuper;
        _rotateLeftSuper.canceled -= RotateLeftSuperCancel;
        
        _rotateLeft.started -= RotateLeft;
        _rotateLeft.canceled -= RotateLeftCancel;

        _thrusterPress.performed -= ThrusterPress;
        _rocketPress.performed -= RocketPress;

        _useItem.performed -= UseItem;

        _pause.performed -= PauseInput;
    }
    
    #region GameplayFunctions

    private void RotateRight(InputAction.CallbackContext context) => rotateRightPressed = true;
    private void RotateRightCancel(InputAction.CallbackContext context) => rotateRightPressed = false;

    private void RotateLeft(InputAction.CallbackContext context) => rotateLeftPressed = true;
    private void RotateLeftCancel(InputAction.CallbackContext context) => rotateLeftPressed = false;
    
    private void RotateRightSuper(InputAction.CallbackContext context) => rotateRightSuperPressed = true;
    private void RotateRightSuperCancel(InputAction.CallbackContext context) => rotateRightSuperPressed = false;

    private void RotateLeftSuper(InputAction.CallbackContext context) => rotateLeftSuperPressed = true;
    private void RotateLeftSuperCancel(InputAction.CallbackContext context) => rotateLeftSuperPressed = false;
    
    private void ThrusterPress(InputAction.CallbackContext context) => ONThrusterPress?.Invoke();
    private void RocketPress(InputAction.CallbackContext context) => ONRocketPress?.Invoke();
    
    private void UseItem(InputAction.CallbackContext context) => ONUseItem?.Invoke();
    
    #endregion

    #endregion

    #region Mash

    public void InitializeMashControls(Action<InputAction.CallbackContext> callback)
    {
        _playerInput.SwitchCurrentActionMap("Mash");
        
        InputAction mash = _playerInput.currentActionMap.FindAction("Mash");
        InputAction paused = _playerInput.currentActionMap.FindAction("Pause");
        
        paused.performed += PauseInput;
        mash.performed += callback;
    }

    public void DestroyMashControls(Action<InputAction.CallbackContext> callback)
    {
        if (!isActiveAndEnabled) return;
        
        _playerInput.SwitchCurrentActionMap("Mash");
        
        InputAction mash = _playerInput.currentActionMap.FindAction("Mash");
        InputAction paused = _playerInput.currentActionMap.FindAction("Pause");
        
        paused.performed -= PauseInput;
        mash.performed -= callback;
    }

    #endregion
    
    #region UI

    public void InitializeUI()
    {
        _playerInput.SwitchCurrentActionMap("UI");
        
        _UiPause = _playerInput.currentActionMap.FindAction("Pause");
        _UiPause.performed += PauseInput;
        
        _leave = _playerInput.currentActionMap.FindAction("Leave");
        _leave.performed += PauseInput;
    }

    public void DestroyUIControls()
    {
        _UiPause.performed -= PauseInput;
        _leave.performed -= PauseInput;
    }

    #endregion
    
    
    private void PauseInput(InputAction.CallbackContext context) 
        => PauseMenu.isPaused = !PauseMenu.isPaused;

    private void OnPauseSet(bool isSet)
    {
        
        if (isSet)
        {
            _previousActionMap = _playerInput.currentActionMap;
            _playerInput.SwitchCurrentActionMap("UI");
        }
        else
        {
            _playerInput.currentActionMap = _previousActionMap;
        }
    }

    private void OnDeviceLost(PlayerInput lost) => OnPauseSet(true);
    
    private void OnDestroy()
    {
        DestroyGameplayControls();
        DestroyUIControls();
        _playerInput.onDeviceLost -= OnDeviceLost;
        PauseMenu.ONPauseSet -= OnPauseSet;
    }
}
