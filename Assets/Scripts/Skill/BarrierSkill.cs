using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

[CreateAssetMenu(fileName = "BarrierSkill", menuName = "ScriptableObjects/Barrier Skill")]
public class BarrierSkill : AbstractSkill
{
    public float Duration;

    public override void OnPress(Player player)
    {
        LevelManager levelManager = GameManager.Instance.LevelManager;
        VisualEffect vfx = levelManager.VisualEffectPool.GetNextObject();

        vfx.enabled = true;
        vfx.visualEffectAsset = this.CastFX;
        vfx.Play();

        // TODO: set player state to immune

        player.StartCoroutine(this.FollowPlayerRoutine(player, vfx));
        player.StartCoroutine(this.CleanupRoutine());
    }

    private IEnumerator FollowPlayerRoutine(Player player, VisualEffect vfx)
    {
        Transform playerTrans = player.transform;
        float startTime = Time.time;

        while (Time.time - startTime < this.CastTime)
        {
            vfx.transform.SetPositionAndRotation(playerTrans.position, playerTrans.rotation);
            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator CleanupRoutine()
    {
        yield return new WaitForSeconds(this.CastTime);

        // TODO: set player state back to normal
    }
}
