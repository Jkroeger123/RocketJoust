using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Cinemachine;
using MoreMountains.Feedbacks;
using UnityEngine;

public class PlayerDeathAnimHandler : MonoBehaviour
{
    private Rigidbody2D _rb;

    public GameObject deathParticle;
    public float pinballSpeed;
    public float _detonationTimer;
    public float spinVelocity;

    public static event Action OnExplosion;
    
    private GameObject _deathFeedback;
    
    public int id;
    
    private BattleManager _battleManager;
    
    private void Awake() {
        _deathFeedback = GameObject.Find("DeathExplosion");
        _battleManager = FindObjectOfType<BattleManager>().GetComponent<BattleManager>();
        _battleManager.camGroupObj.GetComponent<CamGroupController>().AddObject(gameObject);
        _rb = GetComponent<Rigidbody2D>();
    }

    
    private void Update() {
        _detonationTimer -= Time.deltaTime;
        _rb.rotation += spinVelocity;
        
        if (_detonationTimer <= 0)
        {
            _battleManager.camGroupObj.GetComponent<CamGroupController>().SmoothRemove(gameObject);
            GetComponent<PlayerChunkSpawner>().SpawnChunks(id);
            Instantiate(deathParticle, transform.position, transform.rotation);
            _deathFeedback.GetComponent<MMFeedbacks>().PlayFeedbacks();
            OnExplosion?.Invoke();
            Destroy(gameObject);
        }
    }

    public void OnDeath (GameObject killer) {
        _rb.velocity = -killer.transform.up*pinballSpeed;
    }

    public void SetBody(Sprite sprite)
    {
        GetComponent<SpriteRenderer>().sprite = sprite;
    }

}
