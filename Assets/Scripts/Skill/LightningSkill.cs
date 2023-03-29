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

        vfx.transform.position = hit.point;
        vfx.enabled = true;
        vfx.visualEffectAsset = this.CastFX;
        vfx.Play();

        // player.StartCoroutine()
    }
}
