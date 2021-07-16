using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class ItemEntryHandler : PlayerEntryHandler
{
    private void Start()
    {
        GameObject item = transform.parent.gameObject;
        transform.parent = null;
        
        ExecuteSequence(item);
        Destroy(gameObject, 3f); 
    }

}
