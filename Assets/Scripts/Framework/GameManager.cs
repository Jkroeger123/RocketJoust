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
    
    private List<GameObject> _players;

    //Psedu bit mask for storing which IDs are used
    private bool[] _playerIDMask;
    
    private void Start()
    {
        SceneManager.sceneLoaded += OnBattleLoaded;
        _playerIDMask = new bool[10];
        _players = new List<GameObject>();
        DontDestroyOnLoad(gameObject);
    }

    public void OnPlayerJoined(PlayerInput input)
    {
        DontDestroyOnLoad(input.gameObject);
        _players.Add(input.gameObject);
        AssignPlayerID(input.gameObject);
        Instantiate(playerLobbyPrefab, input.gameObject.transform);
    }

    private void AssignPlayerID(GameObject player)
    {
        for (int i = 1; i < _playerIDMask.Length; i++)
        {
            if (_playerIDMask[i]) continue;
            
            player.GetComponent<Player>().PlayerID = i;
            _playerIDMask[i] = true;
            return;
        }
        
        Debug.LogError("Error: No ID available for new player.");
    }

    private void ClearPlayerID(GameObject player)
    {
        _playerIDMask[player.GetComponent<Player>().PlayerID] = false;
    }

    public void OnPlayerLeft(PlayerInput input)
    {
        ClearPlayerID(input.gameObject);
        _players.Remove(input.gameObject);
    }

    public void CheckPlayersReady()
    {
        if (_players.Count < 2) return;
        
        foreach (GameObject p in _players)
        {
            PlayerLobbyController playLob = p.transform.GetChild(0).GetComponent<PlayerLobbyController>();
            if(!playLob.isReady) return;
        }

        //Launch The countdown here
        StartCoroutine(LaunchMatch());
    }

    private IEnumerator LaunchMatch()
    {
        GetComponent<PlayerInputManager>().DisableJoining();
        countDown.gameObject.SetActive(true);

        //DisablePlayer Input
        int curID = 1;
        
        foreach (GameObject player in _players)
        {
            player.GetComponent<Player>().PlayerID = curID++;
            PlayerLobbyController playLob = player.transform.GetChild(0).GetComponent<PlayerLobbyController>();
            playLob.OnMatchStarting();
        }
        
        for (int i = 5; i > 0; i--)
        {
            countDown.text = "Match Starting in " + i + "...";
            yield return new WaitForSeconds(1f);
        }
        
        countDown.gameObject.SetActive(false);

        foreach (GameObject player in _players)
        {
            Destroy(player.transform.GetChild(0).gameObject);
        }
        
        SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);
    }

    private void OnBattleLoaded(Scene scene, LoadSceneMode mode) {
        if (scene.name == "NewLobby") return;
        //Find the Battle Manager and Initialize the game
        GameObject.FindWithTag("BattleManager").GetComponent<BattleManager>().StartMatch(_players);
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnBattleLoaded;
    }
}
