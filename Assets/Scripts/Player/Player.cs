using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Mathematics;

public enum PlayerStatus
{
    Default,
    Immobilized,
    Dead,
}

public class Player : MonoBehaviour
{
    [SerializeField] private int m_MaxHealth = 100;
    [SerializeField] private PlayerMovement m_PlayerMovement;

    private PlayerInput m_PlayerInput;

    private PlayerStatus m_PlayerStatus = PlayerStatus.Default;
    private int m_PlayerNumber;
    private int m_CurrentHealth;

    // index of skill in scriptable obejct
    private int[] m_PlayerSkills;

    public PlayerMovement PlayerMovement => this.m_PlayerMovement;
    public PlayerStatus PlayerStatus => this.m_PlayerStatus;
    public int PlayerNumber => this.m_PlayerNumber;
    public int CurrentHealth => this.m_CurrentHealth;

    /// <summary>
    /// Setup required stuff of the player
    /// Currently only sets the player number and position
    /// </summary>
    /// <param name="playerNumber"></param>
    public void SetupPlayer(int playerNumber)
    {
        this.m_PlayerInput = this.GetComponent<PlayerInput>();
        this.m_PlayerMovement = this.GetComponent<PlayerMovement>();

        this.m_PlayerStatus = PlayerStatus.Default;
        this.m_CurrentHealth = m_MaxHealth;

        this.m_PlayerNumber = playerNumber;
        
        if (playerNumber == 1)
        {
            m_PlayerInput.SwitchCurrentActionMap("PlayerOne");
        } else if (playerNumber == 2)
        {
            m_PlayerInput.SwitchCurrentActionMap("PlayerTwo");
        }
    }

    private void OnMovement(InputValue value)
    {
        float2 moveValue = value.Get<Vector2>();
        this.m_PlayerMovement.SetMoveDirection(moveValue);
    }

    private void OnDash(InputValue value)
    {
        if (value.isPressed)
        {
            this.m_PlayerMovement.Dash();
        }
    }

    private void OnDeath()
    {
        this.m_PlayerStatus = PlayerStatus.Dead;
        Debug.Log("Player dead");
    }

    public void Damage(int damage)
    {
        this.SetHealth(this.CurrentHealth - damage);
    }

    public void SetHealth(int health)
    {
        this.m_CurrentHealth = health;

        if (this.m_CurrentHealth <= 0)
        {
            this.m_CurrentHealth = 0;
            OnDeath();
        }
    }

    /// <summary>Get skill at index.</summary>
    public int GetSkill(int skillIdx)
    {
        Debug.Assert(skillIdx < 3, "Skill index must not exceed 2.");
        return this.m_PlayerSkills[skillIdx];
    }
}
