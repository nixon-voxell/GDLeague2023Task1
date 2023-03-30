using System.Collections;
using UnityEngine;

public class OrbSpawner : MonoBehaviour
{
    [SerializeField] private float m_RespawnInterval;
    private int m_SkillIdx; // I believe this supposed to be not seen by the game designers tho
    private GameObject m_SkillOrb;

    private void Start()
    {
        // add to LevelManager reference
        GameManager.Instance.LevelManager.OrbSpawners.Add(this);
    }

    /// <summary>Enable spawner.</summary>
    public void EnableSpawn()
    {
        StartCoroutine(Respawn(0.0f));
    }

    /// <summary>Disable spawner.</summary>
    public void DisableSpawn()
    {
        if (this.m_SkillOrb != null)
        {
            Object.Destroy(this.m_SkillOrb);
        }
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (m_SkillOrb == null)
                return;


            bool AbleToGainSkill = other.GetComponent<Player>().GetNewSkill(m_SkillIdx);
            if (AbleToGainSkill)
                GetSkill();
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(this.transform.position, 0.5f);
    }
}
