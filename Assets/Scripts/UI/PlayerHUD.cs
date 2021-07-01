using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{

    public Text playerIDText;
    public GameObject livesParent;

    
    public void RemoveLife()
    {
        Destroy(livesParent.transform.GetChild(0).gameObject);
    }
}
