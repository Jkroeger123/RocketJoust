using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketBlaster : MonoBehaviour
{
    private void OnTriggerEnter2D (Collider2D other) {

        if (transform.IsChildOf(other.transform)) return;

        if (other.CompareTag("Player")) other.gameObject.GetComponent<PlayerDeathManager>().Die();

        if (other.CompareTag("Projectile"))
        {
            //Thoom the projectile
        }

    }
}
