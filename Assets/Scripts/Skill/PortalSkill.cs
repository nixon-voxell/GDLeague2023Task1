using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

[CreateAssetMenu(fileName = "PortalSkill", menuName = "ScriptableObjects/Portal Skill")]
public class PortalSkill : AbstractSkill
{
    public float TPRange;
    public float CheckRadius;
    public VisualEffectAsset ArriveFx;
    public float ExpandRangeAmt;

    public override void OnPress(Player player)
    {
        Transform playerTrans = player.transform;
        Vector3 position = playerTrans.position;
        Vector3 direction = player.transform.forward;
        Vector3 offset = Vector3.zero;
        Vector3 positionToTP = Vector3.zero;

        bool isFree = false;
        float expandRange = 0;
        bool canTp = true;

        while (!isFree)
        {
            positionToTP = position + (direction * TPRange) + offset;
            Collider[] collider = Physics.OverlapSphere(positionToTP, CheckRadius);


            if (collider.Length > 0)
            {
                for (int i = 0; i < collider.Length; i++)
                {
                    if (collider[i].CompareTag("Offmap"))
                    {
                        canTp = false;
                        break;
                    }
                }

                offset = new Vector3(Random.Range(0.1f + expandRange, 0.3f + expandRange), 0,  Random.Range(0.1f + expandRange, 0.3f + expandRange));
                expandRange += ExpandRangeAmt;

            }
            else
            {
                isFree = true;
            }
        }
        
        
        // Original pos FX
        VisualEffect vfx = GameManager.Instance.LevelManager.VisualEffectPool.GetNextObject();
        vfx.transform.position = playerTrans.position;
        vfx.transform.rotation = Quaternion.identity;
        vfx.enabled = true;
        vfx.visualEffectAsset = CastFX;
        vfx.Play();

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
