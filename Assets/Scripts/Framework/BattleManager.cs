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
    private List<GameObject> _players;

    private Dictionary<GameObject, int> livesLeft; 
    
    private CinemachineTargetGroup camGroup;

    public GameObject camGroupObj;
    public GameObject playerPrefab;
    public GameObject spawnPoints;
    
    private void Awake() {
        livesLeft = new Dictionary<GameObject, int>();
        camGroup = camGroupObj.GetComponent<CinemachineTargetGroup>();
        PlayerController.ONDeath += OnDeath;
    }

    public void StartMatch(List<GameObject> players)
    {
        _players = new List<GameObject>(players);

        for (int i = 0; i < _players.Count; i++)
        {
            livesLeft.Add(_players[i], 3);
            GameObject spawnPref = Instantiate(playerPrefab, _players[i].transform);
            spawnPref.transform.position = spawnPoints.transform.GetChild(i).position;
            camGroup.AddMember(spawnPref.transform.GetChild(2), 1, 0);
        }

    }

    private void CheckGameOver () {
        int left  = livesLeft.Count(kvp => kvp.Value > 0);

        if (left == 1) {
            //Game is over
            StartCoroutine(Restart());
        }

    }

    public void OnDeath (GameObject player) {
        
        livesLeft[player.transform.parent.parent.gameObject] = livesLeft[player.transform.parent.parent.gameObject] - 1;
        
        CheckGameOver();
        
        if (livesLeft[player.transform.parent.gameObject] > 0) {
            StartCoroutine(Respawn(player.transform.parent.parent.gameObject));
        }
        
        Destroy(player.transform.parent);
    }

    private IEnumerator Respawn (GameObject player) {
        yield return new WaitForSeconds(2f);
        GameObject spawnPref = Instantiate(playerPrefab, player.transform);
        spawnPref.transform.position = spawnPoints.transform.GetChild(0).position;
        camGroup.AddMember(spawnPref.transform.GetChild(2), 1, 0);
    }


    private IEnumerator Restart () {
        yield return new WaitForSeconds(1f);

        foreach (GameObject player in _players)
        {
            Destroy(player);
        }
        
        Destroy(GameObject.Find("LobbyManager").gameObject);

        SceneManager.LoadScene("Lobby");
    }

}
