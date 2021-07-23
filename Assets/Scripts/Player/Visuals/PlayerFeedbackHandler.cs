using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;

public class PlayerFeedbackHandler : MonoBehaviour
{
    public GameObject thrustParticle;
    public GameObject thrustLocation;
    public GameObject deathParticle;
    public GameObject playerEntryObject;
    

    private void Awake()
    {
        PlayerController.ONThrust += OnThrust;
        PlayerDeathHandler.ONDeath += OnDeath;
    }

    private void Start()
    {
        Instantiate(playerEntryObject, transform.position, Quaternion.identity).GetComponent<PlayerEntryHandler>().ExecuteSequence(gameObject);
    }

    private void OnThrust(GameObject g)
    {
        if (g != gameObject) return;
        Instantiate(thrustParticle, thrustLocation.transform.position, transform.rotation);
    }

    private void OnDeath(GameObject g)
    {
        if (g != gameObject) return;
        Instantiate(deathParticle, transform.position, Quaternion.LookRotation(g.transform.position));
    }

    private void OnDestroy ()
    {
        PlayerDeathHandler.ONDeath -= OnDeath;
        PlayerController.ONThrust -= OnThrust;
    }
}
