using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

public class LobbyManager : Singleton<LobbyManager>
{
    [SerializeField] private GameObject m_PlayerPrefab;

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


    [Header("Special P2 Var")]
    [SerializeField] private ColorBlock m_Player2SpecialColor;
    private PlayerLobbyController m_P2LobbyController;


    private PlayerInput m_P1PlayerInput;
    private PlayerInput m_P2PlayerInput;
    private GameObject m_P1OriginalSelectedBtn;
    private GameObject m_P2OriginalSelectedBtn;
    private bool m_P1Ready = false;
    private bool m_P2Ready = false;
    private LobbyPage m_P1LobbyPage;
    private LobbyPage m_P2LobbyPage;


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
            if (m_P1PlayerInput.currentControlScheme == "DefaultKeyboard")
            {

                m_P1PlayerInput.SwitchCurrentControlScheme("KeyboardOne", new InputDevice[]{ Keyboard.current, Mouse.current});
                //m_P1PlayerInput = null;
                //PlayerInput newPlayerInput = PlayerInput.Instantiate(m_PlayerPrefab, controlScheme: "Keyboard", pairWithDevices: [Keyboard.current, Mouse.current]);
                //return;
            }

            m_P1InputSystemUIInputModule.enabled = true;
            m_P1MultiplayerESystem.SetSelectedGameObject(m_P1OriginalSelectedBtn);
            m_P1PlayerInput.GetComponent<PlayerLobbyController>().PlayerNumber = 1;

            m_P1PlayerInput.uiInputModule = m_P1InputSystemUIInputModule;
            SetCurrentPanel(1, LobbyPage.MAINBUTTONMENU);
        }
        else if (m_P2PlayerInput == null)
        {
            m_P2PlayerInput = playerInput;
            m_P2PlayerInput.SwitchCurrentActionMap("UIP2");
            if (m_P2PlayerInput.currentControlScheme == "DefaultKeyboard")
            {

                m_P2PlayerInput.SwitchCurrentControlScheme("KeyboardTwo", new InputDevice[] { Keyboard.current, Mouse.current });
                //m_P1PlayerInput = null;
                //PlayerInput newPlayerInput = PlayerInput.Instantiate(m_PlayerPrefab, controlScheme: "Keyboard", pairWithDevices: [Keyboard.current, Mouse.current]);
                //return;
            }


            m_P2InputSystemUIInputModule.enabled = true;
            m_P2LobbyController = m_P2PlayerInput.GetComponent<PlayerLobbyController>();
            m_P2MultiplayerESystem.SetSelectedGameObject(m_P2OriginalSelectedBtn);
            m_P2LobbyController.PlayerNumber = 2;
            m_P2PlayerInput.uiInputModule = m_P2InputSystemUIInputModule;
            SetCurrentPanel(2, LobbyPage.MAINBUTTONMENU);

        }
        else
        {
            Debug.LogWarning("[INPUT] Third playerInput detected and created!");
        }
        
    }


    private void OnPlayerLeft(PlayerInput playerInput)
    {
        if (m_P1PlayerInput == playerInput)
        {
            Debug.Log("p1 left");
        }
        else if (m_P2PlayerInput == playerInput)
        {
            Debug.Log("P2 left");

        }
    }

    public void BtnPress(LobbyPage btnEvent, int playerNumber)
    {
        Debug.Log(btnEvent + " | " + playerNumber);
        switch (btnEvent)
        {
            case LobbyPage.READYPUP:
                SetCurrentPanel(playerNumber, btnEvent);

                break;
            case LobbyPage.MAINBUTTONMENU:
                SetCurrentPanel(playerNumber, btnEvent);
                break;
            case LobbyPage.QUIT:
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
    private void SetCurrentPanel(int playerNumber, LobbyPage changeToPanel)
    {
        if (changeToPanel == LobbyPage.MAINBUTTONMENU)
        {
            if (playerNumber == 1 && m_P1LobbyPage != LobbyPage.MAINBUTTONMENU)
            {
                m_P1MainBtnCtrlPanel.SetActive(true);
                m_P1JoinInstruction.SetActive(false);
                m_P1ReadyToFight.SetActive(false);
                m_P1LobbyPage = LobbyPage.MAINBUTTONMENU;
            }
            else if (playerNumber == 2 && m_P2LobbyPage != LobbyPage.MAINBUTTONMENU)
            {
                m_P2MainBtnCtrlPanel.SetActive(true);
                m_P2JoinInstruction.SetActive(false);
                m_P2ReadyToFight.SetActive(false);
                m_P2LobbyController.LoadSpecialP2Setup(m_P2OriginalSelectedBtn.GetComponent<Button>(), m_Player2SpecialColor);
                m_P2LobbyPage = LobbyPage.MAINBUTTONMENU;

            }
        }
        else if (changeToPanel == LobbyPage.READYPUP)
        {
            if (playerNumber == 1 && m_P1LobbyPage != LobbyPage.READYPUP)
            {
                m_P1MainBtnCtrlPanel.SetActive(false);
                m_P1JoinInstruction.SetActive(false);
                m_P1ReadyToFight.SetActive(true);
                m_P1LobbyPage = LobbyPage.READYPUP;

            }
            else if (playerNumber == 2 && m_P2LobbyPage != LobbyPage.READYPUP)
            {
                m_P2MainBtnCtrlPanel.SetActive(false);
                m_P2JoinInstruction.SetActive(false);
                m_P2ReadyToFight.SetActive(true);
                m_P2LobbyPage = LobbyPage.READYPUP;
            }
        }
        else if (changeToPanel == LobbyPage.QUIT)
        {
            if (playerNumber == 1 && m_P1LobbyPage != LobbyPage.QUIT)
            {
                m_P1MainBtnCtrlPanel.SetActive(false);
                m_P1JoinInstruction.SetActive(true);
                m_P1ReadyToFight.SetActive(false);
                m_P1LobbyPage = LobbyPage.QUIT;
            }
            else if (playerNumber == 2 && m_P2LobbyPage != LobbyPage.QUIT)
            {
                m_P2MainBtnCtrlPanel.SetActive(false);
                m_P2JoinInstruction.SetActive(true);
                m_P2ReadyToFight.SetActive(false);
                m_P2LobbyPage = LobbyPage.QUIT;
            }
        }
    }

    

    

}


public enum LobbyPage
{
    READYPUP, BINDING, QUIT, MAINBUTTONMENU
}