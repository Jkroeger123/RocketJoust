﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int PlayerID { get; set; }
    public string Name { get; set; }

    private PlayerHUD _playerHUD;

    public void CreatePlayerHUD(GlobalHUDManager globalHUDManager)
    {
        _playerHUD = globalHUDManager.CreatePlayerHUD();
        _playerHUD.playerIDText.text = "P" + PlayerID;
    }

    public void DecrementHealth()
    {
        if (_playerHUD == null) return;
        
        _playerHUD.RemoveLife();
    }

}
