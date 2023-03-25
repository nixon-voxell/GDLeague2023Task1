using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private GridSystem m_GridSystem;
    [SerializeField] private string m_LevelSceneName; // TODO: Change the scene name to the appropriate level name
    [SerializeField] private List<DestructibleObstacle> m_DestructibleObstacles = new List<DestructibleObstacle>();

    public GridSystem GridSystem => this.m_GridSystem;
    public List<DestructibleObstacle> DestructibleObstacles => this.m_DestructibleObstacles;


    private void Start()
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
        SceneManager.UnloadSceneAsync(SceneManager.GetSceneByName("Lobby"));

        Player p1Script = playerGOP1.GetComponent<Player>();
        Player p2Script = playerGOP2.GetComponent<Player>();
        p1Script.SetupPlayer(1);
        p2Script.SetupPlayer(2);

        Destroy(playerGOP1.GetComponent<PlayerLobbyController>());
        Destroy(playerGOP2.GetComponent<PlayerLobbyController>());

    }
}
