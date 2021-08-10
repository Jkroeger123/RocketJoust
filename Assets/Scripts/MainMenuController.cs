using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    public GameObject settingsUI;
    public GameObject mainUI;
    public Settings settings;
    
    public void Quit() => Application.Quit();
    public void Play() => SceneManager.LoadScene("NewLobby", LoadSceneMode.Single);

    public void Settings()
    {
        mainUI.SetActive(false);
        settingsUI.SetActive(true);
        EventSystem.current.SetSelectedGameObject(settingsUI.transform.GetChild(0).gameObject);
        settingsUI.transform.GetChild(0).GetComponent<Slider>().value = settings.GetVolume();
    }

    public void Main()
    {
        mainUI.SetActive(true);
        settingsUI.SetActive(false);
        EventSystem.current.SetSelectedGameObject(mainUI.transform.GetChild(0).gameObject);
    }

}
