using UnityEngine;
using UnityEngine.VFX;

[CreateAssetMenu(fileName = "WindSlashSkill", menuName = "ScriptableObjects/Wind Slash Skill")]
public class WindSlashSkill : AbstractSkill
{
    public int Range;
    public float CloseRangeDamage = 10f;
    public float MidRangeDamage = 7f;
    public float FarRangeDamage = 5f;

    private VisualEffect orbEffect;
    private Player player;

    public override void OnPress(Player player)
    {
        this.player = player;

        if (OrbPrefab != null)
        {
            orbEffect = new GameObject().AddComponent<VisualEffect>();
            // orbEffect.visualEffectAsset = OrbPrefab;
            orbEffect.transform.position = player.transform.position;
            orbEffect.Play();
        }
    }

    public override void OnRelease()
    {
        if (orbEffect != null)
        {
            orbEffect.Stop();
            Destroy(orbEffect.gameObject);
        }

        CastSkill();
    }

    private void CastSkill()
    {
        Vector3 position = player.transform.position;
        Vector3 direction = player.transform.forward;

        RaycastHit[] hits = Physics.RaycastAll(position, direction, Range);

        foreach (RaycastHit hit in hits)
        {
            float distance = Vector3.Distance(position, hit.point);
            float damage = CloseRangeDamage;

            if (distance > 2 * Range / 3) {
                damage = FarRangeDamage;
            } else if (distance > Range / 3) {
                damage = MidRangeDamage;
            }

            Player opponent = hit.collider.GetComponent<Player>();
            if (opponent != null)
            {
                opponent.Damage((int)damage);
            }

            if (CastPrefab != null)
            {
                VisualEffect castEffect = new GameObject().AddComponent<VisualEffect>();
                // castEffect.visualEffectAsset = CastPrefab;
                castEffect.transform.position = hit.point;
                castEffect.Play();
            }
        }
    }

    public float TotalDamage {
        get {
            return CloseRangeDamage + MidRangeDamage + FarRangeDamage;
        }
    }
}
