using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLobbyController : MonoBehaviour
{
    private GameManager _gameManager;
    private PlayerInput _playerInput;
    private GameObject _uiModule;
    public bool isReady = false;
    
    private void Start()
    {
        _uiModule = FindUIModule();
        _uiModule.SetActive(true);
        
        _gameManager = GameObject.Find("LobbyManager").GetComponent<GameManager>();
        _playerInput = GetComponentInParent<PlayerInput>();
        
        _playerInput.SwitchCurrentActionMap("UI");
        _playerInput.currentActionMap.FindAction("Leave").performed += OnLeavePressed;
        _playerInput.currentActionMap.FindAction("Ready").performed += OnPlayerReady;
    }

    private GameObject FindUIModule()
    {
        foreach (Transform playerSlot in GameObject.Find("PlayerSlots").transform)
        {
            if (!playerSlot.GetChild(0).gameObject.activeInHierarchy)
            {
                return playerSlot.GetChild(0).gameObject;
            }
        }

        return null;
    }

    private void OnLeavePressed(InputAction.CallbackContext context)
    {
        _playerInput.currentActionMap.FindAction("Leave").performed -= OnLeavePressed;
        _playerInput.currentActionMap.FindAction("Ready").performed -= OnPlayerReady;
        _uiModule.SetActive(false);
        Destroy(transform.parent.gameObject, 0.3f);   
    }

    private void OnPlayerReady(InputAction.CallbackContext context)
    {
        isReady = !isReady;
        _uiModule.GetComponent<PlayerModule>().SetReadyText(isReady);
        _gameManager.CheckPlayersReady();
    }
}
