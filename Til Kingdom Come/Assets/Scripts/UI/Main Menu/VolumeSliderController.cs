﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine;

public class VolumeSliderController : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Slider master, music, soundEffects;
    void Start()
    {
        master.value = PlayerPrefs.GetFloat("Master", 1);
        music.value = PlayerPrefs.GetFloat("Music", 1);
        soundEffects.value = PlayerPrefs.GetFloat("SoundEffects", 1);
    }
    public void SetMasterVolume(float volume)
    {
        audioMixer.SetFloat("Master", linearise(volume));
        PlayerPrefs.SetFloat("Master", volume);
    }
    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("Music", linearise(volume));
        PlayerPrefs.SetFloat("Music", volume);
    }
    public void SetSoundEffectVolume(float volume)
    {
        audioMixer.SetFloat("SoundEffects", linearise(volume));
        PlayerPrefs.SetFloat("SoundEffects", volume);
    }

    private float linearise(float value)
    {
        return Mathf.Log10(value) * 20;
    }
}