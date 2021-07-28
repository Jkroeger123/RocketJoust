using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{

    public Text playerIDText;
    public GameObject livesParent;
    public GameObject itemIndicator;
    
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

    public void SetItemSprite(Sprite sprite)
    {
        itemIndicator.SetActive(true);
        itemIndicator.GetComponent<Image>().sprite = sprite;
    }

    public void RemoveItem()
    {
        itemIndicator.SetActive(false);
    }

    public void RemoveLife()
    {
        Destroy(livesParent.transform.GetChild(0).gameObject);
    }

    
}
