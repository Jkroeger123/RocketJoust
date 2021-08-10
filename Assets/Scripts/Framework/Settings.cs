using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Settings : MonoBehaviour
{
    public AudioMixer mixer;

    public float GetVolume()
    {
        float volume;
        mixer.GetFloat("MasterVolume", out volume);
        return volume;
    }

    public void SetVolume(float vol)
    {
        mixer.SetFloat("MasterVolume", vol);
    }
}
