using UnityEngine;
using UnityEngine.VFX;

public abstract class AbstractSkill : ScriptableObject
{
    public float CastTime;
    public float Damage;

    public VisualEffectAsset VisualEffectAsset;

    public virtual void OnPress(Player player) {}
    public virtual void OnRelease() {}
}
