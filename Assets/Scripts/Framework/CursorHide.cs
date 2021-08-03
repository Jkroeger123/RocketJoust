using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorHide : MonoBehaviour
{ 
    private void Update()
    {
        if (Screen.fullScreen)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}
