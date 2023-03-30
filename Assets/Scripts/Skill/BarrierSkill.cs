using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

[CreateAssetMenu(fileName = "BarrierSkill", menuName = "ScriptableObjects/Barrier Skill")]
public class BarrierSkill : AbstractSkill
{
    public Vector3 ScaleOverride;

    public override void OnPress(Player player)
    {
        LevelManager levelManager = GameManager.Instance.LevelManager;
        VisualEffect vfx = levelManager.VisualEffectPool.GetNextObject();

        vfx.transform.localScale = this.ScaleOverride;
        vfx.enabled = true;
        vfx.visualEffectAsset = this.CastFX;
        vfx.Play();
        GameManager.Instance.SoundManager.PlayOneShot(FxSound);

        player.SetImmune(true);
        player.StartCoroutine(this.FollowPlayerRoutine(player, vfx));
        player.StartCoroutine(this.CleanupRoutine(player, vfx));
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

    private IEnumerator CleanupRoutine(Player player, VisualEffect vfx)
    {
        yield return new WaitForSeconds(this.CastTime);

        player.SetImmune(false);
        vfx.transform.localScale = Vector3.one;
    }
}
