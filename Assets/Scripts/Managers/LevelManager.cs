using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private string m_LevelSceneName; // TODO: Change the scene name to the appropriate level name
    [SerializeField] private List<DestructableObstacle> m_DestructableObstacle = new List<DestructableObstacle>();
    [SerializeField] private SkillSO m_SkillScriptableObject;
    [SerializeField] private Transform m_PlayerOneSpawnPoint;
    [SerializeField] private Transform m_PlayerTwoSpawnPoint;

    private Player[] m_Players;

    public List<DestructableObstacle> DestructableObstacle => this.m_DestructableObstacle;


    private void Awake()
    {
        GameManager.Instance.LevelManager = this;
    }


    /// <summary>
    /// Moves the two player game object to the level scene and then unload the lobby scene
    /// </summary>
    /// <param name="playerGOP1"></param>
    /// <param name="playerGOP2"></param>
    public void MovePlayerToScene(GameObject playerGOP1, GameObject playerGOP2)
    {
        SceneManager.MoveGameObjectToScene(playerGOP1, SceneManager.GetSceneByName(m_LevelSceneName));
        SceneManager.MoveGameObjectToScene(playerGOP2, SceneManager.GetSceneByName(m_LevelSceneName));


        // TODO: To switch this function to the game manager script
        UnloadScene("Lobby");


        this.m_Players = new Player[2];

        this.m_Players[0] = playerGOP1.GetComponent<Player>();
        this.m_Players[1] = playerGOP2.GetComponent<Player>();
        this.m_Players[0].SetupPlayer(1);
        this.m_Players[1].SetupPlayer(2);

        Destroy(playerGOP1.GetComponent<PlayerLobbyController>());
        Destroy(playerGOP2.GetComponent<PlayerLobbyController>());

        m_Players[0].PlayerMovement.SetTransform(m_PlayerOneSpawnPoint);
        m_Players[1].PlayerMovement.SetTransform(m_PlayerTwoSpawnPoint);
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
    }

    public void UnloadScene(string sceneName)
    {
        SceneManager.UnloadSceneAsync(SceneManager.GetSceneByName(sceneName));
    }


}
