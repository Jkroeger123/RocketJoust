using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Cinemachine;
using UnityEngine;

public class PlayerDeathAnimHandler : MonoBehaviour
{
    private Rigidbody2D _rb;
    public float pinballSpeed;
    public float _detonationTimer;
    public float spinVelocity;

    private BattleManager _battleManager;
    // Start is called before the first frame update
    void Awake() {
        _battleManager = FindObjectOfType<BattleManager>().GetComponent<BattleManager>();
        _battleManager.GetCamGroup().AddMember(transform, 1, 0);
        _rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update() {
        _detonationTimer -= Time.deltaTime;
        _rb.rotation += spinVelocity;
        if (_detonationTimer <= 0) Destroy(gameObject);
    }

    public void OnDeath (GameObject killer) {
        _rb.velocity = -killer.transform.up*pinballSpeed;
    }
    
    
}
