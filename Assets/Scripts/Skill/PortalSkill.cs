using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

[CreateAssetMenu(fileName = "PortalSkill", menuName = "ScriptableObjects/Portal Skill")]
public class PortalSkill : AbstractSkill
{
    public VisualEffect StartVFX;
    public VisualEffect EndVFX;

    public override void OnPress(Player player)
    {
        Transform playerTrans = player.transform;
    }
}
