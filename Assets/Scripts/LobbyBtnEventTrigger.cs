using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Connect Button's OnClick event to the ButtonPressed method
/// Connect this script's UnityBtnEvent to the LobbyManager.ButtonEvent
/// </summary>
public class LobbyBtnEventTrigger : MonoBehaviour
{
    public int PlayerNumber;
    public BtnEvent BtnEvent;

    public void ButtonPressed()
    {
        LobbyManager.Instance.BtnPress(BtnEvent, PlayerNumber);
    }
}
