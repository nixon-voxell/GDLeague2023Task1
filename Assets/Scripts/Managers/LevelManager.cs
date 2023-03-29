using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.VFX;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private List<DestructableObstacle> m_DestructableObstacles = new List<DestructableObstacle>();
    [SerializeField] private List<OrbSpawner> m_OrbSpawners = new List<OrbSpawner>();
    [SerializeField] private SkillSO m_so_Skill;
    [SerializeField] private ObjectPool<VisualEffect> m_VisualEffectPool;
    [SerializeField] private Transform m_PlayerOneSpawnPoint;
    [SerializeField] private Transform m_PlayerTwoSpawnPoint;
    [SerializeField] private int m_MaxRoundTime;

    private Player[] m_Players;

    public List<DestructableObstacle> DestructableObstacles => this.m_DestructableObstacles;
    public List<OrbSpawner> OrbSpawners => this.m_OrbSpawners;
    public SkillSO so_Skill => this.m_so_Skill;
    public ObjectPool<VisualEffect> VisualEffectPool => this.m_VisualEffectPool;

    public int MaxRoundTime { get => m_MaxRoundTime; set => m_MaxRoundTime = value; }
    public Player[] Players { get => m_Players; set => m_Players = value; }

    private void Awake()
    {
        GameManager.Instance.LevelManager = this;

        // initialize vfx pool
        this.m_VisualEffectPool.Initialize(this.transform);
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

        SceneManager.UnloadSceneAsync(GameManager.Instance.LobbyScene);

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

    public void ResetPlayer()
    {
        m_Players[0].PlayerMovement.SetTransform(m_PlayerOneSpawnPoint);
        m_Players[1].PlayerMovement.SetTransform(m_PlayerTwoSpawnPoint);
        m_Players[0].ResetPlayer();
        m_Players[1].ResetPlayer();
    }

    // To disable enable player action for both players
    public void EnablePlayer(bool enable)
    {
        for (int i = 0; i < m_Players.Length; i++)
        {
            m_Players[i].EnablePlayer(enable);
        }
    }

    public void ResetObstacles()
    {
        for (int i = 0; i < m_DestructableObstacles.Count; i++)
        {
            m_DestructableObstacles[i].CreateObstacle();
        }
    }

    public void EnableSpawners(bool enable)
    {
        for (int i = 0; i < m_OrbSpawners.Count; i++)
        {
            if (enable)
                m_OrbSpawners[i].EnableSpawn();
            else
                m_OrbSpawners[i].DisableSpawn();
        }
    }
}
