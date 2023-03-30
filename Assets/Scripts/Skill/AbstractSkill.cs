using UnityEngine;
using UnityEngine.VFX;

public abstract class AbstractSkill : ScriptableObject
{
    public float CastTime;
    public int Damage;
    public Vector3 PositionOffset;

    public GameObject OrbPrefab;
    public VisualEffectAsset CastFX;
    public Sprite SkillIcon;
    public string FxSound;
    public virtual void OnPress(Player player) {}
    public virtual void OnRelease() {}
}
