using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;

public class LobbyAudioManager : MonoBehaviour
{
    public MMFeedbacks playerNavAudio;
    public MMFeedbacks playerReadyAudio;
    public MMFeedbacks playerJoinAudio;
    
    private void Start()
    {
        PlayerLobbyController.OnNav += OnNav;
        PlayerLobbyController.OnJoin += OnJoin;
        PlayerLobbyController.OnReady += OnReady;
    }

    private void OnNav() => playerNavAudio.PlayFeedbacks();
    private void OnJoin() => playerJoinAudio.PlayFeedbacks();
    private void OnReady() => playerReadyAudio.PlayFeedbacks();

    private void OnDestroy()
    {
        PlayerLobbyController.OnNav -= OnNav;
        PlayerLobbyController.OnJoin -= OnJoin;
        PlayerLobbyController.OnReady -= OnReady;
    }
}
