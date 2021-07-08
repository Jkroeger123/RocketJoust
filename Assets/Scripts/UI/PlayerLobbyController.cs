﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerLobbyController : MonoBehaviour
{
    [Header("Module")]
    public GameObject uiModule;
    public List<Sprite> bubbleOptions;
    
    [Header("Selector")]
    public GameObject selectorPrefab;
    public List<Color> selectorColors;
    
    [NonSerialized]
    public bool isReady;
    
    private Player _player;
    private GameManager _gameManager;
    private PlayerInput _playerInput;
    private PlayerLobbyUI _lobbyUI;
    private GameObject _currentSelection;
    private GameObject _selector;

    public static event Action OnNav, OnReady, OnJoin;
    
    private void Start()
    {
        _playerInput = GetComponentInParent<PlayerInput>();
        _gameManager = GameObject.Find("LobbyManager").GetComponent<GameManager>();
        _player = transform.parent.GetComponent<Player>();
        
        _lobbyUI = CreatePlayerSlot();
        _lobbyUI.gameObject.SetActive(true);
        
        SelectCharacter(GameObject.Find("CharacterSlotArea").transform.GetChild(0).gameObject);
        
        InitializeUIControls();
        
        OnJoin?.Invoke();
    }

    private void SelectCharacter(GameObject character)
    {
        _currentSelection = character;
        _player.characterPrefab = _currentSelection.GetComponent<SelectableCharacter>().characterPrefab;
        
        //Place the selector as a child of the character slot
        _selector.transform.SetParent(character.transform.GetChild(0));
        _lobbyUI.SetCharacterImage(character.GetComponent<Image>().sprite);
    }

    private PlayerLobbyUI CreatePlayerSlot()
    {
        //Create Bubble
        GameObject g = Instantiate(uiModule, GameObject.Find("PlayerSlots").transform);

        //Set P# text
        PlayerLobbyUI ui = g.GetComponent<PlayerLobbyUI>();
        ui.SetPlayerNumber(_player.PlayerID);
        ui.SetBubbleImage(bubbleOptions[(_player.PlayerID-1) % bubbleOptions.Count]);
        
        //Create Character selector with correct color
        _selector = Instantiate(selectorPrefab);
        _selector.GetComponent<Image>().color = selectorColors[(_player.PlayerID - 1) % selectorColors.Count];
        
        return ui;
    }

    public void OnMatchStarting() => TerminateUIControls();

    private void InitializeUIControls()
    {
        _playerInput.SwitchCurrentActionMap("UI");
        _playerInput.currentActionMap.FindAction("Leave").performed += OnLeavePressed;
        _playerInput.currentActionMap.FindAction("Ready").performed += OnPlayerReady;
        _playerInput.currentActionMap.FindAction("Nav").performed += OnPlayerNavigate;
    }
    
    private void TerminateUIControls()
    {
        _playerInput.currentActionMap.FindAction("Ready").performed -= OnPlayerReady;
        _playerInput.currentActionMap.FindAction("Leave").performed -= OnLeavePressed;
        _playerInput.currentActionMap.FindAction("Nav").performed -= OnPlayerNavigate;
    }

    
    #region UIControls

    private void OnLeavePressed(InputAction.CallbackContext context)
    {
        _lobbyUI.ResetUI();
        Destroy(_lobbyUI.gameObject);
        
        //Destroy Selector Object
        Destroy(_selector);
        
        TerminateUIControls();
        Destroy(transform.parent.gameObject, 0.3f);   
    }

    private void OnPlayerReady(InputAction.CallbackContext context)
    {
        isReady = !isReady;
        _lobbyUI.SetReadyText(isReady);
        _gameManager.CheckPlayersReady();
        
        OnReady?.Invoke();
    }

    private void OnPlayerNavigate(InputAction.CallbackContext context)
    {
        if (isReady) return;
        
        Vector2 dir = context.ReadValue<Vector2>().normalized;
        float dotEast = Vector2.Dot(Vector2.right, dir);
        float dotNorth = Vector2.Dot(Vector2.up, dir);
        
        if (Mathf.Abs(dotEast) > Mathf.Abs(dotNorth)) {

            if (dotEast > 0)
            {
                //Right
                Selectable newSelection = _currentSelection.GetComponent<SelectableCharacter>().FindSelectableOnRight();
                if (newSelection == null) return;
                SelectCharacter(newSelection.gameObject);
            }
            else
            {
                //Left
                Selectable newSelection = _currentSelection.GetComponent<SelectableCharacter>().FindSelectableOnLeft();
                if (newSelection == null) return;
                SelectCharacter(newSelection.gameObject);
            }

        } else {
            
            if (dotNorth > 0)
            {
                //Up
                Selectable newSelection = _currentSelection.GetComponent<SelectableCharacter>().FindSelectableOnUp();
                if (newSelection == null) return;
                SelectCharacter(newSelection.gameObject);
            }
            else
            {
                //Down
                Selectable newSelection = _currentSelection.GetComponent<SelectableCharacter>().FindSelectableOnDown();
                if (newSelection == null) return;
                SelectCharacter(newSelection.gameObject);
            }
            
        }
        
        OnNav?.Invoke();
    }

    #endregion
    
}