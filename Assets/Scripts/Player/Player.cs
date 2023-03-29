using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Mathematics;
using System.Collections;

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
        // move only when status is default
        if (this.m_PlayerStatus != PlayerStatus.Default) return;

        float2 moveValue = value.Get<Vector2>();
        this.m_PlayerMovement.SetMoveDirection(moveValue);
    }

    private void OnDash(InputValue value)
    {
        if (this.m_PlayerStatus != PlayerStatus.Default) return;

        if (value.isPressed)
        {
            this.StartCoroutine(this.m_PlayerMovement.Dash());
        }
    }

    private void OnSkillOne(InputValue value)
    {
        if (this.m_PlayerStatus != PlayerStatus.Default) return;
        if (!value.isPressed) return;

        this.ActivateSkillIfExist(0);
    }

    private void OnSkillTwo(InputValue value)
    {
        if (this.m_PlayerStatus != PlayerStatus.Default) return;
        if (!value.isPressed) return;

        this.ActivateSkillIfExist(1);
    }

    private void OnSkillThree(InputValue value)
    {
        if (this.m_PlayerStatus != PlayerStatus.Default) return;
        if (!value.isPressed) return;

        this.ActivateSkillIfExist(2);
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
        GameManager.Instance.UIManager.SetHealth(m_PlayerNumber, this.m_CurrentHealth);

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

    public void OnCollisionEnter(Collision collision)
    {
        // Debug.Log(collision.gameObject.name);
    }

    public void SetImmune(bool immune)
    {
        if (immune)
        {
            this.m_PlayerStatus = PlayerStatus.Immobilized;
            StartCoroutine(RemoveImmunity());
        }
        else
        {
            this.m_PlayerStatus = PlayerStatus.Default;
        }
    }

    private void ActivateSkillIfExist(int playerSkillIdx)
    {
        int skillIdx = this.GetSkill(playerSkillIdx);
        if (skillIdx != -1)
        {
            GameManager.Instance.LevelManager.so_Skill.Skills[skillIdx].OnPress(this);
        }
    }

    private IEnumerator RemoveImmunity()
    {
        yield return new WaitForSeconds(3f);
        this.m_PlayerStatus = PlayerStatus.Default;
    }
}
