using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using Random = UnityEngine.Random;

public class BattleManager : MonoBehaviour
{
    private List<GameObject> _players;
    private CinemachineTargetGroup camGroup;

    public GameObject camGroupObj;
    public GameObject playerPrefab;
    public GameObject spawnPoints;
    
    private void Awake()
    {
        camGroup = camGroupObj.GetComponent<CinemachineTargetGroup>();
    }

    public void StartMatch(List<GameObject> players)
    {
        _players = players;

        for (int i = 0; i < _players.Count; i++)
        {
            GameObject spawnPref = Instantiate(playerPrefab, players[i].transform);
            spawnPref.transform.position = spawnPoints.transform.GetChild(i).position;
            camGroup.AddMember(spawnPref.transform, 1, 0);
        }

    }

}
