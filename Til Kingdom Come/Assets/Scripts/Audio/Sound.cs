using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
    [HideInInspector]
    public AudioSource source;
    [Range(0f, 1f)]
    public float volume;
    [Range(.1f, 3)]
    public float pitch;
    public bool loop;
    private AudioMixerGroup outputAudioMixerGroup;
}