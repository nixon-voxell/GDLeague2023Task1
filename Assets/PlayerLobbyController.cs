using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerLobbyController : MonoBehaviour
{
    public int PlayerNumber;

    private ColorBlock m_OriginalPlayer2Color;
    private Button m_Button;
    private bool m_P2Navigated = false;
    private ColorBlock m_HackPlayer2Color;
    private bool m_firstP2Setup = true;


    private void OnCancel(InputValue value)
    {
        LobbyManager.Instance.BtnPress(LobbyPage.MAINBUTTONMENU, PlayerNumber);
    }

    public void LoadSpecialP2Setup(Button p2Button, ColorBlock hackPlayer2Color)
    {
        

        if (m_firstP2Setup)
        {
            m_Button = p2Button;
            m_OriginalPlayer2Color = m_Button.colors;
            m_HackPlayer2Color = hackPlayer2Color;
            m_firstP2Setup=false;
        }
        m_Button.colors = m_HackPlayer2Color;
        m_P2Navigated = false;
    }

    private void OnNavigate(InputValue value)
    {
        if (PlayerNumber == 2  && !m_P2Navigated)
        {
            m_Button.colors = m_OriginalPlayer2Color;
            m_P2Navigated = true;
        }
    }

}
