using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;

public class LobbyManager : Singleton<LobbyManager>
{
    [SerializeField] private GameObject m_P1EventObject;
    [SerializeField] private MultiplayerEventSystem m_P1MultiplayerESystem;
    [SerializeField] private InputSystemUIInputModule m_P1InputSystemUIInputModule;
    [SerializeField] private GameObject m_P1MainBtnCtrlPanel;
    [SerializeField] private GameObject m_P1JoinInstruction;
    [SerializeField] private GameObject m_P1ReadyToFight;

    //-----

    [SerializeField] private GameObject m_P2EventObject;
    [SerializeField] private MultiplayerEventSystem m_P2MultiplayerESystem;
    [SerializeField] private InputSystemUIInputModule m_P2InputSystemUIInputModule;
    [SerializeField] private GameObject m_P2MainBtnCtrlPanel;
    [SerializeField] private GameObject m_P2JoinInstruction;
    [SerializeField] private GameObject m_P2ReadyToFight;


    private PlayerInput m_P1PlayerInput;
    private PlayerInput m_P2PlayerInput;
    private GameObject m_P1OriginalSelectedBtn;
    private GameObject m_P2OriginalSelectedBtn;
    private bool m_P1Ready = false;
    private bool m_P2Ready = false;

    private void Start()
    {
        m_P1OriginalSelectedBtn = m_P1MultiplayerESystem.firstSelectedGameObject;
        m_P2OriginalSelectedBtn = m_P2MultiplayerESystem.firstSelectedGameObject;
    }

    private void OnPlayerJoined(PlayerInput playerInput)
    {
        Debug.Log("[INPUT] Player Joined: " + playerInput);
        if (m_P1PlayerInput == null)
        {
            m_P1PlayerInput = playerInput;
            m_P1PlayerInput.SwitchCurrentActionMap("UIP1");

            m_P1InputSystemUIInputModule.enabled = true;
            m_P1MultiplayerESystem.SetSelectedGameObject(m_P1OriginalSelectedBtn);
            m_P1PlayerInput.GetComponent<PlayerLobbyController>().PlayerNumber = 1;

            m_P1PlayerInput.uiInputModule = m_P1InputSystemUIInputModule;
            SetCurrentPanel(1, BtnEvent.MAINBUTTONMENU);
        }
        else if (m_P2PlayerInput == null)
        {
            m_P2PlayerInput = playerInput;
            m_P2PlayerInput.SwitchCurrentActionMap("UIP2");

            m_P2InputSystemUIInputModule.enabled = true;
            m_P2MultiplayerESystem.SetSelectedGameObject(m_P2OriginalSelectedBtn);
            m_P2PlayerInput.GetComponent<PlayerLobbyController>().PlayerNumber = 2;

            m_P2PlayerInput.uiInputModule = m_P2InputSystemUIInputModule;
            SetCurrentPanel(2, BtnEvent.MAINBUTTONMENU);
        }
        else
        {
            Debug.LogWarning("[INPUT] Third playerInput detected and created!");
        }
        
    }

    private void OnPlayerLeft(PlayerInput playerInput)
    {
        Debug.Log("Player Left");
    }

    public void BtnPress(BtnEvent btnEvent, int playerNumber)
    {
        Debug.Log(btnEvent + " | " + playerNumber);
        switch (btnEvent)
        {
            case BtnEvent.READYPUP:
                SetCurrentPanel(playerNumber, btnEvent);
                break;
            case BtnEvent.MAINBUTTONMENU:
                SetCurrentPanel(playerNumber, btnEvent);
                break;
            case BtnEvent.QUIT:
                SetCurrentPanel(playerNumber, btnEvent);
                PlayerReset(playerNumber);
                break;
        }
    }

    /// <summary>
    /// Destroys player reset
    /// </summary>
    /// <param name="playerNumber"></param>
    private void PlayerReset(int playerNumber)
    {
        if (playerNumber == 1)
        {
            m_P1InputSystemUIInputModule.enabled = false;
            Destroy(m_P1PlayerInput.gameObject);
            m_P1PlayerInput = null;
        }
        else if (playerNumber == 2)
        {
            m_P2InputSystemUIInputModule.enabled = false;
            Destroy(m_P2PlayerInput.gameObject);
            m_P2PlayerInput = null;
        }
    }

    /// <summary>
    /// Enable disable required UI Game Objects
    /// </summary>
    /// <param name="playerNumber"></param>
    /// <param name="enable"></param>
    private void SetCurrentPanel(int playerNumber, BtnEvent currentPanel)
    {
        if (currentPanel == BtnEvent.MAINBUTTONMENU)
        {
            if (playerNumber == 1)
            {
                m_P1MainBtnCtrlPanel.SetActive(true);
                m_P1JoinInstruction.SetActive(false);
                m_P1ReadyToFight.SetActive(false);
            }
            else if (playerNumber == 2)
            {
                m_P2MainBtnCtrlPanel.SetActive(true);
                m_P2JoinInstruction.SetActive(false);
                m_P2ReadyToFight.SetActive(false);

            }
        }
        else if (currentPanel == BtnEvent.READYPUP)
        {
            if (playerNumber == 1)
            {
                m_P1MainBtnCtrlPanel.SetActive(false);
                m_P1JoinInstruction.SetActive(false);
                m_P1ReadyToFight.SetActive(true);
            }
            else if (playerNumber == 2)
            {
                m_P2MainBtnCtrlPanel.SetActive(false);
                m_P2JoinInstruction.SetActive(false);
                m_P2ReadyToFight.SetActive(true);

            }
        }
        else if (currentPanel == BtnEvent.QUIT)
        {
            if (playerNumber == 1)
            {
                m_P1MainBtnCtrlPanel.SetActive(false);
                m_P1JoinInstruction.SetActive(true);
                m_P1ReadyToFight.SetActive(false);
            }
            else if (playerNumber == 2)
            {
                m_P2MainBtnCtrlPanel.SetActive(false);
                m_P2JoinInstruction.SetActive(true);
                m_P2ReadyToFight.SetActive(false);

            }
        }
    }

    

    

}


public enum BtnEvent
{
    READYPUP, BINDING, QUIT, MAINBUTTONMENU
}