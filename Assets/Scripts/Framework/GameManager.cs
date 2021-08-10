using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject playerLobbyPrefab;
    public Image transitionImage;
    
    public Text countDown;
    public Text joinText;
    
    private List<GameObject> _players;

    private Coroutine _startRoutine;
    
    //Psedu bit mask for storing which IDs are used
    private bool[] _playerIDMask;
    private bool _isStartingMatch;
    private bool _isAwake;
    
    private void Awake()
    {
        SceneManager.sceneLoaded += OnBattleLoaded;
        _playerIDMask = new bool[10];
        _players = new List<GameObject>();
        DontDestroyOnLoad(gameObject);
        _isAwake = true;
    }

    public void OnPlayerJoined(PlayerInput playerInput)
    {
        StartCoroutine(JoinOnceAwake(playerInput));
        
        IEnumerator JoinOnceAwake(PlayerInput input)
        {
            while (!_isAwake) yield return null;
        
            DontDestroyOnLoad(input.gameObject);
            _players.Add(input.gameObject);
            AssignPlayerID(input.gameObject);
            Instantiate(playerLobbyPrefab, input.gameObject.transform);
        
            //Reorder the ui if necessary
            _players.Sort((g, o) 
                => g.GetComponent<Player>().PlayerID.CompareTo(o.GetComponent<Player>().PlayerID));

            for (int i = 0; i < _players.Count; i++)
            {
                _players[i].transform.GetChild(0).GetComponent<PlayerLobbyController>()
                    .SetSiblingIndex(i);
            }
        }
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
        if (_players.Count == 0)
        {
            SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
        }
    }

    public void CheckPlayersReady()
    {
        if (_players.Count < 2) return;
        
        foreach (GameObject p in _players)
        {
            PlayerLobbyController playLob = p.transform.GetChild(0).GetComponent<PlayerLobbyController>();
            if (playLob.isReady) continue;
            
            if (_isStartingMatch) CancelMatchStart();
            return;
        }

        //Launch The countdown here
        _startRoutine = StartCoroutine(LaunchMatch());
    }

    private void CancelMatchStart()
    {
        StopCoroutine(_startRoutine);
        _isStartingMatch = false;
        GetComponent<PlayerInputManager>().EnableJoining();
        
        joinText.gameObject.SetActive(true);
        countDown.gameObject.SetActive(false);
        
        //Enable Player Input
        foreach (GameObject player in _players)
        {
            PlayerLobbyController playLob = player.transform.GetChild(0).GetComponent<PlayerLobbyController>();
            playLob.OnMatchStartCancel();
        }
    }

    private IEnumerator LaunchMatch()
    {
        _isStartingMatch = true;
        GetComponent<PlayerInputManager>().DisableJoining();
        
        joinText.gameObject.SetActive(false);
        countDown.gameObject.SetActive(true);

        //DisablePlayer Input
        foreach (GameObject player in _players)
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

        foreach (GameObject player in _players)
        {
            Destroy(player.transform.GetChild(0).gameObject);
        }
        
        DOTween.To(() => transitionImage.fillAmount, (x) => transitionImage.fillAmount = x, 1f, 0.3f)
            .OnComplete(() => SceneManager.LoadScene("SampleScene", LoadSceneMode.Single));
    }

    private void OnBattleLoaded(Scene scene, LoadSceneMode mode) {
        
        if(scene.name == "MainMenu") Destroy(gameObject);
        
        if (scene.name != "SampleScene") return;
        //Find the Battle Manager and Initialize the game
        GameObject.FindWithTag("BattleManager").GetComponent<BattleManager>().StartMatch(_players);
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnBattleLoaded;
    }
}
