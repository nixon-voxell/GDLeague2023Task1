using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_UI : MonoBehaviour
{
    public AbstractSkill thunderSkill;
    private Player[] players;

    private void Start()
    {

        
    }

    private void StartGame()
    {
        Debug.Log("Game Start!");
        GameManager.Instance.UIManager.OnGameStart();
    }

    private void SetupGame()
    {
        players = GameObject.FindObjectsOfType(typeof(Player)) as Player[];

        Debug.Log("setup game");
        GameManager.Instance.UIManager.ResetAllUI();
        GameManager.Instance.UIManager.OnSkillChange(1, 1, thunderSkill);
        Invoke("StartGame", 1.0f);
        players[0].SetHealth(100);
        Debug.Log("Player Health Hooked: " + players[0].PlayerNumber);

    }

    private void Update()
    {
        var kb = Keyboard.current;

        if (kb.digit1Key.wasPressedThisFrame)
        {
            GameManager.Instance.UIManager.OnSkillChange(1, 1, thunderSkill);
        }
        if (kb.digit2Key.wasPressedThisFrame)
        {
            GameManager.Instance.UIManager.OnSkillUsed(1, 1);
        }
        if (kb.digit3Key.wasPressedThisFrame)
        {
            players[0].Damage(5);

        }
        if (kb.digit4Key.wasPressedThisFrame)
        {
            GameManager.Instance.UIManager.OnAbilityUsed(1, "KNOCKBACK");
            GameManager.Instance.UIManager.OnAbilityUsed(1, "DASH");
        }
        if (kb.digit5Key.wasPressedThisFrame)
        {
            GameManager.Instance.UIManager.OnSkillExpire(1, 1);
        }
        if (kb.digit6Key.wasPressedThisFrame)
        {
            GameManager.Instance.UIManager.OnAbilityDoneCD(1, "KNOCKBACK");
            GameManager.Instance.UIManager.OnAbilityDoneCD(1, "DASH");
        }
        if (kb.zKey.wasPressedThisFrame)
        {
            SetupGame();
        }

        if (kb.digit7Key.wasPressedThisFrame)
        {
            GameManager.Instance.UIManager.SetTimer(100);
        }

        if (kb.digit8Key.wasPressedThisFrame)
        {
            GameManager.Instance.UIManager.SetScore(1, 2);
        }
    }

}
