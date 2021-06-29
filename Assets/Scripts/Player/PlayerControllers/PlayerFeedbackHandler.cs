using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;

public class PlayerFeedbackHandler : MonoBehaviour
{
    public MMFeedbacks thrustFeedback;
    
    private void Awake() {
        PlayerController.ONThrust += OnThrust;
    }
    
    private void OnThrust (GameObject g) {
        if (g != gameObject) return;
        thrustFeedback.PlayFeedbacks();
    }

    private void OnDestroy () {
        PlayerController.ONThrust -= OnThrust;
    }
}
