using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLobbyController : MonoBehaviour
{
    
    public GameObject uiModule;
    
    public bool isReady;

    private Player _player;
    private GameManager _gameManager;
    private PlayerInput _playerInput;
    private PlayerLobbyUI _lobbyUI;
    
    
    private void Start()
    {
        _playerInput = GetComponentInParent<PlayerInput>();
        _gameManager = GameObject.Find("LobbyManager").GetComponent<GameManager>();
        _player = transform.parent.GetComponent<Player>();
        
        _lobbyUI = CreatePlayerSlot();
        _lobbyUI.gameObject.SetActive(true);

        InitializeUIControls();
    }

    private PlayerLobbyUI CreatePlayerSlot()
    {
        GameObject g = Instantiate(uiModule, GameObject.Find("PlayerSlots").transform);

        PlayerLobbyUI ui = g.GetComponent<PlayerLobbyUI>();
        ui.SetPlayerNumber(_player.PlayerID);
        return ui;
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
        Destroy(_lobbyUI.gameObject);
        
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
