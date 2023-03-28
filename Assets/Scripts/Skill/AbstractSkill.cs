using UnityEngine;
using UnityEngine.VFX;

public abstract class AbstractSkill : ScriptableObject
{
    public float CastTime;
    public float Damage;

    public VisualEffectAsset OrbVFX;
    public VisualEffectAsset CastVFX;
    public Sprite SkillIcon;    

    public virtual void OnPress(Player player) {}
    public virtual void OnRelease() {}
}
