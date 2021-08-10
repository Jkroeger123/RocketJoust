using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using DG.Tweening;
using MoreMountains.Feedbacks;
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

    public GameObject vCam;
    
    public Image transitionImage;
    
    private List<GameObject> _players;
    private Dictionary<GameObject, int> _livesLeft;

    private GlobalHUDManager _hudManager;
    private CamGroupController _camGroupController;

    private void Awake()
    {
        
        canMove = false;
        
        PlayerDeathHandler.ONDeath += OnDeath;
        
        _livesLeft = new Dictionary<GameObject, int>();

        _hudManager = GetComponent<GlobalHUDManager>();
        _hudManager.HideHUD();
        
        _camGroupController = camGroupObj.GetComponent<CamGroupController>();
    }

    public void StartMatch(List<GameObject> players)
    {
        _players = new List<GameObject>(players);
        transitionImage.fillAmount = 1f;
        
        //Setup CameraWeights for smooth transition
        for (int i = 0; i < _players.Count; i++)
        {
            _camGroupController.AddObject(spawnPoints.transform.GetChild(i).gameObject);
        }
        
        DOTween.To(() => transitionImage.fillAmount, (x) => transitionImage.fillAmount = x, 0f, 0.3f)
            .OnComplete(() => StartCoroutine(SpawnPlayers()));
    }

    private IEnumerator SpawnPlayers()
    {
        
        for (int i = 0; i < _players.Count; i++)
        {
            _livesLeft.Add(_players[i], 3);

            Player pl = _players[i].GetComponent<Player>();
            pl.CreatePlayerHUD(_hudManager);
            
            GameObject spawnPref = Instantiate(pl.characterPrefab, _players[i].transform);
            spawnPref.transform.position = spawnPoints.transform.GetChild(i).position;
            
            _camGroupController.RemoveObject(spawnPoints.transform.GetChild(i).gameObject);
            _camGroupController.AddObject(spawnPref);
        }

        yield return new WaitForSeconds(0.5f);
        
        //ShowHud
        _hudManager.ShowHUD();
        
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
        StartCoroutine(EndSlowdown(winner));
    }

    private IEnumerator EndSlowdown(GameObject winner)
    {
        Time.timeScale = 0.3f;
        Time.fixedDeltaTime = 0.02f * (1 / Time.timeScale);
        GameObject.Find("Camera").GetComponent<MMTimeManager>().NormalTimescale = Time.timeScale;
        
        yield return new WaitForSeconds(0.4f);
        
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f * (1 / Time.timeScale);
        GameObject.Find("Camera").GetComponent<MMTimeManager>().NormalTimescale = Time.timeScale;
        
        yield return new WaitForSeconds(1f);

        StartCoroutine(PraiseWinner(winner));
    }

    private void OnDeath (GameObject player) {
        
        _camGroupController.SmoothRemove(player);
        
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
        
        GameObject spawnPref = Instantiate(player.GetComponent<Player>().characterPrefab, player.transform);
        spawnPref.transform.position = spawnPoints.transform.GetChild(Random.Range(0, spawnPoints.transform.childCount)).position;
        _camGroupController.AddObject(spawnPref);
    }


    private IEnumerator PraiseWinner(GameObject player)
    {
        countDownText.SetActive(true);
        countDownText.GetComponent<Text>().text = "P" + player.GetComponent<Player>().PlayerID + " Wins!";

        countDownText.transform.DOScale(1.4f, 0.4f);

        yield return new WaitForSeconds(4f);
        
        EndMatch();
    }

    public void EndMatch() {

        foreach (GameObject player in _players)
        {
            //Destroy(player);    
            foreach (Transform child in player.transform)
            {
                    Destroy(child.gameObject);
            }
        }
        
        Destroy(GameObject.Find("LobbyManager").gameObject);

        SceneManager.LoadScene("NewLobby");
    }

    public List<GameObject> GetPlayers() => _players;
    
    private void OnDestroy()
    {
        PlayerDeathHandler.ONDeath -= OnDeath;
    }
}
