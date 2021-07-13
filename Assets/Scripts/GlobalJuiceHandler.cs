using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;

public class GlobalJuiceHandler : MonoBehaviour {
    
    public MMFeedbacks _deathFeedbacks;
    
    private void Start()
    {
        PlayerDeathHandler.ONDeath += PlayDeath;
    }

    private void PlayDeath(GameObject o)
    {
        _deathFeedbacks.PlayFeedbacks();
    }

    private void OnDestroy()
    {
        PlayerDeathHandler.ONDeath -= PlayDeath;
    }
}
