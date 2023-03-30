using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{

    public void Resume()
    {
        GameManager.Instance.OnFight();
        GameManager.Instance.SoundManager.PlayOneShot("sfx_button_click");

    }

    void Pause()
    {
        GameManager.Instance.OnPause();
        GameManager.Instance.SoundManager.PlayOneShot("sfx_button_click");

    }

    public void LoadMenu()
    {
        GameManager.Instance.LevelManager.UnloadLevel();
        GameManager.Instance.SoundManager.PlayOneShot("sfx_button_click");

    }

    public void QuitGame()
    {
        Application.Quit();
        GameManager.Instance.SoundManager.PlayOneShot("sfx_button_click");

    }
}
