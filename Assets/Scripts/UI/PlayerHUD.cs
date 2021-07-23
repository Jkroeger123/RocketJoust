using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{

    public Text playerIDText;
    public GameObject livesParent;

    public List<Sprite> hudColors;

    private void SetPanel(Sprite img)
    {
        GetComponent<Image>().sprite = img;
    }

    public void SetID(int id)
    {
        playerIDText.text = "P" + id;
        SetPanel(hudColors[(id-1) % hudColors.Count]);
    }
    
    public void RemoveLife()
    {
        Destroy(livesParent.transform.GetChild(0).gameObject);
    }
}
