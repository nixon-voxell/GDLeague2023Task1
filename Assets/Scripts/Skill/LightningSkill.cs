using UnityEngine;

[CreateAssetMenu(fileName = "LightningSkill", menuName = "ScriptableObjects/Lightning Skill")]
public class LightningSkill : AbstractSkill
{
    public float Range;

    public override void OnPress(Player player)
    {
        // this.VisualEffect.Play();
    }
}
