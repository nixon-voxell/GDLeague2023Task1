using UnityEngine;

[CreateAssetMenu(fileName = "SkillScriptableObject", menuName = "ScriptableObjects/Skill")]
public class SkillSO : ScriptableObject
{
    [SerializeField] public float SkillExpireTime;

    /// <summary>Contains all skills within the game.</summary>
    [SerializeField] public AbstractSkill[] Skills;
}
