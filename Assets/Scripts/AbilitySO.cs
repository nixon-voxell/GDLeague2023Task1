using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AbilityScriptableObject", menuName = "ScriptableObjects/Ability")]
public class AbilitySO : ScriptableObject
{
    public string AbilityName;
    public float CooldownTime;
    public Sprite AbilityActiveIcon;
    public Sprite AbilityCDIcon;
    public AbilityParameter[] Parameters;
}

[System.Serializable]
public class AbilityParameter
{
    public string ParameterName;
    public float ParameterValue;
}