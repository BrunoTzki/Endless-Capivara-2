using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSettings : MonoBehaviour
{

    FMOD.Studio.Bus Master;
    FMOD.Studio.Bus SFX;
    FMOD.Studio.Bus Music;


    float masterVolume = 1f;
    float sfxVolume = 0.5f;
    float musicVolume = 0.5f;

    private void Awake()
    {

        Master = FMODUnity.RuntimeManager.GetBus("bus:/Master Bus");
        SFX = FMODUnity.RuntimeManager.GetBus("bus:/Master Bus/SFX");
        Music = FMODUnity.RuntimeManager.GetBus("bus:/Master Bus/Music");
    }

    void Update()
    {
        Master.setVolume(masterVolume);
        SFX.setVolume(sfxVolume);
        Music.setVolume(musicVolume);
    }

    public void MasterVolumeLevel(float newMasterVolume)
    {
        masterVolume = newMasterVolume;
    }

    public void SFXVolumeLevel(float newSFXVolume)
    {
        sfxVolume = newSFXVolume;
    }

    public void MusicVolumeLevel(float newMusicVolume)
    {
        musicVolume = newMusicVolume;
    }

}
