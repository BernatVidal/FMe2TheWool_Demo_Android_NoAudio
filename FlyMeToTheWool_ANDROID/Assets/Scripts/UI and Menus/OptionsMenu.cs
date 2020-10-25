using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    public AudioMixer audioMixerGame;
    public AudioMixer audioMixerUI;

    public Slider fxSlider;
    public Slider musicSlider;
    public Dropdown qualityDropDown;

    private void Start()
    {
        fxSlider.value = GetFXVolume();
        musicSlider.value = GetMusicVolume();
        qualityDropDown.value = GetQuality();
    }

    private float GetFXVolume()
    {
        float value;
        bool result = audioMixerGame.GetFloat("FX", out value);
        if (result)
            return value;
        else return 0f;
    }

    private float GetMusicVolume()
    {
        float value;
        bool result = audioMixerGame.GetFloat("Music", out value);
        if (result)
            return value;
        else return 0f;
    }

    private int GetQuality()
    {
        return QualitySettings.GetQualityLevel();
    }

    public void SetFX(float volume)
    {       
        audioMixerGame.SetFloat("FX", volume);
        audioMixerUI.SetFloat("FXUI", volume);
    }

    public void SetMusic(float volume)
    {
        audioMixerGame.SetFloat("Music", volume);
        audioMixerUI.SetFloat("MusicUI", volume);

    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

}
