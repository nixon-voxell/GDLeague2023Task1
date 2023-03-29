using System.Collections;
using UnityEngine;

public class OrbSpawner : MonoBehaviour
{
    [SerializeField] private float m_RespawnInterval;
    [SerializeField] private int m_SkillIdx;
    private GameObject m_SkillOrb;

    private void Start()
    {
        StartCoroutine(Respawn(0.0f));
    }

    public int GetSkill()
    {
        this.StartCoroutine(this.Respawn(this.m_RespawnInterval));
        return this.m_SkillIdx;
    }

    private IEnumerator Respawn(float interval)
    {
        // destroy previous skill orb
        if (this.m_SkillOrb != null)
        {
            Object.Destroy(this.m_SkillOrb);
        }
        SkillSO so_skill = GameManager.Instance.LevelManager.so_Skill;

        yield return new WaitForSeconds(interval);

        // randomly select a skill
        m_SkillIdx = Random.Range(0, so_skill.Skills.Length);

        // instantiate as child
        GameObject skillOrbPrefab = so_skill.Skills[this.m_SkillIdx].OrbPrefab;
        this.m_SkillOrb = Object.Instantiate(skillOrbPrefab, this.transform);
    }
}
