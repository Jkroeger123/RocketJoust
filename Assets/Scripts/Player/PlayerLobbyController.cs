using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLobbyController : MonoBehaviour
{
    public bool isReady;
    
    private GameManager _gameManager;
    
    private PlayerInput _playerInput;
    
    private PlayerLobbyUI _lobbyUI;
    
    
    private void Start()
    {
        _playerInput = GetComponentInParent<PlayerInput>();
        _gameManager = GameObject.Find("LobbyManager").GetComponent<GameManager>();
        
        _lobbyUI = FindAvailablePlayerSlot();
        _lobbyUI.gameObject.SetActive(true);

        InitializeUIControls();
    }

    private PlayerLobbyUI FindAvailablePlayerSlot()
    {
        foreach (Transform playerSlot in GameObject.Find("PlayerSlots").transform)
        {
            if (!playerSlot.GetChild(0).gameObject.activeInHierarchy)
            {
                return playerSlot.GetChild(0).gameObject.GetComponent<PlayerLobbyUI>();
            }
        }

        return null;
    }

    public void OnMatchStarting() => TerminateUIControls();

    private void InitializeUIControls()
    {
        _playerInput.SwitchCurrentActionMap("UI");
        _playerInput.currentActionMap.FindAction("Leave").performed += OnLeavePressed;
        _playerInput.currentActionMap.FindAction("Ready").performed += OnPlayerReady;
    }
    
    private void TerminateUIControls()
    {
        _playerInput.currentActionMap.FindAction("Ready").performed -= OnPlayerReady;
        _playerInput.currentActionMap.FindAction("Leave").performed -= OnLeavePressed;
    }

    
    #region UIControls

    private void OnLeavePressed(InputAction.CallbackContext context)
    {
        _lobbyUI.ResetUI();
        _lobbyUI.gameObject.SetActive(false);
        
        TerminateUIControls();
        Destroy(transform.parent.gameObject, 0.3f);   
    }

    private void OnPlayerReady(InputAction.CallbackContext context)
    {
        isReady = !isReady;
        _lobbyUI.SetReadyText(isReady);
        _gameManager.CheckPlayersReady();
    }

    #endregion
    
}
