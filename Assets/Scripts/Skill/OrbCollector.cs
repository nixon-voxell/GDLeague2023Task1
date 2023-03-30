using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbCollector : MonoBehaviour
{
    public int SkillOrbDX;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
            collision.collider.GetComponent<Player>().GetSkill(SkillOrbDX);
    }
}
