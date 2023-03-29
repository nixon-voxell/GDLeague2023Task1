using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string SoundName;
    public AudioClip Clip;

    [Range(0f, 1f)] //Adds slider between the range
    public float Volume = 1f;
    [Range(0f, 1f)]
    public float SpatialBlend;
    [Range(.1f, 3f)]
    public float Pitch = 1f;

}
