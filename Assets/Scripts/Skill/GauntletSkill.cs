using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

[CreateAssetMenu(fileName = "GauntletSkill", menuName = "ScriptableObjects/Gauntlet Skill")]
public class GauntletSkill : AbstractSkill
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
        GameManager.Instance.SoundManager.PlayOneShot(FxSound);


        Transform playerTrans = player.transform;
        Vector3 euler = playerTrans.rotation.eulerAngles;
        euler.y -= 90.0f;
        euler.z -= 90.0f;
        vfx.transform.rotation = Quaternion.Euler(euler);

        Vector3 forwardDirection = playerTrans.forward;

        float punchTime = this.CastTime / 3.0f;
        player.StartCoroutine(this.PunchRoutine(player, forwardDirection, vfx, punchTime, 3));
        player.StartCoroutine(this.CleanupRoutine(vfx));
    }

    private IEnumerator PunchRoutine(
        Player player, Vector3 forwardDirection,
        VisualEffect vfx, float punchDuration, int punchRemaining
    ) {
        Transform playerTrans = player.transform;
        float startTime = Time.time;
        bool damaged = false;

        while (Time.time - startTime < punchDuration)
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

        punchRemaining -= 1;
        if (punchRemaining > 0)
        {
            player.StartCoroutine(this.PunchRoutine(player, forwardDirection, vfx, punchDuration, punchRemaining));
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
