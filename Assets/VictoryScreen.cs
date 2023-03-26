using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryScreen : MonoBehaviour
{
    public void BackToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Scenes/MainMenu");
    }

    public void QuitTheGame()
    {
        Application.Quit();
    }
}
