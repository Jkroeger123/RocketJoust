using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class GlobalHUDManager : MonoBehaviour
{
    public GameObject playerHUDParent;

    public GameObject playerHUDPrefab;

    public float threshold;

    private Dictionary<GameObject, Tween> _hudTweenMap;
    private List<GameObject> _players;
    private Camera _mainCam;
    
    private void Start()
    {
        _hudTweenMap = new Dictionary<GameObject, Tween>();
        _players = GetComponent<BattleManager>().GetPlayers();
        _mainCam = Camera.main;
    }

    public PlayerHUD CreatePlayerHUD()
    {
        return Instantiate(playerHUDPrefab, playerHUDParent.transform).GetComponent<PlayerHUD>();
    }

    private void Update()
    {
        UpdateUiTransparency();
    }

    private void UpdateUiTransparency()
    {
        foreach (Transform child in playerHUDParent.transform)
        {
            if (IsUiBlockingPlayer(child.gameObject))
            {
                //make transparent

                if (_hudTweenMap.ContainsKey(child.gameObject))
                {
                    _hudTweenMap[child.gameObject].Kill();
                    _hudTweenMap.Remove(child.gameObject);
                }
                
                Tween hide = child.GetComponent<CanvasGroup>().DOFade(0.4f, 0.3f);
                _hudTweenMap.Add(child.gameObject, hide);
            }
            else
            {
                //make not transparent
                if (_hudTweenMap.ContainsKey(child.gameObject))
                {
                    _hudTweenMap[child.gameObject].Kill();
                    _hudTweenMap.Remove(child.gameObject);
                }
                
                Tween hide = child.GetComponent<CanvasGroup>().DOFade(1f, 0.3f);
                _hudTweenMap.Add(child.gameObject, hide);
            }
        }
    }

    private bool IsUiBlockingPlayer(GameObject ui)
    {
        foreach (GameObject o in _players)
        {
            if (o.transform.childCount == 0) continue;
            
            Vector2 worldPos = _mainCam.ScreenToWorldPoint(ui.GetComponent<RectTransform>().position);
            if (Vector2.Distance(worldPos, o.transform.GetChild(0).position) <= threshold) return true;
        }

        return false;
    }
    

    public void HideHUD() => playerHUDParent.SetActive(false);
    public void ShowHUD() => playerHUDParent.SetActive(true);

}
