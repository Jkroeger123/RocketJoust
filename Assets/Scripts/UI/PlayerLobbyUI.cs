using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLobbyUI : MonoBehaviour
{

    public GameObject readyText;
    public Text playerText;

    private void Start()
    {
        transform.DOLocalMoveY(10f, 0.6f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
    }

    public void SetReadyText(bool isReady)
    {
        readyText.SetActive(isReady);
    }

    public void SetPlayerNumber(int n)
    {
        playerText.text = "P" + n;
    }

    public void UpdateCharacterDisplay(GameObject displayPrefab, Sprite img)
    {
        Destroy(transform.GetChild(0).gameObject);
        Instantiate(displayPrefab, transform).transform.SetSiblingIndex(0);
        SetCharacterImage(img);
    }

    public void SetCharacterImage(Sprite img)
    {
        transform.GetChild(0).GetComponent<Image>().sprite = img;
    }

    public void SetBubbleImage(Sprite img)
    {
        transform.GetChild(2).GetComponent<Image>().sprite = img;
    }

    public void ResetUI()
    {
        readyText.SetActive(false);
    }

}
