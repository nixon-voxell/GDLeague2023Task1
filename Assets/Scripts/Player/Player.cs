using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.VFX;
using Unity.Mathematics;

public enum PlayerState
{
    Default,
    Immobilized,
    Dead,
}

public class Player : MonoBehaviour
{
    [SerializeField] private int m_MaxHealth = 100;
    [SerializeField] private PlayerMovement m_PlayerMovement;
    [SerializeField] private AbilitySO m_DashSO;
    [SerializeField] private AbilitySO m_KnockbackSO;
    [SerializeField] private VisualEffect m_VFX;

    private PlayerInput m_PlayerInput;

    private PlayerState m_PlayerState = PlayerState.Default;
    private int m_PlayerNumber;
    private int m_CurrentHealth;
    private float m_SkillExpireTime;
    private bool m_CanDash;
    private bool m_CanKnockback;
    private IEnumerator[] m_SkillExpiryCoroutine = new IEnumerator[3]; 
    private bool m_Immune = false;

    // index of skill in scriptable obejct
    private int[] m_PlayerSkills = new int[3];

    public PlayerMovement PlayerMovement => this.m_PlayerMovement;
    public PlayerState PlayerState => this.m_PlayerState;
    public int PlayerNumber => this.m_PlayerNumber;
    public int CurrentHealth => this.m_CurrentHealth;
    public bool Immune => this.m_Immune;

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
        this.m_PlayerState = PlayerState.Immobilized;
        this.m_CanDash = true;
        this.m_CanKnockback = true;
        GameManager.Instance.UIManager.OnAbilityDoneCD(m_PlayerNumber, "DASH");
        GameManager.Instance.UIManager.OnAbilityDoneCD(m_PlayerNumber, "KNOCKBACK");

        this.m_CurrentHealth = m_MaxHealth;
        this.m_Immune = false;
        this.m_PlayerMovement.SetMoveDirection(float2.zero);

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
            this.m_PlayerState = PlayerState.Default;
        else
            this.m_PlayerState = PlayerState.Immobilized;
    }


    private void OnMovement(InputValue value)
    {
        // move only when status is default
        if (this.m_PlayerState != PlayerState.Default) return;

        float2 moveValue = value.Get<Vector2>();
        this.m_PlayerMovement.SetMoveDirection(moveValue);
    }

    private void OnDash(InputValue value)
    {
        if (this.m_PlayerState != PlayerState.Default) return;

        if (value.isPressed && m_CanDash)
        {
            this.StartCoroutine(this.m_PlayerMovement.Dash());
            m_CanDash = false;
            GameManager.Instance.UIManager.OnAbilityUsed(m_PlayerNumber, "DASH");
            StartCoroutine(AbilityDashCD());
        }
    }

    private void OnKnockback(InputValue value)
    {
        if (this.m_PlayerState != PlayerState.Default) return;

        if (value.isPressed && m_CanKnockback)
        {
            Transform trans = this.transform;
            this.m_VFX.Play();

            RaycastHit hit;
            if (Physics.SphereCast(
                trans.position, this.m_KnockbackSO.Radius, this.transform.forward,
                out hit, this.m_KnockbackSO.Range
            )){
                Player otherPlayer = hit.collider.GetComponent<Player>();
                Debug.Log(hit.collider.name);
                if (otherPlayer != null && otherPlayer != this && otherPlayer.Immune == false)
                {
                    otherPlayer.m_PlayerMovement.SetVelocity(trans.forward * this.m_KnockbackSO.Force);
                    this.StartCoroutine(otherPlayer.Knockback());
                }
            }

            // ability cooldown
            m_CanKnockback = false;
            GameManager.Instance.UIManager.OnAbilityUsed(m_PlayerNumber, "KNOCKBACK");
            StartCoroutine(AbilityKnockbackCD());
        }
    }

    public IEnumerator Knockback()
    {
        this.m_PlayerState = PlayerState.Immobilized;
        float defaultDamping = this.m_PlayerMovement.Damping;
        this.m_PlayerMovement.SetDamping(defaultDamping * 2);

        yield return new WaitForSeconds(this.m_KnockbackSO.Duration);
        this.m_PlayerState = PlayerState.Default;
        this.m_PlayerMovement.SetDamping(defaultDamping);
    }

    private void OnSkillOne(InputValue value)
    {
        if (this.m_PlayerState != PlayerState.Default) return;
        if (!value.isPressed) return;

        this.ActivateSkillIfExist(0);
    }

    private void OnSkillTwo(InputValue value)
    {
        if (this.m_PlayerState != PlayerState.Default) return;
        if (!value.isPressed) return;

        this.ActivateSkillIfExist(1);
    }

    private void OnSkillThree(InputValue value)
    {
        if (this.m_PlayerState != PlayerState.Default) return;
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
        this.m_PlayerState = PlayerState.Dead;
        GameManager.Instance.OnRoundEnd(m_PlayerNumber == 1 ? 2 : 1);
    }

    public void Damage(int damage)
    {
        // can only damage when status is default
        if (this.m_PlayerState != PlayerState.Default) return;
        // cannot damage if player is immune
        if (this.Immune == true) return;

        this.SetHealth(this.CurrentHealth - damage);
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

                m_SkillExpiryCoroutine[i] = SkillExpire(i);

                StartCoroutine(m_SkillExpiryCoroutine[i]);
                return true;
            }
        }

        return false;
    }

    private void ActivateSkillIfExist(int playerSkillIdx)
    {
        int skillIdx = this.GetSkill(playerSkillIdx);
        if (skillIdx != -1)
        {
            GameManager.Instance.LevelManager.so_Skill.Skills[skillIdx].OnPress(this);

            m_PlayerSkills[playerSkillIdx] = -1;
            StopCoroutine(m_SkillExpiryCoroutine[playerSkillIdx]);
            GameManager.Instance.UIManager.OnSkillUsed(m_PlayerNumber, playerSkillIdx);
        }
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

    public void SetImmune(bool immune)
    {
        this.m_Immune = immune;
    }

    private IEnumerator SkillExpire(int skillSlot)
    {
        yield return new WaitForSeconds(m_SkillExpireTime);

        m_PlayerSkills[skillSlot] = -1;
        GameManager.Instance.UIManager.OnSkillExpire(m_PlayerNumber, skillSlot);
    }

    private IEnumerator AbilityDashCD()
    {
        yield return new WaitForSeconds(this.m_DashSO.CooldownTime);
        GameManager.Instance.UIManager.OnAbilityDoneCD(m_PlayerNumber, "DASH");
        m_CanDash = true;
    }

    private IEnumerator AbilityKnockbackCD()
    {
        yield return new WaitForSeconds(this.m_KnockbackSO.CooldownTime);
        GameManager.Instance.UIManager.OnAbilityDoneCD(m_PlayerNumber, "KNOCKBACK");
        m_CanKnockback = true;
    }
}
