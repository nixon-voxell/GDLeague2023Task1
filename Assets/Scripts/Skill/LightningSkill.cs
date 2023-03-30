using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

[CreateAssetMenu(fileName = "LightningSkill", menuName = "ScriptableObjects/Lightning Skill")]
public class LightningSkill : AbstractSkill
{
    public float Range;
    public float Radius;

    public override void OnPress(Player player)
    {
        Vector3 position = player.transform.position;
        Vector3 direction = player.transform.forward;

        LayerMask playerLayer = GameManager.Instance.LevelManager.so_Skill.PlayerLayer;

        RaycastHit hit;
        bool isHit = Physics.Raycast(position, direction, out hit, this.Range);

        LevelManager levelManager = GameManager.Instance.LevelManager;
        VisualEffect vfx = levelManager.VisualEffectPool.GetNextObject();

        Vector3 hitLocation;
        if (isHit)
        {
            hitLocation = hit.point;
        } else
        {
            hitLocation = position + direction * this.Range;
        }
        vfx.transform.position = hitLocation + this.PositionOffset;
        vfx.transform.rotation = Quaternion.identity;
        vfx.enabled = true;
        vfx.visualEffectAsset = this.CastFX;
        vfx.Play();
        GameManager.Instance.SoundManager.PlayOneShot(FxSound);


        player.StartCoroutine(this.CleanupRoutine(hitLocation, vfx));
    }

    private IEnumerator CleanupRoutine(Vector3 hitLocation, VisualEffect vfx)
    {
        yield return new WaitForSeconds(this.CastTime);

        LevelManager levelManager = GameManager.Instance.LevelManager;

        // cleanup vfx -> stop and disable
        vfx.Stop();
        vfx.enabled = false;
        vfx.visualEffectAsset = null;

        RaycastHit[] hits = Physics.SphereCastAll(hitLocation + this.PositionOffset, this.Radius, Vector3.down);

        // check if it hits anything
        for (int h = 0; h < hits.Length; h++)
        {
            RaycastHit hit = hits[h];
            Player player = hit.collider.GetComponent<Player>();
            if (player != null)
            {
                player.Damage(this.Damage);
            }

            DestructableObstacle obstacle = hit.collider.GetComponent<DestructableObstacle>();
            if (obstacle != null)
            {
                obstacle.DestroyObstacle();
            }
        }
    }
}
