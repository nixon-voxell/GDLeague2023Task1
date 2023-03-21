using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private SkillSO m_SkillScriptableObject;

    public SkillSO SkillScriptableObject => this.m_SkillScriptableObject;
}
