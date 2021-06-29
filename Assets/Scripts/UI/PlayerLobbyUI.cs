using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLobbyUI : MonoBehaviour
{

    public GameObject readyText;

    public void SetReadyText(bool isReady)
    {
        readyText.SetActive(isReady);
    }

    public void ResetUI()
    {
        readyText.SetActive(false);
    }

}
