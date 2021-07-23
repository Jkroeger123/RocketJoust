using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSprites : MonoBehaviour
{
    public List<Sprite> spriteOptions;

    private void Awake()
    {
        int count = spriteOptions.Count;
        int playerNum = transform.parent.GetComponent<Player>().PlayerID;
        int i = (playerNum-1) % count;
        GetComponent<SpriteRenderer>().sprite = spriteOptions[i];
    }
}
