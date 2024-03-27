using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource music;
    [SerializeField] private AudioSource motor;
    [SerializeField] private AudioSource crash;
    [SerializeField] private AudioSource coinCollect;

    [SerializeField] private AudioClip[] musics;
    [SerializeField] private AudioClip motorSlowSound;
    [SerializeField] private AudioClip motorMediumSound;
    [SerializeField] private AudioClip motorFastSound;
    [SerializeField] private AudioClip crashSound;
    [SerializeField] private AudioClip coinCollectSound;

    [SerializeField] private Slider masterVolume;
    [SerializeField] private Slider musicVolume;
    [SerializeField] private Slider sfxVolume;
    [SerializeField] private AudioMixer audioMixer;

    private int currentMotorPlaying = 0;
    private int currentMusicIndex = -1;
    // Start is called before the first frame update
    void Start()
    {
        motor.loop = true;

        if(PlayerPrefs.HasKey("MasterVolume"))
            masterVolume.value = PlayerPrefs.GetFloat("MasterVolume");
        if (PlayerPrefs.HasKey("MusicVolume"))
            musicVolume.value = PlayerPrefs.GetFloat("MusicVolume");
        if (PlayerPrefs.HasKey("SFXVolume"))
            sfxVolume.value = PlayerPrefs.GetFloat("SFXVolume");
    }

    // Update is called once per frame
    void Update()
    {
        if (!music.isPlaying)
        {
            PlayNextMusic();
        }
    }

    public void SetVolumes()
    {
        var value = masterVolume.value;
        if (value == -20)
            value = -80;
        audioMixer.SetFloat("MasterVolume", value);
        value = musicVolume.value;
        if (value == -40)
            value = -80;
        audioMixer.SetFloat("MusicVolume", value);
        value = sfxVolume.value;
        if (value == -30)
            value = -80;
        audioMixer.SetFloat("SFXVolume", value);

        PlayerPrefs.SetFloat("MasterVolume", masterVolume.value);
        PlayerPrefs.SetFloat("MusicVolume", musicVolume.value);
        PlayerPrefs.SetFloat("SFXVolume", sfxVolume.value);
    }

    public void PlayMotorSlowSound()
    {
        if(currentMotorPlaying != 1)
        {
            motor.clip = motorSlowSound;
            motor.Play();
            currentMotorPlaying = 1;
        }
        
    }

    public void PlayMotorMediumSound()
    {
        if (currentMotorPlaying != 2)
        {
            motor.clip = motorMediumSound;
            motor.Play();
            currentMotorPlaying = 2;
        }
    }

    public void PlayMotorFastSound()
    {
        if (currentMotorPlaying != 3)
        {
            motor.clip = motorFastSound;
            motor.Play();
            currentMotorPlaying = 3;
        }
    }

    public void StopMotorSound()
    {
        motor.Stop();
    }

    public void ContinueMotorSound()
    {
        motor.Play();
    }

    public void PlayCrashSound()
    {
        crash.PlayOneShot(crashSound);
    }

    public void PlayCoinCollectSound()
    {
        coinCollect.PlayOneShot(coinCollectSound);
    }

    private void PlayNextMusic()
    {
        while (true)
        {
            var r = Random.Range(0, musics.Length);
            if(r != currentMusicIndex)
            {
                currentMusicIndex = r;
                music.clip = musics[currentMusicIndex];
                music.Play();
                break;
            }
        }
        
    }
}
