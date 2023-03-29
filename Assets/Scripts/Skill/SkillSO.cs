using UnityEngine;

[CreateAssetMenu(fileName = "SkillScriptableObject", menuName = "ScriptableObjects/Skill")]
public class SkillSO : ScriptableObject
{
    [Tooltip("Number of seconds before a skill disappear.")]
    public float ExpireDuration;
    public LayerMask PlayerLayer;

    /// <summary>Contains all skills within the game.</summary>
    [SerializeField] public AbstractSkill[] Skills;
}
