using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;

public class GameplayAudioManager : MonoBehaviour
{
    public MMFeedbacks playerThrustAudio;
    public MMFeedbacks playerHitAudio;
    public MMFeedbacks explosionAudio;
    public MMFeedbacks song;
    
    private void Start()
    {
        PlayerController.ONBlast += OnThrust;
        PlayerDeathHandler.ONDeath += OnDeath;
        PlayerDeathAnimHandler.OnExplosion += OnExplosion;
        song.PlayFeedbacks();
    }

    private void OnThrust(GameObject g) => playerThrustAudio.PlayFeedbacks();
    private void OnDeath(GameObject g) => playerHitAudio.PlayFeedbacks();

    private void OnExplosion() => explosionAudio.PlayFeedbacks();

    private void OnDestroy()
    {
        PlayerController.ONBlast -= OnThrust;
        PlayerDeathHandler.ONDeath -= OnDeath;
        PlayerDeathAnimHandler.OnExplosion -= OnExplosion;
    }
}
