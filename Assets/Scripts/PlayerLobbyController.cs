using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

/// <summary>
/// A hack to fix the UI bug due to having multiple event system
/// </summary>
public class PlayerLobbyController : MonoBehaviour
{
    public int PlayerNumber;

    private ColorBlock m_OriginalPlayerColor;
    private Button m_Button;
    private bool m_Navigated = false;
    private ColorBlock m_HackPlayerColor;
    private bool m_firstSetup = true;
    private MultiplayerEventSystem m_MultiplayerEventSystem;


    private void OnCancel(InputValue value)
    {
        LobbyManager.Instance.BtnPress(LobbyPage.MAIN_BUTTON_MENU, PlayerNumber);
    }


    public void LoadSpecialLobbySetup(Button button, ColorBlock hackPlayerColor, MultiplayerEventSystem eventSys)
    {
        if (m_firstSetup)
        {
            m_MultiplayerEventSystem = eventSys;
            m_Button = button;
            m_OriginalPlayerColor = m_Button.colors;
            m_HackPlayerColor = hackPlayerColor;
            m_firstSetup=false;
        }
        m_Button.colors = m_HackPlayerColor;
        m_Navigated = false;
    }

    private void OnNavigate(InputValue value)
    {
        Debug.Log("Navigated: " + value);
        if (!m_Navigated)
        {
            //If selected object is of itself, do not reset the UI to normal color 
            if (m_MultiplayerEventSystem.currentSelectedGameObject == m_Button.gameObject)
                return;

            m_Button.colors = m_OriginalPlayerColor;
            m_Navigated = true;
        }
    }

}
