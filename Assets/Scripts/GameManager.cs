using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private Camera m_Camera;
    [HideInInspector] public LevelManager PlayerManager;

    public Camera MainCamera => this.m_Camera;

    private void Awake()
    {
        
    }
}
