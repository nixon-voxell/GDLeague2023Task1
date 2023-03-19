using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SkillScriptableObject", menuName = "ScriptableObjects/Skill")]
public class SkillSO : ScriptableObject
{
    /// <summary>Contains all skills within the game.</summary>
    [SerializeField] public List<AbstractSkill> Skills;
}
