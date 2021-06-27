using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;

public class GlobalJuiceHandler : MonoBehaviour {
    
    public MMFeedbacks _deathFeedbacks;
    
    void Start() {
	    PlayerDeathManager.onDeath += o => _deathFeedbacks.PlayFeedbacks();
    }

}
