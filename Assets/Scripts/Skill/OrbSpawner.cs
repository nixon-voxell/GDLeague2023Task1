using System.Collections;
using UnityEngine;

public class OrbSpawner : MonoBehaviour
{
    [SerializeField] private float m_RespawnInterval;
    [SerializeField] private int m_SkillIdx;
    [SerializeField] private GameObject m_OrbPrefab;
    private GameManager gameManager;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();

        if (gameManager == null)
        {
            Debug.LogError("Could not find GameManager in the scene!");
        }

        StartCoroutine(Respawn());
    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(m_RespawnInterval);

        int numSkills = gameManager.LevelManager.so_Skill.Skills.Length;

        m_SkillIdx = Random.Range(0, numSkills);

        Instantiate(m_OrbPrefab, transform.position, Quaternion.identity);

        StartCoroutine(Respawn());
    }
}
