using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.EventSystems;

public class PauseMenu : MonoBehaviour
{

    
    public static PauseMenu Instance;
    
    public bool isPaused = false;
    public event Action<bool> ONPauseSet;
    public GameObject pauseUI;

    private MMTimeManager _timeManager;

    private void Awake()
    {
        _timeManager = GameObject.Find("Camera").GetComponent<MMTimeManager>();
        
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
        _timeManager.NormalTimescale = Time.timeScale;

        pauseUI.SetActive(false);
        ONPauseSet?.Invoke(false);
    }

    private void Pause()
    {
        Time.timeScale = 0f;
        _timeManager.NormalTimescale = Time.timeScale;
        
        pauseUI.SetActive(true);
        pauseUI.transform.GetChild(0).GetChild(1).gameObject.SetActive(true);
        pauseUI.transform.GetChild(0).GetChild(2).gameObject.SetActive(false);
        EventSystem.current.GetComponent<EventSystem>()
            .SetSelectedGameObject(pauseUI.transform.GetChild(0).GetChild(1).GetChild(0).gameObject);
        ONPauseSet?.Invoke(true);
    }

    public void EndMatch()
    {
        if (isPaused) TogglePause();
        GetComponent<BattleManager>().EndMatch();
    }

}
