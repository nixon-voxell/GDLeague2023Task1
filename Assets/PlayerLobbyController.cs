using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLobbyController : MonoBehaviour
{
    public int PlayerNumber;
    
    private void OnCancel(InputValue value)
    {
        LobbyManager.Instance.BtnPress(BtnEvent.MAINBUTTONMENU, PlayerNumber);
    }
}
