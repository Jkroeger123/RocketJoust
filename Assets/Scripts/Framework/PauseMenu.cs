using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PauseMenu : MonoBehaviour
{

    public static PauseMenu Instance;
    
    public bool isPaused = false;
    public event Action<bool> ONPauseSet;
    public GameObject pauseUI;


    private void Awake()
    {
        if (Instance != null) {
            Destroy(gameObject);
        }else{
            Instance = this;
        }
    }
    
    
    public void TogglePause()
    {
        isPaused = !isPaused;
        
        if (isPaused)
        {
            Pause();
        }
        else
        {
            Resume();
        }
    }

    private void Resume()
    {
        Time.timeScale = 1f;
        pauseUI.SetActive(false);
        ONPauseSet?.Invoke(false);
    }

    private void Pause()
    {
        Time.timeScale = 0f;
        pauseUI.SetActive(true);
        EventSystem.current.GetComponent<EventSystem>()
            .SetSelectedGameObject(pauseUI.transform.GetChild(0).GetChild(1).gameObject);
        ONPauseSet?.Invoke(true);
    }

    public void EndMatch()
    {
        if (isPaused) TogglePause();
        GetComponent<BattleManager>().EndMatch();
    }

}
