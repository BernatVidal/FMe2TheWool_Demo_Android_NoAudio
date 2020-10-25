using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;
using System;
using UnityEngine.Video;

public static class Sounds
{
    public enum SoundID
    {
        Music,
        CatShot,
        CatMove,
        CatDamaged,
        CatOneLive,
        WoolExplosion,
        DroneMoveKKKKKKKKK,
        DroneShot,
        DroneShotDestruction,
        DogMoveKKKKKKKKKK,
        DogShot,
        DogShotDestruction,
        SmallDroneSpawn,
        DebrisDestruction,
        EnemyHurt,
        EnemyHurt2,
        SpaceshipDestruction,
        PowerUpAppear,
        PowerUpObtained,
        GameOver,
        EndGame,
        Pause,
    }
}

[System.Serializable]
public class Sound
{
    public AudioMixerGroup audioMixerGroup;

    private AudioSource source;

    public Sounds.SoundID soundID; 
   // public string clipName;
    public AudioClip clip;

    [Range (0,2f)]
    public float volume = 1f;
    [Range (0,3f)]
    public float pitch = 1f;

    public bool loop = false;
    public bool playOnAwake = false;



    public void SetSource(AudioSource _source)
    {
        source = _source;
        source.clip = clip;
        source.volume = volume;
        source.pitch = pitch;
        source.loop = loop;
        source.playOnAwake = playOnAwake;
        source.outputAudioMixerGroup = audioMixerGroup;
    }

    public void Play()
    {
        source.Play();
    }

    public void Pause()
    {
        source.Pause();
    }

}

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField]
    Sound[] sound;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }

    private void Start()
    {
        for (int i = 0; i < sound.Length; i ++)
        {
            GameObject _go = new GameObject("sound_" + i + "_" + sound[i].soundID);
            _go.transform.SetParent(this.transform);
            sound[i].SetSource(_go.AddComponent<AudioSource>());
        }

        PlaySound(Sounds.SoundID.Music);
    }


    public void PlaySound(Sounds.SoundID _soundID)
    {
        for (int i = 0; i < sound.Length; i++)
        {
            if (sound[i].soundID == _soundID)
            {
                sound[i].Play();
                return;
            }
        }
    }


    public void PauseSound(Sounds.SoundID _soundID)
    {
        for (int i = 0; i < sound.Length; i++)
        {
            if (sound[i].soundID == _soundID)
            {
                sound[i].Pause();
                return;
            }
        }
    }


    //3D SOUND

    //public static void PlaySound(Sound sound, Vector3 position)
    //{
    //    //if (CanPlaySound(sound))
    //    //{
    //    GameObject soundGameObject = new GameObject("Sound");
    //    soundGameObject.transform.position = position;
    //    AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
    //    audioSource.clip = GetAudiClip(sound);
    //    ///       audioSource.outputAudioMixerGroup = AudioMixer.              <------------------
    //    ////        audioSource.Play();                                           <------------------
    //    //}
    //}


    //private static AudioClip GetAudiClip(Sound sound)
    //{
    //    foreach (GameAssets.SoundAudioClip soundAudioClip in GameAssets.i.soundAudioClipArray)
    //    {
    //        if (soundAudioClip.sound == sound)
    //        {
    //            return soundAudioClip.audioClip;
    //        }
    //    }
    //    Debug.LogError("Sound " + sound + " not found!");
    //    return null;
    //}

}
