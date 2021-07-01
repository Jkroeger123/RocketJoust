using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalHUDManager : MonoBehaviour
{
    public GameObject playerHUDParent;
    
    public GameObject playerHUDPrefab;
    
    public PlayerHUD CreatePlayerHUD()
    {
        return Instantiate(playerHUDPrefab, playerHUDParent.transform).GetComponent<PlayerHUD>();
    }

}
