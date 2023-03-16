using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : Singleton<GameManager>
{
    [HideInInspector] public LevelManager PlayerManager;

    public static void CreatePlayer(PlayerInput playerInput)
    {
        
    }
}
