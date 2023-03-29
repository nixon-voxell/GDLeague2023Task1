using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

[CreateAssetMenu(fileName = "KnifeSkill", menuName = "ScriptableObjects/Knife Skill")]
public class KnifeSkill : AbstractSkill
{
    public float Range;
    public float Radius;

    public override void OnPress(Player player)
    {
        Vector3 position = player.transform.position;
        Vector3 direction = player.transform.forward;

        LayerMask playerLayer = GameManager.Instance.LevelManager.so_Skill.PlayerLayer;

        LevelManager levelManager = GameManager.Instance.LevelManager;
        VisualEffect vfx = levelManager.VisualEffectPool.GetNextObject();

        Transform playerTrans = player.transform;
        vfx.transform.SetPositionAndRotation(playerTrans.position, playerTrans.rotation);
        vfx.enabled = true;
        vfx.visualEffectAsset = this.CastFX;
        vfx.Play();

        player.StartCoroutine(this.CleanupRoutine(player, vfx));
    }

    private IEnumerator CleanupRoutine(Player player, VisualEffect vfx)
    {
        yield return new WaitForSeconds(this.CastTime);

        LevelManager levelManager = GameManager.Instance.LevelManager;

        // cleanup vfx -> stop and disable
        vfx.Stop();
        vfx.enabled = false;
        vfx.visualEffectAsset = null;

        Transform playerTrans = player.transform;
        // check if it hits anything
        RaycastHit hit;
        if (Physics.SphereCast(
            playerTrans.position + this.PositionOffset,
            this.Radius, playerTrans.forward,
            out hit, this.Range
        )) {
            Player otherPlayer = hit.collider.GetComponent<Player>();
            if (otherPlayer != null && otherPlayer != player)
            {
                otherPlayer.Damage(this.Damage);
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
