using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Controls the UI in level only
/// </summary>
public class UIManager : MonoBehaviour
{
    [SerializeField] private SkillSO m_SkillSO;
    [SerializeField] private AbilitySO m_DashSO;
    [SerializeField] private AbilitySO m_KnockbackSO;
    [SerializeField] private TextMeshProUGUI m_CountdownText;
    [SerializeField] private PlayerHUD[] m_PlayerHUD = new PlayerHUD[2];
    [SerializeField] private GameObject[] m_Player1VictoryCount = new GameObject[2];
    [SerializeField] private GameObject[] m_Player2VictoryCount = new GameObject[2];

    [Header("Adjustable Parameters")]
    [SerializeField] private Sprite m_SkillEmptyIcon;
    [SerializeField] private float m_TimerInterval; // How many sec for it to decrease/increase cooldown

    private float m_DashTimerUpdateValue;
    private float m_KnockbackTimerUpdateValue;
    private float m_SkillTimerUpdateValue;
    private IEnumerator m_TimerTick;
    private bool IsTimerActive;


    void Start()
    {
        GameManager.Instance.UIManager = this;

        // Calculate per timer update how many to increase
        // If run on update : ((playerHUD.SkillsHUD[i].TimerSlider.value * m_SkillCooldownTime) + m_TimerUpdateTime) / m_SkillCooldownTime;
        // Formula: increase_value = slide max range / (total_time / time_interval)

        m_SkillTimerUpdateValue = 1.0f / (m_SkillSO.ExpireDuration / m_TimerInterval); ;
        m_DashTimerUpdateValue = 1.0f / (m_DashSO.CooldownTime / m_TimerInterval);   
        m_KnockbackTimerUpdateValue = 1.0f / (m_KnockbackSO.CooldownTime / m_TimerInterval);

    }

    //private void OnDisable()
    //{
    //    // Do i ned to remove the reference tho
    //    GameManager.Instance.UIManager = null;
    //}

   
    // ------ [PUBLIC FUNCTIONS] ------

    public void OnSkillChange(int playerNumber, int skillNumber, AbstractSkill skill)
    {
        //int index = items.FindIndex(i => i.ItemName == "Wood");
        // Change the image
        if (skill != null)
        {
            SkillHUD skillHUD = m_PlayerHUD[playerNumber - 1].SkillsHUD[skillNumber - 1];
            // TODO: what the heck is this?
            // AbstractSkill oriSkill =  m_SkillSO.Skills.Find(s => s == skill);
            // skillHUD.SkillImage.sprite = oriSkill.SkillIcon;

            skillHUD.TimerSlider.value = 1;
            skillHUD.IsActive = true;
            skillHUD.ExpireTime = 1;
        }
        else if (skill == null)
        {
            ResetSkillHUD(playerNumber, skillNumber);
        }

    }

    public void OnSkillUsed(int playerNumber, int skillNumber)
    {
        ResetSkillHUD(playerNumber, skillNumber);
    }

    public void OnSkillExpire(int playerNumber, int skillNumber)
    {
        ResetSkillHUD(playerNumber, skillNumber);
    }
    public void OnAbilityUsed(int playerNumber, string abilityName)
    {
        ResetAbilityHUD(playerNumber, abilityName);
    }

    public void OnAbilityDoneCD(int playerNumber, string abilityName)
    {
        SetAbilityActive(playerNumber, abilityName);
    }

    public void SetHealth(int playerNumber, float currentHealth)
    {
        m_PlayerHUD[playerNumber - 1].HPBarHUD.value = currentHealth/100;
    }

    public void SetTimer(int seconds)
    {
        int minutesLabel = seconds / 60;
        int secondLabel = seconds % 60;

        m_CountdownText.text = String.Format("{0} : {1}", minutesLabel, secondLabel);
    }

    public void SetScore(int p1Wins, int p2Wins)
    {
        for (int i = 0; i < p1Wins; i++)
        {
            m_Player1VictoryCount[i].SetActive(true);
        }

        for (int i = 0; i < p2Wins; i++)
        {
            m_Player2VictoryCount[i].SetActive(true);
        }
    }

    // TODO: To set timer tick based on game state
    public void OnGameStart()
    {
        m_TimerTick = StartHUDTick();
        IsTimerActive = true;
        StartCoroutine(m_TimerTick);
    }

    public void OnGameSetup()
    {
        IsTimerActive = false;
        ResetAllUI();
    }


    //---------------

    


    // TODO: To sync with skills and ability timer in their script
    IEnumerator StartHUDTick()
    {
        while (IsTimerActive)
        {
            yield return new WaitForSeconds(m_TimerInterval);
            DecreaseAbilityTimerUI();
            DecreaseSkillTimerUI();
        }
    }




    // ----- [TIMER] -----

    /// <summary>
    /// Sets the skill timer expiry time
    /// </summary>
    private void DecreaseSkillTimerUI()
    {
        for (int i = 0; i < m_PlayerHUD.Length; i++)
        {
            PlayerHUD playerHUD = m_PlayerHUD[i];
            // Decrease skill timer
            for (int j = 0; j < playerHUD.SkillsHUD.Length; j++)
            {
                if (playerHUD.SkillsHUD[j].IsActive == false)
                    continue;

                playerHUD.SkillsHUD[j].ExpireTime = playerHUD.SkillsHUD[j].ExpireTime - m_SkillTimerUpdateValue;
                playerHUD.SkillsHUD[j].TimerSlider.value = playerHUD.SkillsHUD[j].ExpireTime;

                if (playerHUD.SkillsHUD[j].ExpireTime <= 0)
                    ResetSkillHUD(i + 1, j + 1);
            }
        }
    }

    /// <summary>
    /// Sets the ability timer cooldown time
    /// </summary>
    private void DecreaseAbilityTimerUI()
    {
        for (int i = 0; i < m_PlayerHUD.Length; i++)
        {
            PlayerHUD playerHUD = m_PlayerHUD[i];

            if (playerHUD.KnockbackHUD.IsActive == false)
            {
                playerHUD.KnockbackHUD.CooldownTime = playerHUD.KnockbackHUD.CooldownTime + m_KnockbackTimerUpdateValue;
                playerHUD.KnockbackHUD.Background.fillAmount = playerHUD.KnockbackHUD.CooldownTime;

                if (playerHUD.KnockbackHUD.CooldownTime >= 1)
                {
                    SetAbilityActive(i + 1, "KNOCKBACK");
                }
            
            }
            
            if (playerHUD.DashHUD.IsActive == false)
            {
                playerHUD.DashHUD.CooldownTime = playerHUD.DashHUD.CooldownTime + m_DashTimerUpdateValue;
                playerHUD.DashHUD.Background.fillAmount = playerHUD.DashHUD.CooldownTime;

                if (playerHUD.DashHUD.CooldownTime >= 1)
                {
                    SetAbilityActive(i + 1, "DASH");
                }
            }


        }
    }



    private void SetAbilityActive(int playerNumber, string abilityName)
    {
        PlayerHUD playerHUD = m_PlayerHUD[playerNumber - 1];

        if (abilityName == "KNOCKBACK")
        {
            playerHUD.KnockbackHUD.Background.fillAmount = 1;
            playerHUD.KnockbackHUD.IsActive = true;
            playerHUD.KnockbackHUD.AbilityIconHolder.sprite = playerHUD.KnockbackHUD.AbilitySO.AbilityActiveIcon;
        }
        else if (abilityName == "DASH")
        {
            playerHUD.DashHUD.Background.fillAmount = 1;
            playerHUD.DashHUD.IsActive = true;
            playerHUD.DashHUD.AbilityIconHolder.sprite = playerHUD.DashHUD.AbilitySO.AbilityActiveIcon;
        }
    }


    // ----- [RESET] -----


    /// <summary>
    /// Called when Skill Expire and Skill Used
    /// </summary>
    /// <param name="playerNumber"></param>
    /// <param name="skillNumber"></param>
    private void ResetSkillHUD(int playerNumber, int skillNumber)
    {
        SkillHUD skillHUD = m_PlayerHUD[playerNumber - 1].SkillsHUD[skillNumber - 1];
        skillHUD.SkillImage.sprite = m_SkillEmptyIcon;
        skillHUD.TimerSlider.value = 0;
        skillHUD.IsActive = false;
        skillHUD.ExpireTime = 0;
    }

    private void ResetAbilityHUD(int playerNumber, string abilityName)
    {
        PlayerHUD playerHUD = m_PlayerHUD[playerNumber - 1];

        if (abilityName == "KNOCKBACK")
        {
            playerHUD.KnockbackHUD.AbilityIconHolder.sprite = playerHUD.KnockbackHUD.AbilitySO.AbilityCDIcon;
            playerHUD.KnockbackHUD.Background.fillAmount = 0;

            playerHUD.KnockbackHUD.IsActive = false;
            playerHUD.KnockbackHUD.CooldownTime = 0;
        }
        else if (abilityName == "DASH")
        {
            playerHUD.DashHUD.AbilityIconHolder.sprite = playerHUD.DashHUD.AbilitySO.AbilityCDIcon;
            playerHUD.DashHUD.Background.fillAmount = 0;

            playerHUD.DashHUD.IsActive = false;
            playerHUD.DashHUD.CooldownTime = 0;

        }
    }

    private void ResetAllUI()
    {
        ResetAbilityHUD(1, "KNOCKBACK");
        ResetAbilityHUD(1, "DASH");
        ResetAbilityHUD(2, "KNOCKBACK");
        ResetAbilityHUD(2, "DASH");
        ResetSkillHUD(1, 1);
        ResetSkillHUD(1, 2);
        ResetSkillHUD(1, 3);
        ResetSkillHUD(2, 1);
        ResetSkillHUD(2, 2);
        ResetSkillHUD(2, 3);
        SetHealth(1, 100);
        SetHealth(2, 100);

    }
   

}
