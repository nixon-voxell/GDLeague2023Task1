using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Connect Button's OnClick event to the ButtonPressed method
/// </summary>
public class LobbyBtnEventTrigger : MonoBehaviour
{
    public int PlayerNumber;
    public LobbyPage BtnEvent;

    public void ButtonPressed()
    {
        LobbyManager.Instance.BtnPress(BtnEvent, PlayerNumber);
    }
}
