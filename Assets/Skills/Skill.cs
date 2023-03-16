using UnityEngine.Events;

[System.Serializable]
public class Skill
{
#region "Members"

    // Generic skill variables, probably not used /shrug
    public float CastingTime;
    public float Damage;
    public float Range;

#endregion

#region "Delegates"

    /// <summary>
    /// Function to be called when the key responsible for this skill is pressed. (Potentially) contain the behavior to be executed.
    /// </summary>
    public UnityEvent OnPress;
    
    /// <summary>
    /// Function to be called when the key responsible for this skill is released. (Potentially) contain the behavior to be executed.
    /// </summary>
    public UnityEvent OnRelease;

#endregion
}