using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_PlayMusic : MonoBehaviour
{
    [SerializeField] string MusicName;

    private void Start()
    {
        GameManager.Instance.SoundManager.PlayMusic(MusicName);
    }
}
