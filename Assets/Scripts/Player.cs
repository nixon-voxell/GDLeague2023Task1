using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private int m_MaxHealth = 100;

    // TODO: change to a suitable datatype later on
    [SerializeField] private GameObject[] m_PlayerSkill; // for skill inventory 
    [SerializeField] private GameObject[] m_StatusEffects; // for stun/knockback

    private int m_CurrentHealth;
    private bool m_IsAlive;
    private PlayerInput m_PlayerInput;
    private int m_PlayerNumber;

    public int PlayerNumber
    {
        get { return m_PlayerNumber; }
        set
        {
            if (m_PlayerNumber ==  0)
                m_PlayerNumber = value;
        }
    }
    // Recommendation: To have all sub-script references in the playerscript


    // TODO: change to a suitable datatype later on
    public GameObject[] playerSkill
    {
        get { return m_PlayerSkill; }
        set { m_PlayerSkill = value; }
    }

    // TODO: change to a suitable datatype later on
    public GameObject[] statusEffects
    {
        get { return m_StatusEffects; }
        set { m_StatusEffects = value; }
    }
    private void Start()
    {
        m_PlayerInput = GetComponent<PlayerInput>();

        m_IsAlive = true;
        m_CurrentHealth = m_MaxHealth;
    }
    
    /// <summary>
    /// Setup required stuff of the player
    /// Currently only sets the player number and position
    /// </summary>
    /// <param name="playerNumber"></param>
    public void SetupPlayer(int playerNumber)
    {
        PlayerNumber = playerNumber;
        
        if (playerNumber == 1)
        {
            m_PlayerInput.SwitchCurrentActionMap("PlayerOne");

        }
        else if (playerNumber == 2)
        {
            m_PlayerInput.SwitchCurrentActionMap("PlayerTwo");

        }

    }

    private void OnDeath()
    {
        m_IsAlive  = false;
        Debug.Log("Player dead");
    }

    public void TakeDamage(int damage)
    {
        m_CurrentHealth -= damage;

        if (m_CurrentHealth <= 0)
        {
            m_CurrentHealth = 0;
            OnDeath();
        }
    }

    public void GetPlayerPosition()
    {
        Vector3 playerPosition = transform.position;
    }
}
