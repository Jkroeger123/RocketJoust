using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeathHandler : MonoBehaviour
{
    public PlayerKillManager killManager;

    public GameObject deadPlayer;

    private GameObject _currentKiller;
    private bool _isInvincible;
    private bool _isParried;
    
    public static event Action<GameObject> ONDeath;
    
    private void Start ()
    {
        killManager.ONKill += OnKill;
        StartCoroutine(SetPlayerInvincibility(2f));
    }
    
    private IEnumerator SetPlayerInvincibility(float t)
    {
        _isInvincible = true;
        yield return new WaitForSeconds(t);
        _isInvincible = false;
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.IsChildOf(transform)) return;
        if (!other.CompareTag("Beam")) return;
        if (_isInvincible) return;

        StartCoroutine(ExecuteDeath(other.transform.parent.parent.gameObject));
    }
    

    //Caches the killer and waits a few frames before executing the kill
    //this gives a chance for the player to kill the player creating a parry
    //When this occurs, this routine is halted, preventing the death from executing
    private IEnumerator ExecuteDeath(GameObject killer)
    {
        _currentKiller = killer;

        yield return new WaitForSeconds(0.05f);

        _currentKiller = null;

        if (!_isParried) {
            Instantiate(deadPlayer, gameObject.transform.position, gameObject.transform.rotation).GetComponent<PlayerDeathAnimHandler>().OnDeath(killer);
            ONDeath?.Invoke(gameObject); 
        }

        _isParried = false;
    }

    private void OnKill(GameObject playerKilled)
    {
        if (_currentKiller != playerKilled) return;
        
        StopDeath();
        playerKilled.GetComponent<PlayerDeathHandler>().StopDeath();
        
        _currentKiller = null;
    }

    private void StopDeath()
    {
        _isParried = true;
    }

    private void OnDestroy()
    {
        killManager.ONKill -= OnKill;
    }
}
