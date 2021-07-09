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

    public ParticleSystem blastParticle;

    private void Awake()
    {
        PlayerController.ONThrust += OnThrust;
        PlayerController.ONBlast += OnBlast;
        PlayerController.ONDeath += OnDeath;
    }

    private void OnThrust(GameObject g)
    {
        if (g != gameObject) return;
        Instantiate(thrustParticle, thrustLocation.transform.position, transform.rotation);
    }

    private void OnBlast(GameObject g)
    {
        if (g != gameObject) return;
        blastParticle.Play();
    }

    private void OnDeath(GameObject g)
    {
        if (g != gameObject) return;
        Instantiate(deathParticle, transform.position, Quaternion.identity);
    }

    private void OnDestroy ()
    {
        PlayerController.ONDeath -= OnDeath;
        PlayerController.ONThrust -= OnThrust;
        PlayerController.ONBlast -= OnBlast;
    }
}
