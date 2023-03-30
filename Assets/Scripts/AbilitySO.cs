using UnityEngine;

[CreateAssetMenu(fileName = "AbilityScriptableObject", menuName = "ScriptableObjects/Ability")]
public class AbilitySO : ScriptableObject
{
    public string AbilityName;
    public float CooldownTime;
    public Sprite AbilityActiveIcon;
    public Sprite AbilityCDIcon;

    public float Range;
    public float Force;
    public float Radius;
    public float Duration;

    // public AbilityParameter[] Parameters;
}

// [System.Serializable]
// public class AbilityParameter
// {
//     public string ParameterName;
//     public float ParameterValue;
// }