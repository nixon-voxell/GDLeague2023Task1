using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.VFX;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private List<DestructableObstacle> m_DestructableObstacle = new List<DestructableObstacle>();
    [SerializeField] private SkillSO m_SkillScriptableObject;
    [SerializeField] private VisualEffect m_VisualEffectPrefab;

    private VisualEffect[] m_VisualEffectPool;
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
        // move them to the current scene
        SceneManager.MoveGameObjectToScene(playerGOP1, this.gameObject.scene);
        SceneManager.MoveGameObjectToScene(playerGOP2, this.gameObject.scene);

        // TODO: To switch this function to the game manager script
        SceneManager.UnloadSceneAsync(SceneManager.GetSceneByName("Lobby"));

        this.m_Players = new Player[2];

        this.m_Players[0] = playerGOP1.GetComponent<Player>();
        this.m_Players[1] = playerGOP2.GetComponent<Player>();
        this.m_Players[0].SetupPlayer(1);
        this.m_Players[1].SetupPlayer(2);

        Destroy(playerGOP1.GetComponent<PlayerLobbyController>());
        Destroy(playerGOP2.GetComponent<PlayerLobbyController>());
    }
}
