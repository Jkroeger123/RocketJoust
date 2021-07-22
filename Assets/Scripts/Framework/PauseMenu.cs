using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PauseMenu : MonoBehaviour
{
    public static bool isPaused = false;
    
    public static event Action<bool> ONPauseSet;
    
    public GameObject pauseUI;
    
    private bool _pauseBuffer; //status of pause the previous frame to detect change;
    
    private void Update()
    {

        if (_pauseBuffer == isPaused)
        {
            _pauseBuffer = isPaused;
            return;
        }

        if (isPaused)
        {
            Pause();
        }
        else
        {
            Resume();
        }

        _pauseBuffer = isPaused;
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        pauseUI.SetActive(false);
        ONPauseSet?.Invoke(false);
    }

    public void Pause()
    {
        Time.timeScale = 0f;
        pauseUI.SetActive(true);
        EventSystem.current.GetComponent<EventSystem>()
            .SetSelectedGameObject(pauseUI.transform.GetChild(0).GetChild(1).gameObject);
        ONPauseSet?.Invoke(true);
    }

    public void EndMatch()
    {
        isPaused = false;
        Resume();
        GetComponent<BattleManager>().EndMatch();
    }

}
