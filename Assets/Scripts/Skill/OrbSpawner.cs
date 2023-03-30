using System.Collections;
using UnityEngine;

public class OrbSpawner : MonoBehaviour
{
    [SerializeField] private float m_RespawnInterval;
    private int m_SkillIdx; // I believe this supposed to be not seen by the game designers tho
    private GameObject m_SkillOrb;
    private Coroutine m_Coroutine;

    private void Start()
    {
        // add to LevelManager reference
        GameManager.Instance.LevelManager.OrbSpawners.Add(this);
    }

    /// <summary>Enable spawner.</summary>
    public void EnableSpawn()
    {
        this.m_Coroutine = this.StartCoroutine(Respawn(0.0f));
    }

    /// <summary>Disable spawner.</summary>
    public void DisableSpawn()
    {
        if (this.m_SkillOrb != null)
        {
            Object.Destroy(this.m_SkillOrb);
        }
        if (this.m_Coroutine != null)
        {
            this.StopCoroutine(this.m_Coroutine);
            this.m_Coroutine = null;
        }
    }

    public void ChangeSkillOrb()
    {
        // destroy previous skill orb   
        if (this.m_SkillOrb != null)
        {
            Object.Destroy(this.m_SkillOrb);
        }
        this.m_Coroutine = this.StartCoroutine(this.Respawn(this.m_RespawnInterval));
    }

    private IEnumerator Respawn(float interval)
    {
        if (this.m_Coroutine != null)
        {
            this.StopCoroutine(this.m_Coroutine);
            this.m_Coroutine = null;
        }
        yield return new WaitForSeconds(interval);

        SkillSO so_skill = GameManager.Instance.LevelManager.so_Skill;

        // randomly select a skill
        m_SkillIdx = Random.Range(0, so_skill.Skills.Length);

        // instantiate as child
        GameManager.Instance.SoundManager.PlayOneShot("sfx_orb_spawn");

        GameObject skillOrbPrefab = so_skill.Skills[this.m_SkillIdx].OrbPrefab;
        this.m_SkillOrb = Object.Instantiate(skillOrbPrefab, this.transform);

        this.m_Coroutine = null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (m_SkillOrb == null)
                return;

            bool ableToGainSkill = other.GetComponent<Player>().GetNewSkill(m_SkillIdx);

            if (ableToGainSkill) ChangeSkillOrb();
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(this.transform.position, 0.5f);
    }
}
