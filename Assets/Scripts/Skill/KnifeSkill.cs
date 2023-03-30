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
        LevelManager levelManager = GameManager.Instance.LevelManager;
        VisualEffect vfx = levelManager.VisualEffectPool.GetNextObject();

        vfx.enabled = true;
        vfx.visualEffectAsset = this.CastFX;
        vfx.Play();

        player.StartCoroutine(this.DamageRoutine(player, vfx));
        player.StartCoroutine(this.CleanupRoutine(vfx));
    }

    private IEnumerator DamageRoutine(Player player, VisualEffect vfx)
    {
        Transform playerTrans = player.transform;
        Vector3 euler = playerTrans.rotation.eulerAngles;
        euler.x = 90.0f;
        vfx.transform.rotation = Quaternion.Euler(euler);

        Vector3 forwardDirection = playerTrans.forward;

        float startTime = Time.time;
        bool damaged = false;

        while (Time.time - startTime < this.CastTime)
        {
            vfx.transform.position = playerTrans.position;

            // just move the position if damage has been done
            if (!damaged)
            {
                // check if it hits anything
                RaycastHit hit;
                if (Physics.SphereCast(
                    playerTrans.position + this.PositionOffset,
                    this.Radius, forwardDirection,
                    out hit, this.Range
                )) {
                    Player otherPlayer = hit.collider.GetComponent<Player>();
                    if (otherPlayer != null && otherPlayer != player)
                    {
                        otherPlayer.Damage(this.Damage);
                        damaged = true;
                    }

                    DestructableObstacle obstacle = hit.collider.GetComponent<DestructableObstacle>();
                    if (obstacle != null)
                    {
                        obstacle.DestroyObstacle();
                        damaged = true;
                    }
                }
            }

            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator CleanupRoutine(VisualEffect vfx)
    {
        yield return new WaitForSeconds(this.CastTime);

        LevelManager levelManager = GameManager.Instance.LevelManager;

        // cleanup vfx -> stop and disable
        vfx.Stop();
        vfx.enabled = false;
        vfx.visualEffectAsset = null;
    }
}
