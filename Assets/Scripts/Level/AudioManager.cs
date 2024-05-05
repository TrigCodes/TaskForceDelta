/***************************************************************
*file: AudioManager.cs
*author: Samin Hossain, An Le, Otto Del Cid, Luis Navarrete, Luis Salazar, Sebastian Cursaro
*class: CS 4700 - Game Development
*assignment: Final Program
*date last modified: 5/6/2024
*
*purpose: This class provide general audio managing
*
****************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    public static AudioManager main;
    [Header("Refrences")]
    [SerializeField] private AudioSource soundObject;
    [SerializeField] private AudioSource musicObject;
    [SerializeField] private AudioMixer audioMixer;

    // function: Start
    // purpose: Called before the first frame update to set default info
    void Start()
    {
        if (main == null)
        {
            main = this;
            DontDestroyOnLoad(gameObject);

            // Set default volumes on start
            SetMasterVolume(0.75f);
            SetSoundFXVolume(0.75f);
            SetMusicVolume(0.75f);
        }
        else
        {
            Destroy(gameObject);
        }

        Instantiate(musicObject, transform.position, Quaternion.identity);
    }

    // function: PlayAudio
    // purpose: play audio
    public void PlayAudio(AudioClip audioClip, Transform spawnTransform, float volume)
    {
        AudioSource audioSource = Instantiate(soundObject, spawnTransform.position, Quaternion.identity);
        audioSource.clip = audioClip;
        audioSource.volume = volume;
        audioSource.Play();
        float clipLength = audioSource.clip.length;
        Destroy(audioSource.gameObject, clipLength);
    }
    // function: SetMasterVolume
    // purpose: set master volume based on level
    public void SetMasterVolume(float level)
    {
        audioMixer.SetFloat("masterVolume", Mathf.Log10(level) * 20f);
    }
    // function: SetSoundFXVolume
    // purpose: Set FX volume based on level
    public void SetSoundFXVolume(float level)
    {
        audioMixer.SetFloat("soundFXVolume", Mathf.Log10(level) * 20f);
    }
    // function: SetMusicVolume
    // purpose: set music volume based on level
    public void SetMusicVolume(float level)
    {
        audioMixer.SetFloat("musicVolume", Mathf.Log10(level) * 20f);
    }
}
