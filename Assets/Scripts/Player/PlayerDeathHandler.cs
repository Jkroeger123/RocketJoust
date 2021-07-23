using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeathHandler : MonoBehaviour
{
    public PlayerKillManager killManager;
    public GameObject bubble;
    
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
        SetBubbleActive(_isInvincible);
        PlayerController.ONBlast += CheckBubble;
        yield return new WaitForSeconds(t);
        PlayerController.ONBlast -= CheckBubble;
        _isInvincible = false;
        SetBubbleActive(_isInvincible);
    }

    private void CheckBubble(GameObject player)
    {
        if(player != gameObject) return;
        
        PlayerController.ONBlast -= CheckBubble;
        _isInvincible = false;
        SetBubbleActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.IsChildOf(transform)) return;
        if (!other.CompareTag("Beam")) return;
        if (_isInvincible) return;

        StartCoroutine(ExecuteDeath(other.transform.parent.parent.gameObject));
    }

    private void SetBubbleActive(bool isActive)
    {
        bubble.SetActive(isActive);
        Physics2D.IgnoreCollision(bubble.GetComponent<Collider2D>(), GetComponent<Collider2D>());
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
            
            PlayerDeathAnimHandler pd = Instantiate(deadPlayer, gameObject.transform.position, gameObject.transform.rotation)
                .GetComponent<PlayerDeathAnimHandler>();
            pd.OnDeath(killer);

            pd.id = transform.parent.GetComponent<Player>().PlayerID;
            pd.SetBody(GetComponent<SpriteRenderer>().sprite);
            pd.SetFace(transform.Find("face").GetComponent<SpriteRenderer>().sprite);
            
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
