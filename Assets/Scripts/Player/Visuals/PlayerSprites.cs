using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSprites : MonoBehaviour
{
    public List<Sprite> spriteOptions;

    private void Start()
    {
        int count = spriteOptions.Count;
        int playerNum = transform.parent.GetComponent<Player>().PlayerID;
        int i = playerNum % count;
        GetComponent<SpriteRenderer>().sprite = spriteOptions[i];
    }
}
