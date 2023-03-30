using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

[CreateAssetMenu(fileName = "WindSlashSkill", menuName = "ScriptableObjects/Wind Slash Skill")]
public class WindSlashSkill : AbstractSkill
{
    public float Radius;
    public float CastDelay;
    public Vector3 DmgPosOffset;

    public override void OnPress(Player player)
    {
        player.StartCoroutine(Attack(player));
    }

    private IEnumerator Attack(Player player)
    {
        Transform playerTrans = player.transform;
        VisualEffect vfx = GameManager.Instance.LevelManager.VisualEffectPool.GetNextObject();

        vfx.enabled = true;
        vfx.visualEffectAsset = CastFX;

        Vector3 euler = playerTrans.rotation.eulerAngles;
        euler.y += 180.0f;

        vfx.transform.SetPositionAndRotation(
            playerTrans.position + this.PositionOffset,
            Quaternion.Euler(euler)
        );
        vfx.Play();

        GameManager.Instance.SoundManager.PlayOneShot("sfx_windslash");

        yield return new WaitForSeconds(CastDelay);

        Collider[] collider = Physics.OverlapSphere(playerTrans.position + DmgPosOffset, Radius);

        for (int i = 0; i < collider.Length; i++)
        {
            if (collider[i].CompareTag("Player"))
            {
                Player playerHit = collider[i].GetComponent<Player>();
                if (playerHit != player)
                    playerHit.Damage(Damage);
            }
            else if (collider[i].CompareTag("Destructable"))
            {
                collider[i].GetComponent<DestructableObstacle>().DestroyObstacle();
            }
        }

        CleanupRoutine(vfx);
    }

    private IEnumerator CleanupRoutine(VisualEffect vfx)
    {
        yield return new WaitForSeconds(this.CastTime);

        vfx.Stop();
        vfx.enabled = false;
        vfx.visualEffectAsset = null;
    }
}
