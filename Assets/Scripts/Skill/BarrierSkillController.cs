using UnityEngine;

public class BarrierSkillController : MonoBehaviour
{
    public BarrierSkill barrierSkill;

    private void OnTriggerEnter(Collider other)
    {
        Player player = other.GetComponent<Player>();
        if (player != null)
        {
            this.barrierSkill.OnPress(player);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Player player = other.GetComponent<Player>();
        if (player != null)
        {
            this.barrierSkill.OnRelease();
        }
    }
}
