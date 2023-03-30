using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Mathematics;

public enum GameState
{
    MAP_LOAD, GAME_SETUP, START_COUNTDOWN, FIGHT, PAUSE, ROUND_END, GAME_END
}

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private Camera m_Camera;
    public bool DisableStartToLobby;

    [HideInInspector] public LevelManager LevelManager;
    [HideInInspector] public SoundManager SoundManager;
    [HideInInspector] public UIManager UIManager; // UI Manager for the level UI
    //[HideInInspector] public GameState CurrentGameState;
    

    [SerializeField, Voxell.Util.Scene] private string[] m_InitialScenes;
    [Voxell.Util.Scene] public string MainMenuScene;
    [Voxell.Util.Scene] public string LobbyScene;

    public Camera MainCamera => this.m_Camera;

    private GameState m_CurrentGameState;
    private int[] m_WinningCount = new int[2];

    public GameState CurrentGameState { get => m_CurrentGameState; set => m_CurrentGameState = value; }

    private void Awake()
    {
        // randomize seed
        UnityEngine.Random.InitState(System.DateTime.Now.Millisecond);

        //Application.targetFrameRate = 60;
        //QualitySettings.vSyncCount = 1;

        // get all active scenes
        string[] loadedScenes = new string[SceneManager.sceneCount];
        for (int s = 0; s < loadedScenes.Length; s++)
        {
            loadedScenes[s] = SceneManager.GetSceneAt(s).name;
        }

        // load all scenes if it is not active
        for (int s = 0; s < this.m_InitialScenes.Length; s++)
        {
            if (!System.Array.Exists(loadedScenes, (scene) => scene == this.m_InitialScenes[s]))
            {
                SceneManager.LoadScene(this.m_InitialScenes[s], LoadSceneMode.Additive);
            }
        }
    }

    public void OnMapLoad()
    {
        CurrentGameState = GameState.MAP_LOAD;
        SoundManager.PlayMusic("bgm_fight");
        UIManager.EnableHUD(true);
        UIManager.SetScore(0, 0);
        m_WinningCount[0] = 0;
        m_WinningCount[1] = 0;

        OnGameSetup();
    }

    public void OnGameSetup()
    {
        CurrentGameState = GameState.GAME_SETUP;

        LevelManager.ResetObstacles();
        LevelManager.EnableSpawners(false);
        UIManager.ResetAllUI();
        LevelManager.ResetPlayer();
        UIManager.SetScore(m_WinningCount[0], m_WinningCount[1]);

        StartCoroutine(OnStartCountdown());
    }

    public IEnumerator OnStartCountdown()
    {
        CurrentGameState = GameState.START_COUNTDOWN;
        UIManager.SetCenterText("3");

        yield return new WaitForSeconds(1.0f);
        UIManager.SetCenterText("2");
        yield return new WaitForSeconds(1.0f);
        UIManager.SetCenterText("1");
        yield return new WaitForSeconds(1.0f);
        UIManager.SetCenterText("");

        LevelManager.EnableSpawners(true);
        UIManager.OnGameStart();

        OnFight();
    }

    /// <summary>
    /// For unpause too
    /// </summary>
    public void OnFight()
    {
        Time.timeScale = 1.0f;
        LevelManager.EnablePlayer(true);
        UIManager.EnablePauseScreen(false);
        //LevelManager.EnableUIControls(false);


        CurrentGameState = GameState.FIGHT;
    }

    public void OnPause()
    {
        Time.timeScale = 0;
        LevelManager.EnablePlayer(false);
        UIManager.EnablePauseScreen(true);
        //LevelManager.EnableUIControls(true);


        CurrentGameState = GameState.PAUSE;
    }

    /// <summary>
    /// Rounds are ended on two ways
    /// - Player death
    /// - Round time end
    /// </summary>
    /// <param name="playerWinner">playerNumber [1,2 or -1 if timer finish]</param>
    public void OnRoundEnd(int playerWinner)
    {
        CurrentGameState = GameState.ROUND_END;
        UIManager.OnRoundEnd();
        LevelManager.EnablePlayer(false);
        LevelManager.EnableSpawners(false);

        // Check Victory Condition

        int actualPlayerWinner = -1;

        // Check who wins the round based on hp
        if (playerWinner == -1)
        {
            if (LevelManager.Players[0].CurrentHealth != LevelManager.Players[1].CurrentHealth)
            {
                actualPlayerWinner = LevelManager.Players[0].CurrentHealth > LevelManager.Players[1].CurrentHealth ? 1 : 2;
                m_WinningCount[actualPlayerWinner - 1]++;
                // Award point to the winner
                UIManager.SetScore(m_WinningCount[0], m_WinningCount[1]);
                UIManager.SetCenterText(String.Format("PLAYER {0} WINS", actualPlayerWinner));
            }
            else if (LevelManager.Players[0].CurrentHealth == LevelManager.Players[1].CurrentHealth)
            {
                UIManager.SetCenterText(String.Format("DRAW", playerWinner));

            }

        }

        // stop player from moving
        this.LevelManager.Players[0].PlayerMovement.SetMoveDirection(float2.zero);
        this.LevelManager.Players[1].PlayerMovement.SetMoveDirection(float2.zero);

        // Check game end

        if (m_WinningCount[0] == 2 || m_WinningCount[1] == 2)
            StartCoroutine(OnGameEnd(actualPlayerWinner));
        else
            Invoke("OnGameSetup", 3.0f);

    }



    public IEnumerator OnGameEnd(int playerWinner)
    {
        yield return new WaitForSeconds(3.0f);
        UIManager.SetCenterText("");
        UIManager.EnableEndGameScreen(true, playerWinner);
        LevelManager.ResetObstacles(); // For the main menu to use
        //LevelManager.EnableUIControls(true);

        CurrentGameState = GameState.GAME_END;
    }

    


}

