using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SkillScriptableObject", menuName = "ScriptableObjects/Skill")]
public class SkillSO : ScriptableObject
{
    /// <summary>
    /// Contains all skills within the game. Generated at runtime :(
    /// </summary>
    [SerializeField]
    public List<Skill> Skills;

    private void Awake()
    {
        //Skills.Add();
    }
}
