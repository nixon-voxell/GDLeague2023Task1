using UnityEngine;
using UnityEngine.VFX;

public class BarrierSkill : AbstractSkill
{
    public float Duration;

    private VisualEffect barrierEffect;
    private Player player;

    public override void OnPress(Player player)
    {
        this.player = player;

        if (OrbVFX != null)
        {
            VisualEffect orbEffect = new GameObject().AddComponent<VisualEffect>();
            orbEffect.visualEffectAsset = OrbVFX;
            orbEffect.transform.position = player.transform.position;
            orbEffect.Play();
        }

        if (barrierEffect == null && Duration > 0f)
        {
            barrierEffect = player.gameObject.AddComponent<VisualEffect>();
            barrierEffect.visualEffectAsset = CastVFX;
            barrierEffect.Play();

            player.SetImmune(true);
        }
    }

    public override void OnRelease()
    {
        if (barrierEffect != null)
        {
            barrierEffect.Stop();
            Destroy(barrierEffect.gameObject);
        }

        player.SetImmune(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (player == null || barrierEffect == null)
        {
            return;
        }

        Player opponent = collision.collider.GetComponent<Player>();
        if (opponent != null)
        {
            /*    Not sure if you want to use or not
             *    Vector3 knockbackDirection = (opponent.transform.position - player.transform.position).normalized;
             *    knockbackDirection.y = 0f;
             *    opponent.GetComponent<Rigidbody>().AddForce(knockbackDirection * 100f, ForceMode.Impulse);
            */
        }
    }
}
