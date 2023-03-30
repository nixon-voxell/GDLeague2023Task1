using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

[CreateAssetMenu(fileName = "LightningSkill", menuName = "ScriptableObjects/Lightning Skill")]
public class LightningSkill : AbstractSkill
{
    public float Range;

    public override void OnPress(Player player)
    {
        Vector3 position = player.transform.position;
        Vector3 direction = player.transform.forward;

        LayerMask playerLayer = GameManager.Instance.LevelManager.so_Skill.PlayerLayer;

        RaycastHit hit;
        Physics.Raycast(position, direction, out hit, this.Range);

        LevelManager levelManager = GameManager.Instance.LevelManager;
        VisualEffect vfx = levelManager.VisualEffectPool.GetNextObject();

        vfx.transform.position = hit.point + this.PositionOffset;
        vfx.enabled = true;
        vfx.visualEffectAsset = this.CastFX;
        vfx.Play();

        player.StartCoroutine(this.CleanupRoutine(hit, vfx));
    }

    private IEnumerator CleanupRoutine(RaycastHit initHit, VisualEffect vfx)
    {
        yield return new WaitForSeconds(this.CastTime);

        LevelManager levelManager = GameManager.Instance.LevelManager;

        // cleanup vfx -> stop and disable
        vfx.Stop();
        vfx.enabled = false;
        vfx.visualEffectAsset = null;

        // check if it hits anything
        RaycastHit hit;
        if (Physics.Raycast(initHit.point + this.PositionOffset, Vector3.down, out hit))
        {
            Player player = hit.collider.GetComponent<Player>();
            if (player != null)
            {
                player.Damage(this.Damage);
                yield break;
            }

            DestructableObstacle obstacle = hit.collider.GetComponent<DestructableObstacle>();
            if (obstacle != null)
            {
                obstacle.DestroyObstacle();
                yield break;
            }
        }
    }
}
