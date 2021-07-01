using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class BattleManager : MonoBehaviour
{
    public GameObject camGroupObj;
    
    public GameObject playerPrefab;
    
    public GameObject spawnPoints;
    
    private List<GameObject> _players;
    private Dictionary<GameObject, int> _livesLeft;

    private GlobalHUDManager _hudManager;
    private CinemachineTargetGroup _camGroup;

    private void Awake() {
        PlayerController.ONDeath += OnDeath;
        
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

            _players[i].GetComponent<Player>().CreatePlayerHUD(_hudManager);
            
            GameObject spawnPref = Instantiate(playerPrefab, _players[i].transform);
            spawnPref.transform.position = spawnPoints.transform.GetChild(i).position;
            _camGroup.AddMember(spawnPref.transform, 1, 0);
        }

    }

    private void CheckGameOver () {
        int left  = _livesLeft.Count(kvp => kvp.Value > 0);

        if (left == 1) {
            //Game is over
            StartCoroutine(Restart());
        }

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
        spawnPref.transform.position = spawnPoints.transform.GetChild(0).position;
        _camGroup.AddMember(spawnPref.transform, 1, 0);
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
        PlayerController.ONDeath -= OnDeath;
    }
}
