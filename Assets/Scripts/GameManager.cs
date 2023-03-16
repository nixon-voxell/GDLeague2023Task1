using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : Singleton<GameManager>
{
    [HideInInspector] public PlayerManager PlayerManager;

    public static void CreatePlayer(PlayerInput playerInput)
    {
        
    }
}
