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
    private float m_SkillExpireTime;

    // index of skill in scriptable obejct
    private int[] m_PlayerSkills = new int[3];

    public PlayerMovement PlayerMovement => this.m_PlayerMovement;
    public PlayerStatus PlayerStatus => this.m_PlayerStatus;
    public int PlayerNumber => this.m_PlayerNumber;
    public int CurrentHealth => this.m_CurrentHealth;

    private void Start()
    {
        m_SkillExpireTime = GameManager.Instance.LevelManager.so_Skill.ExpireDuration;
    }

    /// <summary>
    /// Setup required stuff of the player
    /// Currently only sets the player number and position
    /// </summary>
    /// <param name="playerNumber"></param>
    public void SetupPlayer(int playerNumber)
    {
        this.m_PlayerInput = this.GetComponent<PlayerInput>();
        this.m_PlayerMovement = this.GetComponent<PlayerMovement>();


        // Removing this coz it will be set using the reset player function
        //this.m_PlayerStatus = PlayerStatus.Default;
        //this.m_CurrentHealth = m_MaxHealth;

        this.m_PlayerNumber = playerNumber;
        
        if (playerNumber == 1)
        {
            m_PlayerInput.SwitchCurrentActionMap("PlayerOne");
        } else if (playerNumber == 2)
        {
            m_PlayerInput.SwitchCurrentActionMap("PlayerTwo");
        }

    }

    public void ResetPlayer()
    {
        this.m_PlayerStatus = PlayerStatus.Immobilized;
        this.m_CurrentHealth = m_MaxHealth;

        for (int i = 0; i < m_PlayerSkills.Length; i++)
        {
            m_PlayerSkills[i] = -1;
        }
    }

    /// <summary>
    /// Used mostly for pausing and resuming player actions
    /// </summary>
    public void EnablePlayer(bool enable)
    {
        if (enable)
            this.m_PlayerStatus = PlayerStatus.Default;
        else
            this.m_PlayerStatus = PlayerStatus.Immobilized;
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

    private void OnPause(InputValue value)
    {
        if (value.isPressed)
            GameManager.Instance.OnPause();
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

    // <summary>Allow player to gain new skill
    public bool GetNewSkill(int skillIdx)
    {

        for (int i = 0; i < 3; i++)
        {
            if (m_PlayerSkills[i] == -1)
            {
               
                GameManager.Instance.UIManager.OnSkillChange(m_PlayerNumber, skillIdx, i);
                m_PlayerSkills[i] = skillIdx;

                StartCoroutine(SkillExpire(i));
                return true;
            }
        }

        return false;
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

            m_PlayerSkills[playerSkillIdx] = -1;
            GameManager.Instance.UIManager.OnSkillUsed(m_PlayerNumber, playerSkillIdx);
        }
    }

    private IEnumerator RemoveImmunity()
    {
        yield return new WaitForSeconds(3f);
        this.m_PlayerStatus = PlayerStatus.Default;
    }

    private IEnumerator SkillExpire(int skillSlot)
    {

        yield return new WaitForSeconds(m_SkillExpireTime);

        m_PlayerSkills[skillSlot] = -1;
        GameManager.Instance.UIManager.OnSkillExpire(m_PlayerNumber, skillSlot);
    }
}
