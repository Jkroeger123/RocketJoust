using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLobbyUI : MonoBehaviour
{

    public GameObject readyText;
    public Text playerText;
    
    public void SetReadyText(bool isReady)
    {
        readyText.SetActive(isReady);
    }

    public void SetPlayerNumber(int n)
    {
        playerText.text = "P" + n;
    }

    public void ResetUI()
    {
        readyText.SetActive(false);
    }

}
