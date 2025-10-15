using System;
using UnityEngine;

[Serializable]
public class Sound
{
    public string Name;
    public AudioClip Clip;

    [Range(0f, 1f)] public float Volume;
    [Range(0f, 1.5f)] public float Pitch;

    public bool loop = false;

    [HideInInspector] public AudioSource Source;
}
