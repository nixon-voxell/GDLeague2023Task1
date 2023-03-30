using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DmgText : MonoBehaviour
{
    public float Speed = 2f;
    public float Duration = 2f;
    public TextMeshProUGUI Damage;

    private float timer = 0f;

    public void SetDmg(int dmg)
    {
        Damage.text = "-" + dmg;
        gameObject.SetActive(true);
    }

    void Update()
    {
        //floating
        transform.Translate(Vector3.up * Speed * Time.deltaTime);
        timer += Time.deltaTime;

        if (timer >= Duration)
        {
            Destroy(gameObject);
        }

        transform.rotation = Quaternion.LookRotation(transform.position - Camera.main.transform.position);
    }
}
