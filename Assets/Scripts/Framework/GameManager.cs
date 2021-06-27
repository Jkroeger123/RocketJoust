using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject playerLobbyPrefab;

    public Text countDown;
    
    [NonSerialized]
    public List<GameObject> players;

    private void Start()
    {
        players = new List<GameObject>();
        DontDestroyOnLoad(gameObject);
    }

    public void OnPlayerJoined(PlayerInput input)
    {
        DontDestroyOnLoad(input.gameObject);
        players.Add(input.gameObject);
        Instantiate(playerLobbyPrefab, input.gameObject.transform);
    }

    public void OnPlayerLeft(PlayerInput input)
    {
        players.Remove(input.gameObject);
    }

    public void CheckPlayersReady()
    {
        if (players.Count < 2) return;
        
        foreach (GameObject p in players)
        {
            PlayerLobbyController playLob = p.transform.GetChild(0).GetComponent<PlayerLobbyController>();
            if(!playLob.isReady) return;
        }

        //Launch The countdown here
        StartCoroutine(LaunchMatch());
    }

    private IEnumerator LaunchMatch()
    {
        countDown.gameObject.SetActive(true);

        //DisablePlayer Input
        foreach (GameObject player in players)
        {
            PlayerLobbyController playLob = player.transform.GetChild(0).GetComponent<PlayerLobbyController>();
            playLob.OnMatchStarting();
        }
        
        for (int i = 5; i > 0; i--)
        {
            countDown.text = "Match Starting in " + i + "...";
            yield return new WaitForSeconds(1f);
        }
        
        countDown.gameObject.SetActive(false);
        
        SceneManager.sceneLoaded += OnSceneLoaded;
        
        foreach (GameObject player in players)
        {
            Destroy(player.transform.GetChild(0).gameObject);
        }
        
        SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //Find the Battle Manager and Initialize the game
        GameObject.FindWithTag("BattleManager").GetComponent<BattleManager>().StartMatch(players);
    }

}
