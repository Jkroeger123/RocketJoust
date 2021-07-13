﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class BattleManager : MonoBehaviour
{
    public GameObject camGroupObj;
    
    public GameObject playerPrefab;
    
    public GameObject spawnPoints;

    public GameObject countDownText;
    
    public static bool canMove;
    
    private List<GameObject> _players;
    private Dictionary<GameObject, int> _livesLeft;

    private GlobalHUDManager _hudManager;
    private CinemachineTargetGroup _camGroup;

    private void Awake()
    {

        canMove = false;
        
        PlayerDeathHandler.ONDeath += OnDeath;
        
        _livesLeft = new Dictionary<GameObject, int>();

        _hudManager = GetComponent<GlobalHUDManager>();
        _camGroup = camGroupObj.GetComponent<CinemachineTargetGroup>();
    }

    public void StartMatch(List<GameObject> players)
    {
        _players = new List<GameObject>(players);

        for (int i = 0; i < _players.Count; i++)
        {
            _livesLeft.Add(_players[i], 3);

            Player pl = _players[i].GetComponent<Player>();
            pl.CreatePlayerHUD(_hudManager);
            
            GameObject spawnPref = Instantiate(pl.characterPrefab, _players[i].transform);
            spawnPref.transform.position = spawnPoints.transform.GetChild(i).position;
            _camGroup.AddMember(spawnPref.transform, 1, 0);
        }

        Countdown(3, () => canMove = true);
        
    }

    private void Countdown(int time, Action callback)
    {
        if (time <= 0)
        {
            countDownText.GetComponent<Text>().text = "GO";

            countDownText.transform.DOScale(1.5f, 0.4f).OnComplete(() =>
            {
                countDownText.transform.DOScale(0, 0.4f).OnComplete(() => countDownText.SetActive(false));
            });
            
            callback();
            return;
        }

        countDownText.SetActive(true);
        countDownText.GetComponent<Text>().text = "" + time;
        
        countDownText.transform.DOScale(1.5f, 0.4f).OnComplete(() =>
        {
            countDownText.transform.DOScale(1, 0.4f).OnComplete(() =>
            {
                Countdown(time - 1, callback);
            });
        });
    }

    private void CheckGameOver () {
        
        int left  = _livesLeft.Count(kvp => kvp.Value > 0);
        if (left != 1) return;
        
        GameObject winner = _livesLeft.Single(pair => pair.Value > 0).Key;
        StartCoroutine(PraiseWinner(winner));
    }

    private void OnDeath (GameObject player) {
        GameObject p = player.transform.parent.gameObject;
        
        _livesLeft[p] = _livesLeft[p] - 1;
        p.GetComponent<Player>().DecrementHealth();
        
        CheckGameOver();
        
        if (_livesLeft[p] > 0) {
            StartCoroutine(Respawn(p));
        }
        
        Destroy(player);
    }

    private IEnumerator Respawn (GameObject player) {
        
        yield return new WaitForSeconds(2f);
        
        GameObject spawnPref = Instantiate(playerPrefab, player.transform);
        spawnPref.transform.position = spawnPoints.transform.GetChild(Random.Range(0, spawnPoints.transform.childCount)).position;
        _camGroup.AddMember(spawnPref.transform, 1, 0);
    }


    private IEnumerator PraiseWinner(GameObject player)
    {
        
        countDownText.SetActive(true);
        countDownText.GetComponent<Text>().text = "P" + player.GetComponent<Player>().PlayerID + " Wins!";

        countDownText.transform.DOScale(1.4f, 0.4f);

        yield return new WaitForSeconds(3f);

        StartCoroutine(Restart());
    }

    private IEnumerator Restart () {
        yield return new WaitForSeconds(1f);

        foreach (GameObject player in _players)
        {
            Destroy(player);
        }
        
        Destroy(GameObject.Find("LobbyManager").gameObject);

        SceneManager.LoadScene("NewLobby");
    }

    private void OnDestroy()
    {
        PlayerDeathHandler.ONDeath -= OnDeath;
    }
}
