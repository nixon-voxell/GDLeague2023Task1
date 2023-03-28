using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Used for knockback and dash
/// </summary>
[System.Serializable]
public class AbilityHUD
{
    public Image AbilityIconHolder;
    public Image Background;
    public AbilitySO AbilitySO;

    private bool m_IsActive;
    private float m_CooldownTime;

    public bool IsActive { get => m_IsActive; set => m_IsActive = value; }
    public float CooldownTime { get => m_CooldownTime; set => m_CooldownTime = value; }
}
