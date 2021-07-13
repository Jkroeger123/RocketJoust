using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKillManager : MonoBehaviour
{
    public event Action<GameObject> ONKill;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        ONKill?.Invoke(other.gameObject);
    }
}
