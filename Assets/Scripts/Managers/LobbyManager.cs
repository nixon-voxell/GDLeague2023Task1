using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;
using System;

public class LobbyManager : Singleton<LobbyManager>
{
    [SerializeField] private GameObject m_PlayerPrefab;

    [SerializeField] private MultiplayerEventSystem m_P1MultiplayerESystem;
    [SerializeField] private InputSystemUIInputModule m_P1InputSystemUIInputModule;
    [SerializeField] private GameObject m_P1MainBtnCtrlPanel;
    [SerializeField] private GameObject m_P1JoinInstruction;
    [SerializeField] private GameObject m_P1ReadyToFight;

    //-----

    [SerializeField] private MultiplayerEventSystem m_P2MultiplayerESystem;
    [SerializeField] private InputSystemUIInputModule m_P2InputSystemUIInputModule;
    [SerializeField] private GameObject m_P2MainBtnCtrlPanel;
    [SerializeField] private GameObject m_P2JoinInstruction;
    [SerializeField] private GameObject m_P2ReadyToFight;


    [Header("Special P1 Var")]
    [SerializeField] private ColorBlock m_Player1SpecialColor;
    private PlayerLobbyController m_P1LobbyController;
    
    [Header("Special P2 Var")]
    [SerializeField] private ColorBlock m_Player2SpecialColor;
    private PlayerLobbyController m_P2LobbyController;


    private PlayerInput m_P1PlayerInput;
    private PlayerInput m_P2PlayerInput;
    private GameObject m_P1OriginalSelectedBtn;
    private GameObject m_P2OriginalSelectedBtn;
    private LobbyPage m_P1LobbyPage = LobbyPage.QUIT;
    private LobbyPage m_P2LobbyPage = LobbyPage.QUIT;


    private void Start()
    {
        m_P1OriginalSelectedBtn = m_P1MultiplayerESystem.firstSelectedGameObject;
        m_P2OriginalSelectedBtn = m_P2MultiplayerESystem.firstSelectedGameObject;

    }

    private void OnJoinLobbyKb1(InputValue value)
    {
        if (m_P1PlayerInput == null)
        {
            m_P1PlayerInput = PlayerInput.Instantiate(m_PlayerPrefab, controlScheme: "Keyboard", pairWithDevices: new InputDevice[] { Keyboard.current, Mouse.current});
            SetupPlayerUIInput(1);
            // GameManager.Instance.PlayerManager.MovePlayerToScene(m_P1PlayerInput.gameObject);
        }
       
    }

   
    private void OnJoinLobbyKb2(InputValue value)
    {
        if (m_P2PlayerInput == null)
        {
            m_P2PlayerInput = PlayerInput.Instantiate(m_PlayerPrefab, controlScheme: "Keyboard", pairWithDevices: new InputDevice[] { Keyboard.current, Mouse.current });
            SetupPlayerUIInput(2);
        }
    }


    private void OnPlayerJoined(PlayerInput playerInput)
    {
        //Destroy player input if exceed max 2
        if (m_P1PlayerInput != null & m_P2PlayerInput != null)
        {
            Debug.LogWarning("Exceeded 2 player input");
            Destroy (playerInput.gameObject);
        }

        if (playerInput.currentControlScheme == "Controller")
        {
            if (m_P1PlayerInput == null)
            {
                m_P1PlayerInput = playerInput;
                SetupPlayerUIInput(1);
            }
            else if (m_P2PlayerInput == null)
            {
                m_P2PlayerInput = playerInput;
                SetupPlayerUIInput(2);
            }
        }
    }

    private void SetupPlayerUIInput(int playerNumber)
    {
        if (playerNumber == 1)
        {
            m_P1PlayerInput.SwitchCurrentActionMap("UIP1");
            m_P1InputSystemUIInputModule.enabled = true;
            m_P1LobbyController = m_P1PlayerInput.GetComponent<PlayerLobbyController>();

            m_P1MultiplayerESystem.SetSelectedGameObject(m_P1OriginalSelectedBtn);
            m_P1LobbyController.PlayerNumber = 1;

            m_P1PlayerInput.uiInputModule = m_P1InputSystemUIInputModule;
            SetCurrentPanel(1, LobbyPage.MAIN_BUTTON_MENU);
        }
        else if (playerNumber == 2)
        {
            m_P2PlayerInput.SwitchCurrentActionMap("UIP2");
            m_P2InputSystemUIInputModule.enabled = true;
            m_P2LobbyController = m_P2PlayerInput.GetComponent<PlayerLobbyController>();

            m_P2MultiplayerESystem.SetSelectedGameObject(m_P2OriginalSelectedBtn);
            m_P2LobbyController.PlayerNumber = 2;

            m_P2PlayerInput.uiInputModule = m_P2InputSystemUIInputModule;
            SetCurrentPanel(2, LobbyPage.MAIN_BUTTON_MENU);
        }
    }

    private void OnPlayerLeft(PlayerInput playerInput)
    {
        if (m_P1PlayerInput == playerInput)
        {
            Debug.Log("[CONTROLLER] P1 left");
        }
        else if (m_P2PlayerInput == playerInput)
        {
            Debug.Log("[CONTROLLER] P2 left");

        }
    }

    public void BtnPress(LobbyPage btnEvent, int playerNumber)
    {
        switch (btnEvent)
        {
            case LobbyPage.READYUP:
                SetCurrentPanel(playerNumber, btnEvent);
                CheckPlayerReadyUp();
                break;
            case LobbyPage.MAIN_BUTTON_MENU:
                SetCurrentPanel(playerNumber, btnEvent);
                break;
            case LobbyPage.QUIT:
                SetCurrentPanel(playerNumber, btnEvent);
                PlayerReset(playerNumber);
                break;
        }
    }

    private void CheckPlayerReadyUp()
    {
        // Check if both player readied up
        if (m_P1LobbyPage == LobbyPage.READYUP && m_P2LobbyPage == LobbyPage.READYUP)
        {
            GameManager.Instance.LevelManager.MovePlayerToScene(m_P1PlayerInput.gameObject, m_P2PlayerInput.gameObject);
        
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
        if (changeToPanel == LobbyPage.MAIN_BUTTON_MENU)
        {
            if (playerNumber == 1 && m_P1LobbyPage != LobbyPage.MAIN_BUTTON_MENU)
            {
                m_P1MainBtnCtrlPanel.SetActive(true);
                m_P1JoinInstruction.SetActive(false);
                m_P1ReadyToFight.SetActive(false);
                m_P1LobbyController.LoadSpecialLobbySetup(m_P1OriginalSelectedBtn.GetComponent<Button>(), m_Player1SpecialColor, m_P1MultiplayerESystem);
                m_P1LobbyPage = LobbyPage.MAIN_BUTTON_MENU;
            }
            else if (playerNumber == 2 && m_P2LobbyPage != LobbyPage.MAIN_BUTTON_MENU)
            {
                m_P2MainBtnCtrlPanel.SetActive(true);
                m_P2JoinInstruction.SetActive(false);
                m_P2ReadyToFight.SetActive(false);
                m_P2LobbyController.LoadSpecialLobbySetup(m_P2OriginalSelectedBtn.GetComponent<Button>(), m_Player2SpecialColor, m_P2MultiplayerESystem);
                m_P2LobbyPage = LobbyPage.MAIN_BUTTON_MENU;

            }
        }
        else if (changeToPanel == LobbyPage.READYUP)
        {
            if (playerNumber == 1 && m_P1LobbyPage != LobbyPage.READYUP)
            {
                m_P1MainBtnCtrlPanel.SetActive(false);
                m_P1JoinInstruction.SetActive(false);
                m_P1ReadyToFight.SetActive(true);
                m_P1LobbyPage = LobbyPage.READYUP;

            }
            else if (playerNumber == 2 && m_P2LobbyPage != LobbyPage.READYUP)
            {
                m_P2MainBtnCtrlPanel.SetActive(false);
                m_P2JoinInstruction.SetActive(false);
                m_P2ReadyToFight.SetActive(true);
                m_P2LobbyPage = LobbyPage.READYUP;
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

    public void ReturnToMainMenu()
    {

        GameManager.Instance.LevelManager.UnloadScene("Lobby");
        GameManager.Instance.LevelManager.LoadScene("MainMenu");
    }

    

}


public enum LobbyPage
{
    READYUP, BINDING, QUIT, MAIN_BUTTON_MENU
}