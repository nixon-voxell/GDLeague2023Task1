using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryScreen : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_playerWinnerText;

    public void SetPlayerWinner(int playerWinner)
    {
        m_playerWinnerText.text = string.Format("PLAYER {0} WINS!", playerWinner);
    }

    public void BackToMenu()
    {
        GameManager.Instance.LevelManager.UnloadLevel();
    }

    public void Retry()
    {
        GameManager.Instance.OnMapLoad();
    }
}