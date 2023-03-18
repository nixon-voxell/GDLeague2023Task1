using UnityEngine;

public abstract class AbstractSkill : ScriptableObject
{
    public virtual void OnPress() {}
    public virtual void OnRelease() {}
}
