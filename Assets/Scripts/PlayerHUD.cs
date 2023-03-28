using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class PlayerHUD
{
    public SkillHUD[] SkillsHUD = new SkillHUD[3];
    public AbilityHUD KnockbackHUD;
    public AbilityHUD DashHUD;
    public Slider HPBarHUD;

}
