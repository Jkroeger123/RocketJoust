using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int PlayerID { get; set; }
    public string Name { get; set; }

    [NonSerialized]
    public GameObject characterPrefab;
    
    public PlayerHUD playerHUD;

    public void CreatePlayerHUD(GlobalHUDManager globalHUDManager)
    {
        playerHUD = globalHUDManager.CreatePlayerHUD();
        playerHUD.SetID(PlayerID);
    }

    public PlayerHUD GetPlayerHUD() => playerHUD;

    public void DecrementHealth()
    {
        if (playerHUD == null) return;
        
        playerHUD.RemoveLife();
    }

}
