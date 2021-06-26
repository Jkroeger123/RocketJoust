using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModule : MonoBehaviour
{

    public GameObject readyText;

    public void SetReadyText(bool isReady)
    {
        readyText.SetActive(isReady);
    }

}
