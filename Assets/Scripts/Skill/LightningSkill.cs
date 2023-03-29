using UnityEngine;
using UnityEngine.VFX;

[CreateAssetMenu(fileName = "LightningSkill", menuName = "ScriptableObjects/Lightning Skill")]
public class LightningSkill : AbstractSkill
{
    public int Range;

    public override void OnPress(Player player)
    {
        Vector3 position = player.transform.position;
        Vector3 direction = player.transform.forward;

        LayerMask playerLayer = GameManager.Instance.LevelManager.so_Skill.PlayerLayer;

        RaycastHit hit;
        Physics.Raycast(position, direction, out hit, Range);

        LevelManager levelManager = GameManager.Instance.LevelManager;
        VisualEffect vfx = levelManager.VisualEffectPool.GetNextObject();

        vfx.enabled = true;
        vfx.visualEffectAsset = this.CastFX;

        // foreach (RaycastHit hit in hits)
        // {
        //     Player opponent = hit.collider.GetComponent<Player>();
        //     hit.collider.
        //     if (opponent != null)
        //     {
        //         opponent.Damage((int)Damage);
        //     }

        //     if (CastFX != null)
        //     {
        //         VisualEffect castEffect = new GameObject().AddComponent<VisualEffect>();
        //         // castEffect.visualEffectAsset = CastPrefab;
        //         castEffect.transform.position = hit.point;
        //         castEffect.Play();
        //     }
        // }
    }
}
