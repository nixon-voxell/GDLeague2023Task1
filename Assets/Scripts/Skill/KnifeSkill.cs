using UnityEngine;
using UnityEngine.VFX;

[CreateAssetMenu(fileName = "KnifeSkill", menuName = "ScriptableObjects/Knife Skill")]
public class KnifeSkill : AbstractSkill
{
    public int Range;
    public float StabInterval;
    public float PoisonDuration;
    public float PoisonDamage;

    private Player player;
    private float lastStabTime;
    private int stabCount;
    private float poisonEndTime;

    public override void OnPress(Player player)
    {
        this.player = player;

        // if (OrbPrefab != null)
        // {
        //     VisualEffect orbEffect = new GameObject().AddComponent<VisualEffect>();
        //     orbEffect.visualEffectAsset = OrbPrefab;
        //     orbEffect.transform.position = player.transform.position;
        //     orbEffect.Play();
        // }

        stabCount = 0;
        lastStabTime = Time.time;
        poisonEndTime = 0f;
    }

    private void Update()
    {
        if (stabCount >= 2)
        {
            return;
        }

        float timeSinceLastStab = Time.time - lastStabTime;

        if (timeSinceLastStab >= StabInterval)
        {
            lastStabTime = Time.time;

            Vector3 position = player.transform.position;
            Vector3 direction = player.transform.forward;

            RaycastHit hit;
            if (Physics.Raycast(position, direction, out hit, Range))
            {
                Player opponent = hit.collider.GetComponent<Player>();
                if (opponent != null)
                {
                    opponent.Damage(2);

                    if (poisonEndTime <= Time.time)
                    {
                        poisonEndTime = Time.time + PoisonDuration;
                    }
                }

                if (CastFX != null)
                {
                    VisualEffect castEffect = new GameObject().AddComponent<VisualEffect>();
                    // castEffect.visualEffectAsset = CastPrefab;
                    castEffect.transform.position = hit.point;
                    castEffect.Play();
                }
            }

            stabCount++;
        }
    }

    private void FixedUpdate()
    {
        if (poisonEndTime > Time.time)
        {
            player.Damage(Mathf.RoundToInt(PoisonDamage * Time.fixedDeltaTime));
        }
    }
}
