using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [SerializeField] private int m_MaxHealth = 100;

    // TODO: change to a suitable datatype later on
    [SerializeField] private GameObject[] m_PlayerSkill; // for skill inventory 
    [SerializeField] private GameObject[] m_StatusEffects; // for stun/knockback

    private int m_CurrentHealth;
    private bool m_IsAlive;

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
        m_IsAlive = true;
        m_CurrentHealth = m_MaxHealth;
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
