using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class SkillHUD 
{
    public Image SkillImage;
    public Slider TimerSlider;
    private bool m_IsActive;
    private float m_ExpireTime;

    public bool IsActive { get => m_IsActive; set => m_IsActive = value; }
    public float ExpireTime { get => m_ExpireTime; set => m_ExpireTime = value; }
}
