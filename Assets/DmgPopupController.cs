using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DmgPopupController : MonoBehaviour
{
    public GameObject dmgPopupPf;

    public void OnDamage(int dmg)
    {
        Instantiate(dmgPopupPf, transform).GetComponent<DmgText>().SetDmg(dmg);
    }
}
