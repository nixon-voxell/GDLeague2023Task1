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
        //testSphere.transform.localScale 

        
        //RaycastHit[] hits = Physics.RaycastAll(position, direction, Range);

        //foreach (RaycastHit hit in hits)
        //{
        //    float distance = Vector3.Distance(position, hit.point);
        //    float damage = CloseRangeDamage;

        //    if (distance > 2 * Range / 3)
        //    {
        //        damage = FarRangeDamage;
        //    }
        //    else if (distance > Range / 3)
        //    {
        //        damage = MidRangeDamage;
        //    }

        //    Player opponent = hit.collider.GetComponent<Player>();
        //    if (opponent != null)
        //    {
        //        opponent.Damage((int)damage);
        //    }

        //}
    }

    private IEnumerator Attack(Player player)
    {

        VisualEffect vfx = GameManager.Instance.LevelManager.VisualEffectPool.GetNextObject();

        vfx.enabled = true;
        vfx.visualEffectAsset = CastFX;
        vfx.transform.position = player.transform.position + PositionOffset;
        vfx.transform.rotation = Quaternion.identity;
        vfx.transform.rotation = Quaternion.Euler(new Vector3(0, 90, 0));
        vfx.Play();

        yield return new WaitForSeconds(CastDelay);

        GameManager.Instance.SoundManager.PlayOneShot(FxSound);

        Collider[] collider = Physics.OverlapSphere(player.transform.position + DmgPosOffset, Radius);

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
