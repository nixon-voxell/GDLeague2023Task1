using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

[CreateAssetMenu(fileName = "PortalSkill", menuName = "ScriptableObjects/Portal Skill")]
public class PortalSkill : AbstractSkill
{
    public float TPRange;
    public VisualEffectAsset ArriveFx;

    public override void OnPress(Player player)
    {
        Transform playerTrans = player.transform;
        Vector3 playerPosition = playerTrans.position;
        Vector3 forwardDirection = player.transform.forward;
        Vector3 offset = Vector3.zero;
        Vector3 positionToTP = Vector3.zero;

        const int MAX_ITERATION = 100;
        int iteration = 0;
        bool canTp = true;

        float playerRadius = player.GetComponent<SphereCollider>().radius;
        float halfPlayerRadius = playerRadius * 0.5f;

        while (true && iteration++ < MAX_ITERATION)
        {
            positionToTP = playerPosition + (forwardDirection * TPRange) + offset;

            if (Vector3.Distance(positionToTP, playerPosition) < playerRadius)
            {
                positionToTP = playerPosition;
                break;
            }

            Collider[] collider = Physics.OverlapSphere(positionToTP, playerRadius);

            if (collider.Length == 0 || collider == null)
            {
                break;
            }

            for (int i = 0; i < collider.Length; i++)
            {
                if (collider[i].CompareTag("Obstacle") || collider[i].CompareTag("Offmap"))
                {
                    offset -= forwardDirection * halfPlayerRadius;
                    continue;
                }
            }
        }
        
        
        // Original pos FX
        VisualEffect vfx = GameManager.Instance.LevelManager.VisualEffectPool.GetNextObject();
        vfx.transform.position = playerTrans.position;
        vfx.transform.rotation = Quaternion.identity;
        vfx.enabled = true;
        vfx.visualEffectAsset = CastFX;
        vfx.Play();
        GameManager.Instance.SoundManager.PlayOneShot(FxSound);


        VisualEffect tpVfx = GameManager.Instance.LevelManager.VisualEffectPool.GetNextObject();

        if (canTp)
        {
            // Destination pos FX
            tpVfx.transform.position = positionToTP + PositionOffset;
            //Force height to player height
            tpVfx.enabled = true;
            tpVfx.visualEffectAsset = ArriveFx;
            tpVfx.transform.rotation = Quaternion.identity;

            tpVfx.Play();

            playerTrans.position = new Vector3(positionToTP.x, playerTrans.position.y, positionToTP.z);
            playerTrans.GetComponent<Player>().PlayerMovement.SetTransform(playerTrans);
        }
        

        player.StartCoroutine(CleanupRoutine(vfx));
        player.StartCoroutine(CleanupRoutine(tpVfx));
    }

    private IEnumerator CleanupRoutine (VisualEffect vfx)
    {
        yield return new WaitForSeconds(CastTime);

        vfx.Stop();
        vfx.enabled = false;
        vfx.visualEffectAsset = null;
    }
}
