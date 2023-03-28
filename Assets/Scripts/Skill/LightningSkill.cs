using UnityEngine;
using UnityEngine.VFX;

[CreateAssetMenu(fileName = "LightningSkill", menuName = "ScriptableObjects/Lightning Skill")]
public class LightningSkill : AbstractSkill
{
    public int Range;

    private VisualEffect orbEffect;
    private Player player;

    public override void OnPress(Player player)
    {
        this.player = player;

        if (OrbVFX != null)
        {
            orbEffect = new GameObject().AddComponent<VisualEffect>();
            orbEffect.visualEffectAsset = OrbVFX;
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
            Player opponent = hit.collider.GetComponent<Player>();
            if (opponent != null)
            {
                opponent.Damage((int)Damage);
            }

            if (CastVFX != null)
            {
                VisualEffect castEffect = new GameObject().AddComponent<VisualEffect>();
                castEffect.visualEffectAsset = CastVFX;
                castEffect.transform.position = hit.point;
                castEffect.Play();
            }
        }
    }
}
