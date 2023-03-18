using UnityEngine;

[CreateAssetMenu(fileName = "LightningSkill", menuName = "ScriptableObjects/Lightning Skill")]
public class LightningSkill : AbstractSkill
{
    public float CastingTime;
    public float Damage;
    public float Range;
}
