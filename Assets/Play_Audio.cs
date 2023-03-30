using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Play_Audio : MonoBehaviour
{
    public string AudioName;

    public void PlaySound()
    {
        GameManager.Instance.SoundManager.PlayOneShot(AudioName);

    }
}
