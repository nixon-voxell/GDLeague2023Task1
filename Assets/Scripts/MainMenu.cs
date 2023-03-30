using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public AudioMixer AudioMixer;
    public Slider slider;
    private void Start()
    {
        float volume;
        AudioMixer.GetFloat("MainVolume", out volume);
        slider.value = volume;

    }

    public void PlayGame()
    {
        SceneManager.UnloadSceneAsync(GameManager.Instance.MainMenuScene);

        if (!GameManager.Instance.DisableStartToLobby)
        {
            SceneManager.LoadSceneAsync(GameManager.Instance.LobbyScene, LoadSceneMode.Additive);
        }

       
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void AudioSetting(float audioLevel)
    {
        AudioMixer.SetFloat("MainVolume", audioLevel);
    }
}