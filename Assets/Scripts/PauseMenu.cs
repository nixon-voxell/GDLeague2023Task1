using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{

    public void Resume()
    {
        GameManager.Instance.OnFight();
    }

    void Pause()
    {
        GameManager.Instance.OnPause();
    }

    public void LoadMenu()
    {
        GameManager.Instance.LevelManager.UnloadLevel();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
